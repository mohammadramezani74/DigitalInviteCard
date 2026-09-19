namespace DigiCard.Contracts.Templates;

/// <summary>
/// A template as the browser and other modules see it. Design tokens are plain scalars so the
/// contract stays free of EF types and of any rendering engine. This is deliberately NOT a
/// published snapshot: publishing requires immutable per-revision assets (see docs/ARCHITECTURE.md).
/// </summary>
public sealed record TemplateSummary(
    Guid Id,
    string Slug,
    string Name,
    int Version,
    string Category,
    string Family,
    string Accent,
    string Background,
    string Ink,
    string Frame,
    string Ornament,
    string Typeface,
    string Layout,
    string Artwork,
    int SafeTop,
    int SafeBottom,
    /// <summary>
    /// The template's saved arrangement, as the JSON documented on <see cref="CardLayout"/>.
    /// Empty means the template predates the element model and falls back to the fixed markup.
    /// Kept as text rather than a parsed layout so the contract has no rendering opinion and the
    /// projection stays a plain column read.
    /// </summary>
    string Elements);

public interface ITemplateCatalog
{
    Task<IReadOnlyList<TemplateSummary>> ListAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<TemplateSummary>> ListByCategoryAsync(string category, CancellationToken cancellationToken);
    Task<TemplateSummary?> FindAsync(Guid id, CancellationToken cancellationToken);
    Task<TemplateSummary?> FindBySlugAsync(string slug, CancellationToken cancellationToken);
}
