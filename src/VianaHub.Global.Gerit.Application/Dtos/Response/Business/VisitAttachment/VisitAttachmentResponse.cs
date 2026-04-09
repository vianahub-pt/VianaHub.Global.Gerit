namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitAttachment;

public record VisitAttachmentResponse
{
    public int Id { get; init; }
    public int TenantId { get; init; }
    public int FileTypeId { get; init; }
    public string FileTypeName { get; init; }
    public string MimeType { get; init; }
    public int VisitId { get; init; }
    public int AttachmentCategoryId { get; init; }
    public string AttachmentCategoryName { get; init; }
    public Guid PublicId { get; init; }
    public string S3Key { get; init; }
    public string FileName { get; init; }
    public long FileSizeBytes { get; init; }
    public string FormattedFileSize { get; init; }
    public int DisplayOrder { get; init; }
    public bool IsPrimary { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? ModifiedAt { get; init; }
}
