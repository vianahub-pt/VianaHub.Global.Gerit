namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitTeamVehicles;

public class VisitTeamVehicleDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int VisitTeamId { get; set; }
    public string? VisitTeam { get; set; }
    public int VehicleId { get; set; }
    public string? Vehicle { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
