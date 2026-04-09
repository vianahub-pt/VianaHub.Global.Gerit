using VianaHub.Global.Gerit.Application.Dtos.Base;

namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitTeamEquipments;

public class VisitTeamEquipmentResponse 
{
    public int VisitTeamId { get; set; }
    public string VisitTeam { get; set; }
    public int EquipmentId { get; set; }
    public string Equipment { get; set; }
}
