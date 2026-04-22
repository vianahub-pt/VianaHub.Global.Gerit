namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientContact;

/// <summary>
/// Request para atualização de ClientContact
/// </summary>
public class UpdateClientContactRequest
{
    public int ClientId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string CellPhoneNumber { get; set; }
    public bool IsWhatsapp { get; set; }
    public bool IsPrimary { get; set; }
}
