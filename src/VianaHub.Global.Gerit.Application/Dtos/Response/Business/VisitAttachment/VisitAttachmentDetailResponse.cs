namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitAttachment;

/// <summary>
/// DTO de resposta detalhada para VisitAttachment — inclui campos de auditoria.
/// Classe independente — não herda de VisitAttachmentResponse.
/// </summary>
public class VisitAttachmentDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int FileTypeId { get; set; }
    public string? FileTypeName { get; set; }
    public string? MimeType { get; set; }
    public int VisitId { get; set; }
    public Guid PublicId { get; set; }
    public string? S3Key { get; set; }
    public string? FileName { get; set; }
    public long FileSizeBytes { get; set; }
    public string? FormattedFileSize { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
