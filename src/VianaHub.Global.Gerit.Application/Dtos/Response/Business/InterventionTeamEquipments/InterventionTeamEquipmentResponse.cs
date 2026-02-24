using VianaHub.Global.Gerit.Application.Dtos.Base;

namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionTeamEquipments;

public class InterventionTeamEquipmentResponse 
{
    public int InterventionTeamId { get; set; }
    public string InterventionTeam { get; set; }
    public int EquipmentId { get; set; }
    public string Equipment { get; set; }
}
