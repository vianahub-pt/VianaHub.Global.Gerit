namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.TeamMembersTeams;

public class TeamMembersTeamResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int TeamId { get; set; }
    public string Team { get; set; }
    public int TeamMemberId { get; set; }
    public string TeamMember { get; set; }
    public bool IsLeader { get; set; }
    public bool IsActive { get; set; }
}
