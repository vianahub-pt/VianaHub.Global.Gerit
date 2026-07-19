namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.Equipment;

public class EquipmentResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int EquipmentTypeId { get; set; }
    public string? EquipmentType { get; set; }
    public int StatusDefinitionId { get; set; }
    public int StatusDomainId { get; set; }
    public string? StatusDefinition { get; set; }
    public string? Name { get; set; }
    public string? SerialNumber { get; set; }
    public string? UrlImage { get; set; }
    public bool IsActive { get; set; }
}
