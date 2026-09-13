using System.ComponentModel.DataAnnotations;
namespace DigiCard.Contracts.Invitations;
public sealed class CreateDraftRequest
{
    public Guid TemplateId { get; set; }
    [Required, StringLength(100)] public string Title { get; set; } = "دعوت عروسی ما";
    [Required, StringLength(80)] public string BrideName { get; set; } = "";
    [Required, StringLength(80)] public string GroomName { get; set; } = "";
    [StringLength(1000)] public string Message { get; set; } = "با حضور شما شادی ما کامل می‌شود";
}
public sealed record DraftResponse(Guid Id, string Title, string BrideName, string GroomName,
    string Message, Guid TemplateId, int TemplateVersion, string Accent, DateTimeOffset CreatedAt);
