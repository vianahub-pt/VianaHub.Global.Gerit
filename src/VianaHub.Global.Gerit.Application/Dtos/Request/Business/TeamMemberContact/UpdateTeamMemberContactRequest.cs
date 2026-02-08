namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.TeamMemberContact;

/// <summary>
/// DTO para atualização de TeamMemberContact
/// </summary>
public class UpdateTeamMemberContactRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsPrimary { get; set; }
}
