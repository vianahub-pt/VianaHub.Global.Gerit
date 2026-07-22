namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitTeamEmployee;

public class VisitTeamEmployeeDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int VisitTeamId { get; set; }
    public int EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public int VisitTeamFunctionId { get; set; }
    public string? VisitTeamFunctionName { get; set; }
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
