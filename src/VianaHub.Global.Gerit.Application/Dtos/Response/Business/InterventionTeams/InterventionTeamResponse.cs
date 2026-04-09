namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitTeams;

public class VisitTeamResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int VisitId { get; set; }
    public string Visit { get; set; }
    public int TeamId { get; set; }
    public string Team { get; set; }
    public bool IsActive { get; set; }
}
