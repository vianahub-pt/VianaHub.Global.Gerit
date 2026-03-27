namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.Client;

/// <summary>
/// Response de Client
/// </summary>
public class ClientResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Contacto { get; set; }
    public bool Consent { get; set; }
    public bool IsActive { get; set; }
}
