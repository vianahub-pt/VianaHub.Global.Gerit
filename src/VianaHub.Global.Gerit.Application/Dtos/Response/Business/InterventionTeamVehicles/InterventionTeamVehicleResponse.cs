namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionTeamVehicles;

public class InterventionTeamVehicleResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int InterventionTeamId { get; set; }
    public string InterventionTeam { get; set; }
    public int VehicleId { get; set; }
    public string Vehicle { get; set; }
    public bool IsActive { get; set; }
}
