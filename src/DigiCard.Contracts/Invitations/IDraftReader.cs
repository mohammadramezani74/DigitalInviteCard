namespace DigiCard.Contracts.Invitations;
public interface IDraftReader
{
    Task<DraftResponse?> FindOwnedAsync(Guid id, string ownerId, CancellationToken ct);
}
