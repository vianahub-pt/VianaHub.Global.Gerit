namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientContact;

/// <summary>
/// Request para atualização de ClientContact
/// </summary>
public class UpdateClientContactRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsPrimary { get; set; }
}
