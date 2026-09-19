using DigiCard.Contracts.Templates;
using Microsoft.EntityFrameworkCore;

namespace DigiCard.Modules.Templates.Features.ListOccasions;

internal sealed class OccasionCatalog(TemplatesDbContext db) : IOccasionCatalog
{
    public async Task<IReadOnlyList<OccasionSummary>> ListAsync(CancellationToken cancellationToken) =>
        await Project(db.Occasions.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.SortOrder))
            .ToListAsync(cancellationToken);

    /// <summary>
    /// Looks up by slug regardless of IsActive, because the admin panel has to open a hidden
    /// occasion to build its first template. The caller decides what a hidden one means: public
    /// pages answer "not found" so it never becomes an indexable empty page.
    /// </summary>
    public Task<OccasionSummary?> FindAsync(string slug, CancellationToken cancellationToken) =>
        Project(db.Occasions.AsNoTracking().Where(x => x.Slug == slug))
            .SingleOrDefaultAsync(cancellationToken);

    private static IQueryable<OccasionSummary> Project(IQueryable<Occasion> source) =>
        source.Select(x => new OccasionSummary(
            x.Id, x.Slug, x.Title, x.Tagline, x.Icon, x.SortOrder, x.IsActive,
            x.PrimaryLabel,
            // Empty string is how the database says "this occasion has one name"; the contract
            // uses null so callers can test it without knowing that.
            x.SecondaryLabel == "" ? null : x.SecondaryLabel,
            x.Joiner == "" ? null : x.Joiner,
            x.DefaultKicker, x.DefaultMessage));
}
