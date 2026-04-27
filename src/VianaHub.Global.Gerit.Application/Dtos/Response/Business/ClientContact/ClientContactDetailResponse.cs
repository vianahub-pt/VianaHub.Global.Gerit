namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientContact;

public class ClientContactDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; private set; }
    public int ClientId { get; private set; }
    public string Client { get; set; }

    public string Name { get; private set; } = null!;
    public string PhoneNumber { get; private set; }
    public string CellPhoneNumber { get; private set; }
    public bool IsWhatsapp { get; private set; }
    public string Email { get; private set; } = null!;

    public bool IsPrimary { get; private set; }
    public bool IsActive { get; private set; }
}
