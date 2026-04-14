namespace VianaHub.Global.Gerit.Application.Dtos.Business.Client;

public class CreateClientContactRequest
{
    public string Name { get; set; } = null!;
    public string PhoneNumber { get; set; }
    public string CellPhoneNumber { get; set; }
    public bool IsWhatsapp { get; set; }
    public string Email { get; set; } = null!;
    public bool IsPrimary { get; set; }
}
