namespace DigiCard.Contracts.Media;

/// <summary>What an uploaded file is for. The kind decides which sizes are produced.</summary>
public static class MediaKind
{
    /// <summary>A photo placed on the card itself - never larger than the card.</summary>
    public const string Photo = "photo";

    /// <summary>The page behind the card when a guest opens the invitation.</summary>
    public const string Background = "background";

    private static readonly HashSet<string> All = [Photo, Background];

    public static bool IsKnown(string? kind) => kind is not null && All.Contains(kind);

    /// <summary>
    /// The widths stored for each kind. A photo on a card is never shown wider than the card, so
    /// producing a 1920px version of it would be bytes nobody ever downloads; a background fills
    /// the window and needs the large one.
    /// </summary>
    public static int[] WidthsFor(string kind) => kind switch
    {
        Background => [640, 1280, 1920],
        _ => [320, 640, 1024],
    };

    /// <summary>The width a browser gets when it ignores srcset.</summary>
    public static int DefaultWidthFor(string kind) => kind == Background ? 1280 : 640;
}

/// <summary>
/// An uploaded image after processing. The stored dimensions are the original's, which is what
/// lets the renderer set width and height on the img tag - the single cheapest thing that stops
/// a page jumping around while images load, and one Google measures directly.
/// </summary>
public sealed record MediaSummary(Guid Id, string Kind, int Width, int Height)
{
    /// <summary>
    /// What a card stores. Deliberately without a width: the element records which image it uses,
    /// and the renderer decides which size to ask for.
    /// </summary>
    public string Source => $"/api/media/{Id}";

    public static string Url(string source, int width) => $"{source}/{width}.webp";

    /// <summary>Every stored size, for the browser to choose from.</summary>
    public static string SrcSet(string source, string kind) =>
        string.Join(", ", MediaKind.WidthsFor(kind).Select(w => $"{Url(source, w)} {w}w"));
}
