namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientContact;

/// <summary>
/// Request para criação de ClientContact
/// </summary>
public class CreateClientContactRequest
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string CellPhoneNumber { get; set; }
    public bool IsWhatsapp { get; set; }
    public string Email { get; set; }
    public bool IsPrimary { get; set; }
}
