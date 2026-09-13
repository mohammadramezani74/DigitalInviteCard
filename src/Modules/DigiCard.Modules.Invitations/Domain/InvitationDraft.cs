namespace DigiCard.Modules.Invitations.Domain;
public sealed class InvitationDraft
{
    private InvitationDraft() { }
    public Guid Id { get; private set; }
    public string OwnerId { get; private set; } = "";
    public string Title { get; private set; } = "";
    public string BrideName { get; private set; } = "";
    public string GroomName { get; private set; } = "";
    public string Message { get; private set; } = "";
    public Guid TemplateId { get; private set; }
    public int TemplateVersion { get; private set; }
    public string Accent { get; private set; } = "";
    public DateTimeOffset CreatedAt { get; private set; }
    public byte[] RowVersion { get; private set; } = [];
    public static InvitationDraft Create(string ownerId, string title, string bride, string groom,
        string message, Guid templateId, int version, string accent, DateTimeOffset now)
    {
        if (string.IsNullOrWhiteSpace(ownerId)) throw new ArgumentException("Owner is required.", nameof(ownerId));
        if (templateId == Guid.Empty || version < 1) throw new ArgumentException("A versioned template is required.");
        return new InvitationDraft { Id = Guid.NewGuid(), OwnerId = ownerId,
            Title = Required(title, 100), BrideName = Required(bride, 80), GroomName = Required(groom, 80),
            Message = Optional(message, 1000), TemplateId = templateId, TemplateVersion = version,
            Accent = accent, CreatedAt = now };
    }
    private static string Required(string value, int max) => string.IsNullOrWhiteSpace(value)
        ? throw new ArgumentException("Text is required.") : Optional(value, max);
    private static string Optional(string? value, int max)
    {
        var text = (value ?? "").Trim();
        return text.Length <= max ? text : throw new ArgumentException("Text exceeds its maximum length.");
    }
}
