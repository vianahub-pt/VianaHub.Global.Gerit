namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.EmployeeContact;

/// <summary>
/// DTO de resposta para a página de detalhe de EmployeeContact
/// </summary>
public class EmployeeContactDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int EmployeeId { get; set; }
    public string? Employee { get; set; }
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
