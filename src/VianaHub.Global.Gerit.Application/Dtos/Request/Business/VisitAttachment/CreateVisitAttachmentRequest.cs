namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitAttachment;

public record CreateVisitAttachmentRequest
{
    public int FileTypeId { get; init; }
    public int VisitId { get; init; }
    public int AttachmentCategoryId { get; init; }
    public string S3Key { get; init; }
    public string FileName { get; init; }
    public long FileSizeBytes { get; init; }
    public int DisplayOrder { get; init; }
    public bool IsPrimary { get; init; }
}
