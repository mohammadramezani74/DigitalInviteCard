using DigiCard.Contracts.Invitations;
using DigiCard.Modules.Invitations.Infrastructure;
using Microsoft.EntityFrameworkCore;
namespace DigiCard.Modules.Invitations.Features.GetDraft;
internal sealed class DraftReader(InvitationsDbContext db) : IDraftReader
{
    public Task<DraftResponse?> FindOwnedAsync(Guid id, string ownerId, CancellationToken ct) =>
        db.Drafts.AsNoTracking().Where(x => x.Id == id && x.OwnerId == ownerId)
            .Select(x => new DraftResponse(x.Id, x.Title, x.BrideName, x.GroomName, x.Message,
                x.TemplateId, x.TemplateVersion, x.Accent, x.CreatedAt)).SingleOrDefaultAsync(ct);
}
