namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.EquipmentType;

/// <summary>
/// DTO de resposta de detalhe para EquipmentType (inclui campos de auditoria).
/// Classe independente — não herda de EquipmentTypeResponse.
/// </summary>
public class EquipmentTypeDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
