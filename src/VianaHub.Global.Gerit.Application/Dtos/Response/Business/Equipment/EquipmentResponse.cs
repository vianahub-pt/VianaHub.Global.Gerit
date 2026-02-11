namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.Equipment;

public class EquipmentResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int EquipmentTypeId { get; set; }
    public string EquipmentType { get; set; }
    public string Name { get; set; }
    public string SerialNumber { get; set; }
    public int Status { get; set; }
    public bool IsActive { get; set; }
}
