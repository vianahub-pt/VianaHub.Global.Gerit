namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.EmployeeTeams;

public class BulkUploadEmployeeTeamItem
{
    public int TeamId { get; set; }
    public int EmployeeId { get; set; }
    public bool IsLeader { get; set; }
}
