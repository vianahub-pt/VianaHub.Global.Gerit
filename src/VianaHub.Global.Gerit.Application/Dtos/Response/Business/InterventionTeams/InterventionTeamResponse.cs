namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionTeams;

public class InterventionTeamResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int InterventionId { get; set; }
    public string Intervention { get; set; }
    public int TeamId { get; set; }
    public string Team { get; set; }
    public bool IsActive { get; set; }
}
