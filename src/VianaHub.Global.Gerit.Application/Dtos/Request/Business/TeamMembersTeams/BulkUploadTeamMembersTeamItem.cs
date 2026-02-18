namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.TeamMembersTeams;

public class BulkUploadTeamMembersTeamItem
{
    public int TeamId { get; set; }
    public int TeamMemberId { get; set; }
    public bool IsLeader { get; set; }
}
