namespace DigiCard.Contracts.Templates;

/// <summary>
/// A verse offered in the editor's poem picker.
///
/// Only classical, public-domain poetry is stocked here. Modern Persian poets - Shamlou, Sepehri,
/// Moshiri and their contemporaries - are still in copyright, and a curated list inside a paid
/// product is a very different thing from a user typing their own favourite line. Users can always
/// type whatever they like; this list is the one the product ships.
/// </summary>
/// <param name="Text">The verse. Line breaks separate hemistichs and are preserved when rendered.</param>
/// <param name="Poet">Attribution, shown under the verse.</param>
/// <param name="OccasionSlug">The occasion it suits, or null when it suits any.</param>
public sealed record PoemSummary(
    Guid Id,
    string Text,
    string Poet,
    string? OccasionSlug,
    int SortOrder);

public interface IPoemCatalog
{
    /// <summary>
    /// Active verses, in display order. With an occasion slug, the ones marked for that occasion
    /// come first, followed by the general ones - a verse about separation belongs on a memorial
    /// card and nowhere near a wedding.
    /// </summary>
    Task<IReadOnlyList<PoemSummary>> ListAsync(string? occasionSlug, CancellationToken cancellationToken);
}
