namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionContact;

/// <summary>
/// Response de InterventionContact
/// </summary>
public class InterventionContactResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int InterventionId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
}
