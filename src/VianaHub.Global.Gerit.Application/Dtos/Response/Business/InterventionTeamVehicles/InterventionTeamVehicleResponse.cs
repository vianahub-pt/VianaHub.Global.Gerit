namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionTeamVehicles;

public class InterventionTeamVehicleResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int InterventionId { get; set; }
    public int VehicleId { get; set; }
    public bool IsActive { get; set; }
}
