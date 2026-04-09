namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitTeamEmployee;

public record CreateVisitTeamEmployeeRequest
{
    public int VisitTeamId { get; init; }
    public int EmployeeId { get; init; }
    public int FunctionId { get; init; }
    public bool IsLeader { get; init; }
    public DateTime StartDateTime { get; init; }
}
