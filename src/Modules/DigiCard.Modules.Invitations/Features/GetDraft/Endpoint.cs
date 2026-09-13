using System.Security.Claims;
using DigiCard.Contracts.Invitations;
using DigiCard.Modules.Invitations.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
namespace DigiCard.Modules.Invitations.Features.GetDraft;
internal static class Endpoint
{
    public static void Map(RouteGroupBuilder group) => group.MapGet("/{id:guid}", async (Guid id,
        ClaimsPrincipal user, HttpContext http, IDraftReader reader, CancellationToken ct) =>
    {
        http.Response.Headers.CacheControl = "no-store";
        var owner = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var draft = string.IsNullOrWhiteSpace(owner) ? null : await reader.FindOwnedAsync(id, owner, ct);
        return draft is null ? Results.NotFound() : Results.Ok(draft);
    });
}
