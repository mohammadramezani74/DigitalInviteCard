namespace DigiCard.Contracts.Templates;

/// <summary>
/// An occasion a card can be made for. This used to be a fixed six-item list in code, which
/// only worked while the product was wedding-only. It is data now, so an occasion can be added
/// without a deployment.
///
/// The label fields are the important part: a card for a funeral must not ask for "نام عروس".
/// Every user-facing word that depends on the occasion comes from here rather than from markup.
/// </summary>
/// <param name="PrimaryLabel">What the first name field is called - bride, deceased, celebrant.</param>
/// <param name="SecondaryLabel">The second name field, or null when the occasion has only one.</param>
/// <param name="Joiner">The word between the two names ("و"), or null when there is only one.</param>
/// <param name="DefaultKicker">The small line above the names on a new card.</param>
/// <param name="DefaultMessage">The body text a new card starts with.</param>
public sealed record OccasionSummary(
    Guid Id,
    string Slug,
    string Title,
    string Tagline,
    string Icon,
    int SortOrder,
    bool IsActive,
    string PrimaryLabel,
    string? SecondaryLabel,
    string? Joiner,
    string DefaultKicker,
    string DefaultMessage)
{
    public bool HasSecondName => !string.IsNullOrWhiteSpace(SecondaryLabel);
}

public interface IOccasionCatalog
{
    /// <summary>Active occasions, in display order. Inactive ones are hidden from the site.</summary>
    Task<IReadOnlyList<OccasionSummary>> ListAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Any occasion by slug, active or not - the admin panel will need to open a hidden one to
    /// build its first template. Public pages must check <see cref="OccasionSummary.IsActive"/>
    /// themselves and answer "not found" when it is false, so a hidden occasion never becomes an
    /// indexable empty page.
    /// </summary>
    Task<OccasionSummary?> FindAsync(string slug, CancellationToken cancellationToken);
}
