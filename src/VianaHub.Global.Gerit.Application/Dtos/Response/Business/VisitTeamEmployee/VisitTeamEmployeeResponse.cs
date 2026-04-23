namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitTeamEmployee;

public record VisitTeamEmployeeResponse
{
    public int Id { get; init; }
    public int TenantId { get; init; }
    public int VisitTeamId { get; init; }
    public int EmployeeId { get; init; }
    public string EmployeeName { get; init; }
    public int FunctionId { get; init; }
    public string FunctionName { get; init; }
    public bool IsLeader { get; init; }
    public DateTime StartDateTime { get; init; }
    public DateTime? EndDateTime { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? ModifiedAt { get; init; }
}
