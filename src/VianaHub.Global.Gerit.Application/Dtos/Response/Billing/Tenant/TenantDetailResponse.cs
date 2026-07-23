namespace VianaHub.Global.Gerit.Application.Dtos.Response.Billing.Tenant;

/// <summary>
/// DTO de resposta de detalhe para Tenant (inclui todos os campos da grid + campos de auditoria).
/// Classe independente — não herda de TenantResponse.
/// </summary>
public class TenantDetailResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool Consent { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public int AcquisitionSourceTypeId { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
