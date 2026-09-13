using DigiCard.Contracts.Templates;
using Microsoft.EntityFrameworkCore;
namespace DigiCard.Modules.Templates.Features.ListTemplates;
internal sealed class TemplateCatalog(TemplatesDbContext db) : ITemplateCatalog
{
    public async Task<IReadOnlyList<TemplateSummary>> ListAsync(CancellationToken cancellationToken) =>
        await db.Templates.AsNoTracking().OrderBy(x => x.Name)
            .Select(x => new TemplateSummary(x.Id, x.Name, x.Version, x.Accent)).ToListAsync(cancellationToken);
    public Task<TemplateSummary?> FindAsync(Guid id, CancellationToken cancellationToken) =>
        db.Templates.AsNoTracking().Where(x => x.Id == id)
            .Select(x => new TemplateSummary(x.Id, x.Name, x.Version, x.Accent)).SingleOrDefaultAsync(cancellationToken);
}
