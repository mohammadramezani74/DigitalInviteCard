using DigiCard.Contracts.Templates;
using Microsoft.EntityFrameworkCore;

namespace DigiCard.Modules.Templates.Features.ListTemplates;

internal sealed class TemplateCatalog(TemplatesDbContext db) : ITemplateCatalog
{
    public async Task<IReadOnlyList<TemplateSummary>> ListAsync(CancellationToken cancellationToken) =>
        await Project(db.Templates.AsNoTracking().OrderBy(x => x.Family).ThenBy(x => x.Name))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TemplateSummary>> ListByCategoryAsync(string category, CancellationToken cancellationToken) =>
        await Project(db.Templates.AsNoTracking().Where(x => x.Category == category)
                .OrderBy(x => x.Family).ThenBy(x => x.Name))
            .ToListAsync(cancellationToken);

    public Task<TemplateSummary?> FindAsync(Guid id, CancellationToken cancellationToken) =>
        Project(db.Templates.AsNoTracking().Where(x => x.Id == id)).SingleOrDefaultAsync(cancellationToken);

    public Task<TemplateSummary?> FindBySlugAsync(string slug, CancellationToken cancellationToken) =>
        Project(db.Templates.AsNoTracking().Where(x => x.Slug == slug)).SingleOrDefaultAsync(cancellationToken);

    private static IQueryable<TemplateSummary> Project(IQueryable<CardTemplate> source) =>
        source.Select(x => new TemplateSummary(
            x.Id, x.Slug, x.Name, x.Version, x.Category, x.Family,
            x.Accent, x.Background, x.Ink, x.Frame, x.Ornament, x.Typeface, x.Layout,
            x.Artwork, x.SafeTop, x.SafeBottom, x.Elements));
}
