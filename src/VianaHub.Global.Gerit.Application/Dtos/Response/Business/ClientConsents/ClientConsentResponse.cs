namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientConsents;

public class ClientConsentResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int ClientId { get; set; }
    public string? Client { get; set; }
    public int ConsentTypeId { get; set; }
    public string? ConsentType { get; set; }
    public int ConsentOriginTypeId { get; set; }
    public string? ConsentOriginType { get; set; }
    public bool Granted { get; set; }
    public DateTime GrantedDate { get; set; }
    public DateTime? RevokedDate { get; set; }
}
