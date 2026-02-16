using VianaHub.Global.Gerit.Domain.Enums;

namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.Equipment;

public class UpdateEquipmentRequest
{
    public int EquipmentTypeId { get; set; }
    public int StatusId { get; set; }
    public string Name { get; set; }
    public string SerialNumber { get; set; }
}
