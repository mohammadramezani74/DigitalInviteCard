using DigiCard.Contracts.Templates;
using DigiCard.Modules.Invitations.Domain;
namespace DigiCard.Tests;
public sealed class DraftTests
{
    // The layout a draft copies from its template at creation.
    private static readonly string Layout = CardLayoutDefaults.Json(30, 70);

    private static readonly EventSchedule None = new(null, null, null);

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Owner_is_required(string owner) => Assert.Throws<ArgumentException>(() =>
        InvitationDraft.Create(owner, "Title", "Bride", "Groom", "", "", Guid.NewGuid(), 1, "#52796f", Layout, None, DateTimeOffset.UtcNow));
    [Fact]
    public void Long_names_cannot_break_template_limits() => Assert.Throws<ArgumentException>(() =>
        InvitationDraft.Create("owner", "Title", new string('x',81), "Groom", "", "", Guid.NewGuid(), 1, "#52796f", Layout, None, DateTimeOffset.UtcNow));
    [Fact]
    public void Draft_keeps_the_selected_template_version_and_trims_text()
    {
        var id = Guid.NewGuid();
        var draft = InvitationDraft.Create("owner", "  Title  ", "سارا", "علی", "سلام", "", id, 4, "#52796f", Layout, None, DateTimeOffset.UtcNow);
        Assert.Equal(4, draft.TemplateVersion); Assert.Equal(id, draft.TemplateId); Assert.Equal("Title", draft.Title);
    }

    [Fact]
    public void Draft_snapshots_the_layout_rather_than_pointing_at_the_template()
    {
        // Editing a template later must not rearrange a card someone already saved, so the draft
        // keeps its own copy of the arrangement.
        var draft = InvitationDraft.Create("owner", "Title", "Bride", "Groom", "", "", Guid.NewGuid(), 1, "#52796f",
            Layout, None, DateTimeOffset.UtcNow);

        Assert.Equal(Layout, draft.Elements);
        Assert.True(CardLayout.TryParse(draft.Elements, out var parsed));
        Assert.NotEmpty(parsed.Elements);
    }

    [Fact]
    public void A_template_with_no_saved_layout_still_produces_a_draft()
    {
        // Templates are converted one at a time; an unconverted one falls back to the fixed
        // markup rather than blocking card creation.
        var draft = InvitationDraft.Create("owner", "Title", "Bride", "Groom", "", "", Guid.NewGuid(), 1, "#52796f",
            "", None, DateTimeOffset.UtcNow);

        Assert.Equal("", draft.Elements);
    }

    [Fact]
    public void A_layout_from_the_browser_is_normalized_before_it_is_stored()
    {
        // The editor posts JSON. Storing it verbatim would mean a hand-written request could put
        // anything into the column and have it rendered back later, so the draft re-parses it.
        const string hostile = """
            [{"id":"a","role":"drop-tables","x":-9000,"style":{"color":"#fff;position:fixed","fontSize":9999}}]
            """;

        var draft = InvitationDraft.Create("owner", "Title", "Bride", "Groom", "", "", Guid.NewGuid(), 1,
            "#52796f", hostile, None, DateTimeOffset.UtcNow);

        Assert.True(CardLayout.TryParse(draft.Elements, out var stored));
        var element = Assert.Single(stored.Elements);

        Assert.Equal(CardRole.Text, element.Role);
        Assert.Equal(-25d, element.X);
        Assert.Equal("ink", element.Style.Color);
        Assert.Equal(30d, element.Style.FontSize);
    }

    [Fact]
    public void Unreadable_layout_json_falls_back_rather_than_being_stored()
    {
        var draft = InvitationDraft.Create("owner", "Title", "Bride", "Groom", "", "", Guid.NewGuid(), 1,
            "#52796f", "{ not json", None, DateTimeOffset.UtcNow);

        Assert.Equal("", draft.Elements);
    }

    [Fact]
    public void An_end_time_before_the_start_is_dropped()
    {
        // Almost always a typo. Printing "۲۲:۰۰ تا ۱۸:۰۰" on a wedding card is worse than
        // printing no end time at all.
        var backwards = new EventSchedule(new DateOnly(2026, 8, 15), new TimeOnly(18, 0), new TimeOnly(9, 0));

        var draft = InvitationDraft.Create("owner", "Title", "Bride", "Groom", "", "", Guid.NewGuid(), 1,
            "#52796f", Layout, backwards, DateTimeOffset.UtcNow);

        Assert.Equal(new TimeOnly(18, 0), draft.EventTime);
        Assert.Null(draft.EventEndTime);
    }

    [Fact]
    public void The_schedule_and_address_survive_creation()
    {
        var schedule = new EventSchedule(new DateOnly(2026, 8, 15), new TimeOnly(18, 0), new TimeOnly(23, 0));

        var draft = InvitationDraft.Create("owner", "Title", "Bride", "Groom", "", "  تهران، خیابان ولیعصر  ",
            Guid.NewGuid(), 1, "#52796f", Layout, schedule, DateTimeOffset.UtcNow);

        Assert.Equal("تهران، خیابان ولیعصر", draft.Address);
        Assert.Equal(new DateOnly(2026, 8, 15), draft.EventDate);
        Assert.Equal(new TimeOnly(23, 0), draft.EventEndTime);
    }
}
