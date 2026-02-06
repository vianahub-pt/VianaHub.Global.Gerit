namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.TeamMember;

public class TeamMemberResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int FunctionId { get; set; }
    public string Name { get; set; }
    public string TaxNumber { get; set; }
    public bool IsActive { get; set; }
}
