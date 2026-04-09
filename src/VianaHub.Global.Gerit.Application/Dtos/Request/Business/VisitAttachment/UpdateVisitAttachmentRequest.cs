namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitAttachment;

public record UpdateVisitAttachmentRequest
{
    public int AttachmentCategoryId { get; init; }
    public string FileName { get; init; }
    public int DisplayOrder { get; init; }
    public bool IsPrimary { get; init; }
}
