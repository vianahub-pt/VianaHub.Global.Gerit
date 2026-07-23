namespace VianaHub.Global.Gerit.Application.Dtos.Response.Billing.TenantContact;

/// <summary>
/// DTO de resposta de detalhe para TenantContact (inclui todos os campos da grid + campos de auditoria).
/// Classe independente — não herda de TenantContactResponse.
/// </summary>
public class TenantContactDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
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
