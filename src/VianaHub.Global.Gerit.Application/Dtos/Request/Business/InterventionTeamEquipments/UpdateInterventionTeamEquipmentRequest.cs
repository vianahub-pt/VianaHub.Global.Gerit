namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitTeamEquipments;

public class UpdateVisitTeamEquipmentRequest
{
    public int VisitTeamId { get; set; }
    public int EquipmentId { get; set; }
    public int ModifiedBy { get; set; }
}
