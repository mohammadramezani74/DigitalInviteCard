namespace DigiCard.Modules.Invitations.Domain;

/// <summary>
/// An image someone uploaded. The bytes live on disk; this row is what says who owns them and
/// how big the picture is.
///
/// Nothing here records the original file name, format or metadata, and that is deliberate.
/// Everything is re-encoded on the way in, so the original is not kept and cannot be served -
/// which is what removes the whole class of "the file claimed to be a JPEG" problems, and takes
/// the EXIF with it. A wedding photo straight off a phone carries the venue's GPS coordinates.
/// </summary>
public sealed class MediaAsset
{
    private MediaAsset() { }

    public Guid Id { get; private set; }

    public string OwnerId { get; private set; } = "";

    /// <summary>photo | background - see MediaKind.</summary>
    public string Kind { get; private set; } = "";

    /// <summary>The original's dimensions, kept so the renderer can reserve the right space.</summary>
    public int Width { get; private set; }
    public int Height { get; private set; }

    /// <summary>Total bytes of every stored size, for a per-account quota later.</summary>
    public long Bytes { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public static MediaAsset Create(Guid id, string ownerId, string kind, int width, int height,
        long bytes, DateTimeOffset now)
    {
        if (string.IsNullOrWhiteSpace(ownerId)) throw new ArgumentException("Owner is required.", nameof(ownerId));
        if (width < 1 || height < 1) throw new ArgumentException("A decoded image is required.");

        return new MediaAsset
        {
            Id = id,
            OwnerId = ownerId,
            Kind = kind,
            Width = width,
            Height = height,
            Bytes = bytes,
            CreatedAt = now,
        };
    }
}
