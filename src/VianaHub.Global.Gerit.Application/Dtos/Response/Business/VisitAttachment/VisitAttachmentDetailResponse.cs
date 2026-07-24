namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitAttachment;

/// <summary>
/// DTO de resposta detalhada para VisitAttachment — inclui campos de auditoria.
/// Classe independente — não herda de VisitAttachmentResponse.
/// </summary>
public class VisitAttachmentDetailResponse
{
    public int Id { get; set; }
    public string? FileTypeName { get; set; }
    public string? MimeType { get; set; }
    public string? FileName { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
