namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitTeamEquipments;

public class CreateVisitTeamEquipmentRequest
{
    public int TenantId { get; set; }
    public int VisitTeamId { get; set; }
    public int EquipmentId { get; set; }
    public int CreatedBy { get; set; }
}
