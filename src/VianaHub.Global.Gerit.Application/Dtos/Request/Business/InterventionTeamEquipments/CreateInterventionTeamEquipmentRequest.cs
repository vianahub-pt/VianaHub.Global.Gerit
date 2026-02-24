namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionTeamEquipments;

public class CreateInterventionTeamEquipmentRequest
{
    public int TenantId { get; set; }
    public int InterventionTeamId { get; set; }
    public int EquipmentId { get; set; }
    public int CreatedBy { get; set; }
}
