namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.EmployeeTeams;

public class EmployeeTeamResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int TeamId { get; set; }
    public string Team { get; set; }
    public int EmployeeId { get; set; }
    public string Employee { get; set; }
    public bool IsLeader { get; set; }
    public bool IsActive { get; set; }
}
