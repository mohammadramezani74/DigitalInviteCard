using System.Security.Claims;
using DigiCard.Contracts.Invitations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DigiCard.Modules.Invitations.Features.ListDrafts;

internal static class Endpoint
{
    public static void Map(RouteGroupBuilder group) => group.MapGet("/", async (
        ClaimsPrincipal user, HttpContext http, IDraftReader reader, CancellationToken ct) =>
    {
        // Private data: never cached by a browser or anything between.
        http.Response.Headers.CacheControl = "no-store";

        var owner = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(owner)) return Results.Unauthorized();

        return Results.Ok(await reader.ListOwnedAsync(owner, ct));
    });
}
