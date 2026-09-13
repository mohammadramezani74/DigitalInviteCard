namespace DigiCard.Contracts.Templates;
public sealed record TemplateSummary(Guid Id, string Name, int Version, string Accent);
public interface ITemplateCatalog
{
    Task<IReadOnlyList<TemplateSummary>> ListAsync(CancellationToken cancellationToken);
    Task<TemplateSummary?> FindAsync(Guid id, CancellationToken cancellationToken);
}
