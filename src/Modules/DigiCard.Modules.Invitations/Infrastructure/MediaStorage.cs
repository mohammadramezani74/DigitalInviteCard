using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DigiCard.Modules.Invitations.Infrastructure;

/// <summary>
/// Where the processed images live.
///
/// Outside wwwroot, on purpose. Anything under wwwroot is served by the static file middleware to
/// anyone who guesses the path, with no check on who is asking - which for a folder of other
/// people's wedding photos is not a detail. These are read back through an endpoint that checks
/// ownership first.
///
/// Every path is built from a Guid and an integer the caller never supplies as text, so there is
/// no string from a request anywhere in a file path and nothing to traverse with.
/// </summary>
public sealed class MediaStorage
{
    private readonly string root;

    public MediaStorage(IConfiguration configuration, IHostEnvironment environment)
    {
        var configured = configuration["Media:Root"];

        root = string.IsNullOrWhiteSpace(configured)
            ? Path.Combine(environment.ContentRootPath, "media-store")
            : configured;

        Directory.CreateDirectory(root);
    }

    public async Task WriteAsync(Guid id, int width, Stream content, CancellationToken ct)
    {
        var folder = FolderFor(id);
        Directory.CreateDirectory(folder);

        var path = Path.Combine(folder, $"{width}.webp");

        // Written to a temporary name and moved into place, so a half-written file is never
        // readable under the real name if the process dies mid-upload.
        var temporary = path + ".part";

        await using (var file = File.Create(temporary))
        {
            await content.CopyToAsync(file, ct);
        }

        File.Move(temporary, path, overwrite: true);
    }

    public FileStream? OpenRead(Guid id, int width)
    {
        var path = Path.Combine(FolderFor(id), $"{width}.webp");
        return File.Exists(path) ? File.OpenRead(path) : null;
    }

    public void Delete(Guid id)
    {
        var folder = FolderFor(id);
        if (Directory.Exists(folder)) Directory.Delete(folder, recursive: true);
    }

    /// <summary>
    /// Two levels of fan-out from the id. A single folder with a hundred thousand children is
    /// slow to list on most filesystems, and one day someone will list it.
    /// </summary>
    private string FolderFor(Guid id)
    {
        var text = id.ToString("N");
        return Path.Combine(root, text[..2], text[2..4], text);
    }
}
