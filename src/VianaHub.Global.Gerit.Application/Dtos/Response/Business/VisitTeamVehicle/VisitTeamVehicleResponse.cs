namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitTeamVehicle;

public class VisitTeamVehicleResponse
{
    public int Id { get; set; }
    public string? VisitTeam { get; set; }
    public string? Vehicle { get; set; }
    public bool IsActive { get; set; }
}
