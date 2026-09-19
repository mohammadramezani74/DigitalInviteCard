using System.ComponentModel.DataAnnotations;
namespace DigiCard.Contracts.Invitations;
/// <summary>
/// Saving over a card that already exists. Carries the row version the browser last saw, so two
/// tabs editing the same card cannot silently overwrite each other - the second one is told.
/// </summary>
public sealed class UpdateDraftRequest : CreateDraftRequest
{
    /// <summary>Base64 of the row version from the last read. Empty means "I did not check".</summary>
    public string RowVersion { get; set; } = "";
}

public class CreateDraftRequest
{
    public Guid TemplateId { get; set; }
    [Required, StringLength(100)] public string Title { get; set; } = "دعوت عروسی ما";
    [Required, StringLength(80)] public string BrideName { get; set; } = "";
    [Required, StringLength(80)] public string GroomName { get; set; } = "";
    [StringLength(1000)] public string Message { get; set; } = "با حضور شما شادی ما کامل می‌شود";

    [StringLength(300)] public string Address { get; set; } = "";

    /// <summary>
    /// The arrangement the editor produced, as the JSON documented on CardLayout. The server does
    /// not store this string as it arrives: it parses it, normalizes it and re-serializes it, so
    /// whatever the browser sends, what lands in the database is inside the model's limits.
    /// Empty means "use the template's own layout".
    /// </summary>
    [StringLength(60000)] public string Elements { get; set; } = "";

    public DateOnly? EventDate { get; set; }
    public TimeOnly? EventTime { get; set; }
    public TimeOnly? EventEndTime { get; set; }
}
public sealed record DraftResponse(Guid Id, string Title, string BrideName, string GroomName,
    string Message, Guid TemplateId, int TemplateVersion, string Accent, DateTimeOffset CreatedAt,
    // The draft's own arrangement, copied from the template when the draft was created. See
    // CardLayout for the format. Empty means this draft predates the element model.
    string Elements,
    string Address,
    // Null until the card is given a date in the editor.
    DateOnly? EventDate,
    TimeOnly? EventTime,
    TimeOnly? EventEndTime,
    DateTimeOffset UpdatedAt,
    /// <summary>Base64 row version, handed back on save so the server can detect a stale write.</summary>
    string RowVersion);
