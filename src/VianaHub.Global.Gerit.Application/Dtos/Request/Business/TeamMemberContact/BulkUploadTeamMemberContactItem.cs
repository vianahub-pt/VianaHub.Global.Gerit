namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.TeamMemberContact;

/// <summary>
/// Item para bulk upload de TeamMemberContacts via CSV
/// </summary>
public class BulkUploadTeamMemberContactItem
{
    public int TeamMemberId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsPrimary { get; set; }
}
