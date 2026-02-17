namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionContact;

/// <summary>
/// Request para criação de InterventionContact
/// </summary>
public class CreateInterventionContactRequest
{
    public int InterventionId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsPrimary { get; set; }
}
