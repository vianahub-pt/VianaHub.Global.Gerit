namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitTeamEmployee;

public class BulkUploadVisitTeamEmployeeItem
{
    public int EmployeeId { get; set; }
    public int VisitTeamFunctionId { get; set; }
    public bool IsLeader { get; set; }
    public DateTime StartDateTime { get; set; }
}
