namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.TeamMemberContact;

/// <summary>
/// DTO de resposta para TeamMemberContact
/// </summary>
public class TeamMemberContactResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int TeamMemberId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
}
