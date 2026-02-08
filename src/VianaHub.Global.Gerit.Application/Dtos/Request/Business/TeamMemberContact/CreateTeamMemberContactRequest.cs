namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.TeamMemberContact;

/// <summary>
/// DTO para criação de TeamMemberContact
/// </summary>
public class CreateTeamMemberContactRequest
{
    public int TeamMemberId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsPrimary { get; set; }
}
