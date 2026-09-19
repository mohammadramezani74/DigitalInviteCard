using System.Security.Claims;
using DigiCard.Modules.Invitations.Infrastructure;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace DigiCard.Modules.Invitations.Features.DeleteDraft;

/// <summary>
/// The same deletion as <see cref="Endpoint"/>, reached by an ordinary HTML form instead of fetch.
/// "کارت‌های من" is a server-rendered page with no JavaScript on it at all - that is what makes it
/// load instantly on a slow connection - so a delete button there cannot call a DELETE verb.
/// A browser can only GET or POST from a form, and a GET that deletes things is how a link
/// prefetcher or an antivirus browser extension wipes someone's cards for them.
/// </summary>
internal static class FormEndpoint
{
    public static void Map(RouteGroupBuilder group) => group.MapPost("/{id:guid}/delete", async (
        Guid id, ClaimsPrincipal user, HttpContext http, IAntiforgery antiforgery,
        InvitationsDbContext db, CancellationToken ct) =>
    {
        // Validated by hand, as everywhere else in this module, so the rule is visible in the
        // file that needs it rather than assumed from middleware.
        try { await antiforgery.ValidateRequestAsync(http); }
        catch (AntiforgeryValidationException) { return Results.BadRequest(new { error = "Invalid antiforgery token." }); }

        var owner = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(owner)) return Results.Unauthorized();

        await db.Drafts.Where(x => x.Id == id && x.OwnerId == owner).ExecuteDeleteAsync(ct);

        // Redirect rather than render: without it a refresh re-submits the delete, and the browser
        // asks an alarming question about resending the form. A missing card is not an error worth
        // a page of its own either - the list is the answer to "is it gone", so both cases land
        // there. LocalRedirect would refuse an off-site target; this one is a constant anyway.
        return Results.LocalRedirect("/my-cards");
    });
}
