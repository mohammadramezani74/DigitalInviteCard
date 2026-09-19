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
namespace DigiCard.Modules.Invitations.Features.CreateDraft;
internal static class Endpoint
{
    public static void Map(RouteGroupBuilder group) => group.MapPost("/", Handle);
    private static async Task<IResult> Handle(CreateDraftRequest request, ClaimsPrincipal user,
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
        var template = await templates.FindAsync(request.TemplateId, ct);
        if (template is null) return Results.ValidationProblem(new Dictionary<string, string[]> { ["TemplateId"] = ["قالب معتبر انتخاب کنید."] });
        // The editor's arrangement wins; a client that sends none falls back to the template's.
        var elements = string.IsNullOrWhiteSpace(request.Elements) ? template.Elements : request.Elements;

        var draft = InvitationDraft.Create(owner, request.Title, request.BrideName, request.GroomName,
            request.Message, request.Address, template.Id, template.Version, template.Accent, elements,
            new EventSchedule(request.EventDate, request.EventTime, request.EventEndTime),
            clock.GetUtcNow());
        db.Drafts.Add(draft);
        await db.SaveChangesAsync(ct);
        // Mapped in one place so create and update cannot drift apart in what they hand back.
        return Results.Created($"/api/invitations/{draft.Id}", UpdateDraft.Endpoint.Map(draft));
    }
}
