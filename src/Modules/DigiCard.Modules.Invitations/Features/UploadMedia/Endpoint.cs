using System.Security.Claims;
using DigiCard.Contracts.Media;
using DigiCard.Modules.Invitations.Domain;
using DigiCard.Modules.Invitations.Infrastructure;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SkiaSharp;

namespace DigiCard.Modules.Invitations.Features.UploadMedia;

internal static class Endpoint
{
    /// <summary>What arrives from a phone camera, with room to spare. Bigger is refused outright.</summary>
    private const long MaxUploadBytes = 12 * 1024 * 1024;

    /// <summary>
    /// A small file can describe an enormous image. Decoding one is how a single upload eats all
    /// the memory on the server, so the dimensions are read from the header and judged before any
    /// pixels are decoded.
    /// </summary>
    private const long MaxPixels = 40_000_000;

    private const int Quality = 78;

    /// <summary>
    /// How pixels are sampled when the picture is redrawn smaller. Linear with mipmaps is the
    /// right choice for downscaling - nearest-neighbour on a photo shrunk to a third of its size
    /// produces visible stair-stepping on every diagonal, which on a wedding card is exactly the
    /// kind of thing people notice without being able to name.
    /// </summary>
    private static readonly SKSamplingOptions Sampling = new(SKFilterMode.Linear, SKMipmapMode.Linear);

    public static void Map(RouteGroupBuilder group) =>
        group.MapPost("/", Handle).DisableAntiforgery();

    private static async Task<IResult> Handle(HttpRequest http, ClaimsPrincipal user,
        IAntiforgery antiforgery, MediaStorage storage, InvitationsDbContext db,
        TimeProvider clock, CancellationToken ct)
    {
        // The framework filter is turned off above because this is multipart; the token is still
        // required, just validated here, where the form has been read.
        try { await antiforgery.ValidateRequestAsync(http.HttpContext); }
        catch (AntiforgeryValidationException) { return Results.BadRequest(new { error = "Invalid antiforgery token." }); }

        var owner = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(owner)) return Results.Unauthorized();

        if (!http.HasFormContentType) return Results.BadRequest(new { error = "Expected a file upload." });

        var form = await http.ReadFormAsync(ct);
        var file = form.Files.GetFile("file");
        var kind = form["kind"].ToString();

        if (file is null || file.Length == 0) return Results.BadRequest(new { error = "فایلی فرستاده نشد." });
        if (!MediaKind.IsKnown(kind)) return Results.BadRequest(new { error = "نوع فایل مشخص نیست." });
        if (file.Length > MaxUploadBytes) return Results.BadRequest(new { error = "حجم فایل بیش از ۱۲ مگابایت است." });

        // Read once into memory: the header is inspected, then the same bytes are decoded, and a
        // network stream cannot be rewound.
        using var buffer = new MemoryStream();
        await using (var source = file.OpenReadStream())
        {
            await source.CopyToAsync(buffer, ct);
        }
        buffer.Position = 0;

        // Whatever the extension or the content type claimed, only a decoder's opinion counts.
        using var codec = SKCodec.Create(buffer);
        if (codec is null) return Results.BadRequest(new { error = "این فایل تصویر نیست." });

        if ((long)codec.Info.Width * codec.Info.Height > MaxPixels)
            return Results.BadRequest(new { error = "ابعاد تصویر بیش از حد بزرگ است." });

        using var decoded = SKBitmap.Decode(codec);
        if (decoded is null) return Results.BadRequest(new { error = "این فایل تصویر نیست." });

        // Phone cameras almost always write the photo sideways and record the rotation in EXIF.
        // Re-encoding drops that tag, so the rotation has to be baked into the pixels here or
        // every portrait photo ends up lying on its side.
        using var rotated = Orient(decoded, codec.EncodedOrigin);
        var image = rotated ?? decoded;

        var id = Guid.NewGuid();
        long written = 0;

        try
        {
            using var full = SKImage.FromBitmap(image);

            foreach (var width in MediaKind.WidthsFor(kind))
            {
                // Only ever downscale. Enlarging a small photo costs bytes and adds nothing.
                var targetWidth = Math.Min(width, image.Width);
                var targetHeight = Math.Max(1, (int)Math.Round(image.Height * (targetWidth / (double)image.Width)));

                using var surface = SKSurface.Create(new SKImageInfo(targetWidth, targetHeight));
                surface.Canvas.Clear(SKColors.Transparent);
                surface.Canvas.DrawImage(full, new SKRect(0, 0, targetWidth, targetHeight), Sampling);

                using var snapshot = surface.Snapshot();
                using var encoded = snapshot.Encode(SKEncodedImageFormat.Webp, Quality);
                if (encoded is null) return Results.BadRequest(new { error = "تبدیل تصویر انجام نشد." });

                await using var stream = encoded.AsStream();
                written += encoded.Size;
                await storage.WriteAsync(id, width, stream, ct);
            }

            db.Media.Add(MediaAsset.Create(id, owner, kind, image.Width, image.Height, written, clock.GetUtcNow()));
            await db.SaveChangesAsync(ct);
        }
        catch
        {
            // A row with no files behind it is a broken image on someone's card; files with no row
            // are only wasted disk. If the write fails, leave nothing behind.
            storage.Delete(id);
            throw;
        }

        return Results.Ok(new MediaSummary(id, kind, image.Width, image.Height));
    }

    /// <summary>
    /// Re-draws the picture the right way up, or returns null when it already is. Only the four
    /// plain rotations are handled: the mirrored origins come from scanners and are vanishingly
    /// rare on a photo someone puts on a wedding card.
    /// </summary>
    private static SKBitmap? Orient(SKBitmap bitmap, SKEncodedOrigin origin)
    {
        var quarterTurns = origin switch
        {
            SKEncodedOrigin.RightTop => 1,
            SKEncodedOrigin.BottomRight => 2,
            SKEncodedOrigin.LeftBottom => 3,
            _ => 0,
        };

        if (quarterTurns == 0) return null;

        var sideways = quarterTurns % 2 == 1;
        var target = new SKBitmap(
            sideways ? bitmap.Height : bitmap.Width,
            sideways ? bitmap.Width : bitmap.Height);

        using var canvas = new SKCanvas(target);
        using var image = SKImage.FromBitmap(bitmap);

        canvas.Translate(target.Width / 2f, target.Height / 2f);
        canvas.RotateDegrees(quarterTurns * 90);
        canvas.Translate(-bitmap.Width / 2f, -bitmap.Height / 2f);
        canvas.DrawImage(image, 0, 0, Sampling);

        return target;
    }
}
