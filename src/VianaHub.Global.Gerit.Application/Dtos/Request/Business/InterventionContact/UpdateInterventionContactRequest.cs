namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionContact;

/// <summary>
/// Request para atualização de InterventionContact
/// </summary>
public class UpdateInterventionContactRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsPrimary { get; set; }
}
