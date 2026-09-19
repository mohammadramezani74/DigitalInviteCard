namespace DigiCard.Contracts.Invitations;
public interface IDraftReader
{
    Task<DraftResponse?> FindOwnedAsync(Guid id, string ownerId, CancellationToken ct);

    /// <summary>Everything this person has saved, most recently changed first.</summary>
    Task<IReadOnlyList<DraftResponse>> ListOwnedAsync(string ownerId, CancellationToken ct);
}
