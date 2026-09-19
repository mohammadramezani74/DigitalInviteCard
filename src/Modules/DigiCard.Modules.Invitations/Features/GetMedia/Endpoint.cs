using System.Security.Claims;
using DigiCard.Contracts.Media;
using DigiCard.Modules.Invitations.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace DigiCard.Modules.Invitations.Features.GetMedia;

internal static class Endpoint
{
    public static void Map(RouteGroupBuilder group) => group.MapGet("/{id:guid}/{width:int}.webp", Handle);

    private static async Task<IResult> Handle(Guid id, int width, ClaimsPrincipal user,
        HttpContext http, MediaStorage storage, InvitationsDbContext db, CancellationToken ct)
    {
        var owner = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(owner)) return Results.Unauthorized();

        // Ownership is in the query. Once cards can be published this grows a second branch for
        // "belongs to a published card", and nothing else about this endpoint changes.
        var asset = await db.Media.AsNoTracking()
            .Where(x => x.Id == id && x.OwnerId == owner)
            .Select(x => new { x.Kind })
            .SingleOrDefaultAsync(ct);

        if (asset is null) return Results.NotFound();

        // Only the widths this kind actually has. Anything else is a typo or someone probing.
        if (!MediaKind.WidthsFor(asset.Kind).Contains(width)) return Results.NotFound();

        var file = storage.OpenRead(id, width);
        if (file is null) return Results.NotFound();

        // The bytes behind this URL can never change - a new upload gets a new id - so it is safe
        // to let the browser keep it for a year and never ask again. "private" because it belongs
        // to one person and must not sit in a shared cache on the way.
        http.Response.Headers.CacheControl = "private, max-age=31536000, immutable";
        http.Response.Headers[HeaderNames.XContentTypeOptions] = "nosniff";

        return Results.File(file, "image/webp");
    }
}
