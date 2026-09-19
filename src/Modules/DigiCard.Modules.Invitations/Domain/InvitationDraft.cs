using DigiCard.Contracts.Templates;
namespace DigiCard.Modules.Invitations.Domain;

/// <summary>When the ceremony is. Grouped so Create does not grow a tail of loose nullables.</summary>
public sealed record EventSchedule(DateOnly? Date, TimeOnly? Start, TimeOnly? End);
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

    /// <summary>
    /// This card's arrangement, as the JSON documented on CardLayout. Copied from the template
    /// at creation rather than read through it: once someone has saved a card, editing the
    /// template must not quietly move their names around.
    /// </summary>
    public string Elements { get; private set; } = "";

    /// <summary>
    /// When the ceremony is, as a real date rather than a formatted string. A card has to be
    /// able to show the same day as "۲۴ مرداد ۱۴۰۵" on one template and "۱۴۰۵/۰۵/۲۴" inside a
    /// ring on another, and a stored string can only ever be one of those.
    ///
    /// DateOnly and TimeOnly rather than a DateTimeOffset: a wedding at seven in the evening is
    /// seven in the evening wherever the guest reading the card happens to be, so an instant
    /// with an offset would be the wrong type and would shift the time for anyone abroad.
    /// </summary>
    public DateOnly? EventDate { get; private set; }

    public TimeOnly? EventTime { get; private set; }

    /// <summary>
    /// When the ceremony ends. Optional, because plenty of invitations only name a start -
    /// but "۱۸:۰۰ تا ۲۲:۰۰" is how a great many Persian cards actually read, so the card has
    /// to be able to say it.
    /// </summary>
    public TimeOnly? EventEndTime { get; private set; }

    public string Address { get; private set; } = "";

    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// When the card was last saved. This is what "کارت‌های من" sorts by - creation order is
    /// almost never the order someone wants to see their own work in.
    /// </summary>
    public DateTimeOffset UpdatedAt { get; private set; }
    public byte[] RowVersion { get; private set; } = [];
    public static InvitationDraft Create(string ownerId, string title, string bride, string groom,
        string message, string address, Guid templateId, int version, string accent, string elements,
        EventSchedule schedule, DateTimeOffset now)
    {
        if (string.IsNullOrWhiteSpace(ownerId)) throw new ArgumentException("Owner is required.", nameof(ownerId));
        if (templateId == Guid.Empty || version < 1) throw new ArgumentException("A versioned template is required.");
        return new InvitationDraft { Id = Guid.NewGuid(), OwnerId = ownerId,
            Title = Required(title, 100), BrideName = Required(bride, 80), GroomName = Required(groom, 80),
            Message = Optional(message, 1000), TemplateId = templateId, TemplateVersion = version,
            Accent = accent,
            Address = Optional(address, 300),
            // Never stored as it arrived. Parsing and re-serializing is what guarantees the row
            // is inside the model's limits however the browser behaved.
            Elements = Normalized(elements),
            EventDate = schedule.Date,
            EventTime = schedule.Start,
            // An end before the start is a typo, not an instruction. Dropped rather than drawn.
            EventEndTime = schedule.End > schedule.Start ? schedule.End : null,
            CreatedAt = now, UpdatedAt = now };
    }
    /// <summary>
    /// Saves over this card. The same validation as Create, deliberately: an update is not a
    /// lesser kind of write, and a card edited badly is exactly as broken as one created badly.
    /// The owner is never touched - a draft does not change hands.
    /// </summary>
    public void Update(string title, string bride, string groom, string message, string address,
        Guid templateId, int version, string accent, string elements, EventSchedule schedule,
        DateTimeOffset now)
    {
        if (templateId == Guid.Empty || version < 1) throw new ArgumentException("A versioned template is required.");

        Title = Required(title, 100);
        BrideName = Required(bride, 80);
        GroomName = Required(groom, 80);
        Message = Optional(message, 1000);
        Address = Optional(address, 300);

        TemplateId = templateId;
        TemplateVersion = version;
        Accent = accent;
        Elements = Normalized(elements);

        EventDate = schedule.Date;
        EventTime = schedule.Start;
        EventEndTime = schedule.End > schedule.Start ? schedule.End : null;

        UpdatedAt = now;
    }

    private static string Normalized(string? elements) =>
        CardLayout.TryParse(elements, out var layout) && !layout.IsEmpty ? layout.Serialize() : "";

    private static string Required(string value, int max) => string.IsNullOrWhiteSpace(value)
        ? throw new ArgumentException("Text is required.") : Optional(value, max);
    private static string Optional(string? value, int max)
    {
        var text = (value ?? "").Trim();
        return text.Length <= max ? text : throw new ArgumentException("Text exceeds its maximum length.");
    }
}
