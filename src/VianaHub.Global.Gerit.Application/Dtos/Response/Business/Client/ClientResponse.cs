namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.Client;

/// <summary>
/// Response de Client (listagem)
/// </summary>
public class ClientResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string? Tenant { get; set; }
    public int PartyTypeId { get; set; }
    public string? PartyType { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CellPhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Contact { get; set; }
    public bool IsActive { get; set; }
}
