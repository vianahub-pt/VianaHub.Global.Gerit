namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.EmployeeTeams;

public class EmployeeTeamsDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int TeamId { get; set; }
    public string? Team { get; set; }
    public int EmployeeId { get; set; }
    public string? Employee { get; set; }
    public bool IsLeader { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
