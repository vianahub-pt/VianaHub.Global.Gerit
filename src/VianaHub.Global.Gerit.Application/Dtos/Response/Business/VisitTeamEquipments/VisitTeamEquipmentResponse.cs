namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitTeamEquipments;

public class VisitTeamEquipmentResponse 
{
    public int Id { get; set; }
    public string? VisitTeam { get; set; }
    public string? Equipment { get; set; }
    public bool IsActive { get; set; }
}
