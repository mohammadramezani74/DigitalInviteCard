using DigiCard.Modules.Invitations.Domain;
namespace DigiCard.Tests;
public sealed class DraftTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Owner_is_required(string owner) => Assert.Throws<ArgumentException>(() =>
        InvitationDraft.Create(owner, "Title", "Bride", "Groom", "", Guid.NewGuid(), 1, "#52796f", DateTimeOffset.UtcNow));
    [Fact]
    public void Long_names_cannot_break_template_limits() => Assert.Throws<ArgumentException>(() =>
        InvitationDraft.Create("owner", "Title", new string('x',81), "Groom", "", Guid.NewGuid(), 1, "#52796f", DateTimeOffset.UtcNow));
    [Fact]
    public void Draft_keeps_the_selected_template_version_and_trims_text()
    {
        var id = Guid.NewGuid();
        var draft = InvitationDraft.Create("owner", "  Title  ", "سارا", "علی", "سلام", id, 4, "#52796f", DateTimeOffset.UtcNow);
        Assert.Equal(4, draft.TemplateVersion); Assert.Equal(id, draft.TemplateId); Assert.Equal("Title", draft.Title);
    }
}
