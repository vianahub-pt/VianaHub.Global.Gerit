namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.Client;

/// <summary>
/// Response de Client
/// </summary>
public class ClientResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int ClientType { get; set; }
    public string ClientTypeDescription { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Contact { get; set; }
    public bool IsActive { get; set; }
}
