namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitTeamEmployee;

public record VisitTeamEmployeeResponse
{
    public int Id { get; init; }
    public string? EmployeeName { get; init; }
    public string? VisitTeamFunctionName { get; init; }
    public bool IsLeader { get; init; }
    public DateTime StartDateTime { get; init; }
    public bool IsActive { get; init; }
}
