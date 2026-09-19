using DigiCard.Contracts.Invitations;
using DigiCard.Modules.Invitations.Domain;
using DigiCard.Modules.Invitations.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DigiCard.Modules.Invitations.Features.GetDraft;

internal sealed class DraftReader(InvitationsDbContext db) : IDraftReader
{
    public async Task<DraftResponse?> FindOwnedAsync(Guid id, string ownerId, CancellationToken ct)
    {
        // Both halves of the predicate are on the entity, before the projection. Filtering after
        // Select would ask SQL Server to compare a constructed C# object, which it cannot do.
        var row = await Project(Owned(ownerId).Where(x => x.Id == id)).SingleOrDefaultAsync(ct);
        return row is null ? null : ToResponse(row);
    }

    public async Task<IReadOnlyList<DraftResponse>> ListOwnedAsync(string ownerId, CancellationToken ct)
    {
        // Ordering happens on the entity for the same reason the id filter does: Row is a record
        // with a constructor, and EF cannot see through one to work out which column UpdatedAt was.
        var rows = await Project(Owned(ownerId).OrderByDescending(x => x.UpdatedAt).Take(200))
            .ToListAsync(ct);

        // Sorted again in memory - on at most 200 rows, free. A projection wrapped around a TOP
        // query becomes a sub-select, and SQL Server does not promise the outer query keeps the
        // inner ORDER BY. This makes newest-first a fact rather than a hope.
        return rows.OrderByDescending(r => r.UpdatedAt).Select(ToResponse).ToList();
    }

    /// <summary>
    /// Ownership is part of the query, never a check afterwards: an id on its own is never enough
    /// to read someone else's card. Every read starts here.
    /// </summary>
    private IQueryable<InvitationDraft> Owned(string ownerId) =>
        db.Drafts.AsNoTracking().Where(x => x.OwnerId == ownerId);

    /// <summary>
    /// Everything the response needs, read as plain columns. The row version is turned into text
    /// afterwards rather than inside the query: Convert.ToBase64String has no SQL translation, and
    /// putting it in the projection would quietly pull the whole table into memory instead.
    /// </summary>
    private static IQueryable<Row> Project(IQueryable<InvitationDraft> query) =>
        query.Select(x => new Row(
            x.Id, x.Title, x.BrideName, x.GroomName, x.Message, x.TemplateId, x.TemplateVersion,
            x.Accent, x.CreatedAt, x.Elements, x.Address, x.EventDate, x.EventTime, x.EventEndTime,
            x.UpdatedAt, x.RowVersion));

    private static DraftResponse ToResponse(Row r) => new(
        r.Id, r.Title, r.BrideName, r.GroomName, r.Message, r.TemplateId, r.TemplateVersion,
        r.Accent, r.CreatedAt, r.Elements, r.Address, r.EventDate, r.EventTime, r.EventEndTime,
        r.UpdatedAt, Convert.ToBase64String(r.RowVersion));

    private sealed record Row(
        Guid Id, string Title, string BrideName, string GroomName, string Message,
        Guid TemplateId, int TemplateVersion, string Accent, DateTimeOffset CreatedAt,
        string Elements, string Address, DateOnly? EventDate, TimeOnly? EventTime,
        TimeOnly? EventEndTime, DateTimeOffset UpdatedAt, byte[] RowVersion);
}
