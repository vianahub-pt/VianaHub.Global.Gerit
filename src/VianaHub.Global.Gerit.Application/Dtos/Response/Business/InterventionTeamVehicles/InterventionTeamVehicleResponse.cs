namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitTeamVehicles;

public class VisitTeamVehicleResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int VisitTeamId { get; set; }
    public string VisitTeam { get; set; }
    public int VehicleId { get; set; }
    public string Vehicle { get; set; }
    public bool IsActive { get; set; }
}
