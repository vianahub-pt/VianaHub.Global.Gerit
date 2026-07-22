namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitContact;

/// <summary>
/// Response de detalhe de VisitContact
/// </summary>
public class VisitContactDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int VisitId { get; set; }
    public string? Visit { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? JobTitle { get; set; }
    public string? Department { get; set; }
    public string? CellPhoneNumber { get; set; }
    public bool IsCellPhoneWhatsapp { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
