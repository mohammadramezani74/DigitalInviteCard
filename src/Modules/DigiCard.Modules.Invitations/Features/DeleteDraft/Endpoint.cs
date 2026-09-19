using System.Security.Claims;
using DigiCard.Modules.Invitations.Infrastructure;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace DigiCard.Modules.Invitations.Features.DeleteDraft;

internal static class Endpoint
{
    public static void Map(RouteGroupBuilder group) => group.MapDelete("/{id:guid}", async (
        Guid id, ClaimsPrincipal user, HttpContext http, IAntiforgery antiforgery,
        InvitationsDbContext db, CancellationToken ct) =>
    {
        try { await antiforgery.ValidateRequestAsync(http); }
        catch (AntiforgeryValidationException) { return Results.BadRequest(new { error = "Invalid antiforgery token." }); }

        var owner = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(owner)) return Results.Unauthorized();

        // Scoped to the owner in the query, so a guessed id deletes nothing.
        var removed = await db.Drafts
            .Where(x => x.Id == id && x.OwnerId == owner)
            .ExecuteDeleteAsync(ct);

        return removed == 0 ? Results.NotFound() : Results.NoContent();
    });
}
