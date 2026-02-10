using VianaHub.Global.Gerit.Domain.Enums;

namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.Equipment;

public class UpdateEquipmentRequest
{
    public string Name { get; set; }
    public string SerialNumber { get; set; }
    public EquipamentType EquipamentType { get; set; }
    public EquipmentStatus Status { get; set; }
}
