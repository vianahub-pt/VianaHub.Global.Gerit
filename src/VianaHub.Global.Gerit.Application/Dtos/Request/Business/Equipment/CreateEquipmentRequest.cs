using VianaHub.Global.Gerit.Domain.Enums;

namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.Equipment;

public class CreateEquipmentRequest
{
    public string Name { get; set; }
    public string SerialNumber { get; set; }
    public TypeEquipament TypeEquipament { get; set; }
}
