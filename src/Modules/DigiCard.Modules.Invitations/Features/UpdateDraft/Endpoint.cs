using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using DigiCard.Contracts.Invitations;
using DigiCard.Contracts.Templates;
using DigiCard.Modules.Invitations.Domain;
using DigiCard.Modules.Invitations.Infrastructure;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace DigiCard.Modules.Invitations.Features.UpdateDraft;

internal static class Endpoint
{
    public static void Map(RouteGroupBuilder group) => group.MapPut("/{id:guid}", Handle);

    private static async Task<IResult> Handle(Guid id, UpdateDraftRequest request, ClaimsPrincipal user,
        HttpContext http, IAntiforgery antiforgery, ITemplateCatalog templates,
        InvitationsDbContext db, TimeProvider clock, CancellationToken ct)
    {
        try { await antiforgery.ValidateRequestAsync(http); }
        catch (AntiforgeryValidationException) { return Results.BadRequest(new { error = "Invalid antiforgery token." }); }

        var owner = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(owner)) return Results.Unauthorized();

        var errors = new List<ValidationResult>();
        if (!Validator.TryValidateObject(request, new ValidationContext(request), errors, true))
            return Results.ValidationProblem(errors.SelectMany(e => e.MemberNames.Select(m => (m, e.ErrorMessage!)))
                .GroupBy(e => e.m).ToDictionary(g => g.Key, g => g.Select(e => e.Item2).ToArray()));

        // Ownership is in the query. A draft that belongs to someone else is simply not found,
        // which is also the right answer to give: it does not confirm the card exists.
        var draft = await db.Drafts.SingleOrDefaultAsync(x => x.Id == id && x.OwnerId == owner, ct);
        if (draft is null) return Results.NotFound();

        var template = await templates.FindAsync(request.TemplateId, ct);
        if (template is null)
            return Results.ValidationProblem(new Dictionary<string, string[]> { ["TemplateId"] = ["قالب معتبر انتخاب کنید."] });

        // The version the browser last read. EF compares it on the UPDATE, so a card changed in
        // another tab is refused rather than silently overwritten.
        if (TryDecode(request.RowVersion, out var seen))
            db.Entry(draft).Property(x => x.RowVersion).OriginalValue = seen;

        var elements = string.IsNullOrWhiteSpace(request.Elements) ? template.Elements : request.Elements;

        draft.Update(request.Title, request.BrideName, request.GroomName, request.Message, request.Address,
            template.Id, template.Version, template.Accent, elements,
            new EventSchedule(request.EventDate, request.EventTime, request.EventEndTime),
            clock.GetUtcNow());

        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException)
        {
            // 409 rather than a merge attempt: the two versions are both someone's work, and the
            // editor is in a better position than this endpoint to say what should happen.
            return Results.Conflict(new { error = "این کارت جای دیگری تغییر کرده است." });
        }

        return Results.Ok(Map(draft));
    }

    private static bool TryDecode(string? value, out byte[] bytes)
    {
        bytes = [];
        if (string.IsNullOrWhiteSpace(value)) return false;

        try
        {
            bytes = Convert.FromBase64String(value);
            return bytes.Length > 0;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    internal static DraftResponse Map(InvitationDraft d) => new(
        d.Id, d.Title, d.BrideName, d.GroomName, d.Message, d.TemplateId, d.TemplateVersion,
        d.Accent, d.CreatedAt, d.Elements, d.Address, d.EventDate, d.EventTime, d.EventEndTime,
        d.UpdatedAt, Convert.ToBase64String(d.RowVersion));
}
