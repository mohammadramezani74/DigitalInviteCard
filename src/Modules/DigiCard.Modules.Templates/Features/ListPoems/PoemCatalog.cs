using DigiCard.Contracts.Templates;
using Microsoft.EntityFrameworkCore;

namespace DigiCard.Modules.Templates.Features.ListPoems;

internal sealed class PoemCatalog(TemplatesDbContext db) : IPoemCatalog
{
    public async Task<IReadOnlyList<PoemSummary>> ListAsync(string? occasionSlug, CancellationToken cancellationToken)
    {
        var slug = occasionSlug ?? "";

        var query = db.Poems.AsNoTracking().Where(x => x.IsActive);

        // Verses marked for this occasion first, then the general ones. Ordering in the database
        // rather than in memory keeps this a single indexed read.
        if (slug.Length > 0)
            query = query.Where(x => x.OccasionSlug == slug || x.OccasionSlug == "");

        return await query
            .OrderByDescending(x => x.OccasionSlug == slug && slug.Length > 0)
            .ThenBy(x => x.SortOrder)
            .Select(x => new PoemSummary(
                x.Id,
                x.Text,
                x.Poet,
                x.OccasionSlug == "" ? null : x.OccasionSlug,
                x.SortOrder))
            .ToListAsync(cancellationToken);
    }
}
