namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.Equipment;

/// <summary>
/// Resposta detalhada de um Equipment (inclui campos de auditoria).
/// Classe independente — não herda de EquipmentResponse.
/// </summary>
public class EquipmentDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int EquipmentTypeId { get; set; }
    public string? EquipmentType { get; set; }
    public int StatusDefinitionId { get; set; }
    public string? StatusDefinitionName { get; set; }
    public int StatusDomainId { get; set; }
    public string? StatusDomainName { get; set; }
    public string? StatusDefinition { get; set; }
    public string? Name { get; set; }
    public string? SerialNumber { get; set; }
    public string? UrlImage { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
