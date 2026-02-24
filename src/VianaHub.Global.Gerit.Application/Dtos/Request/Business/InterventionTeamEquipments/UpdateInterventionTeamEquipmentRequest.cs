namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionTeamEquipments;

public class UpdateInterventionTeamEquipmentRequest
{
    public int InterventionTeamId { get; set; }
    public int EquipmentId { get; set; }
    public int ModifiedBy { get; set; }
}
