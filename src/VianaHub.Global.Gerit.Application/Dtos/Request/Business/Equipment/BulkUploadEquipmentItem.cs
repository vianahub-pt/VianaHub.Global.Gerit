namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.Equipment;

public class BulkUploadEquipmentItem
{
    public int EquipmentTypeId { get; set; }
    public int StatusDefinitionId { get; set; }
    public int StatusDomainId { get; set; }
    public string? Name { get; set; }
    public string? SerialNumber { get; set; }
    public string? UrlImage { get; set; }
}
