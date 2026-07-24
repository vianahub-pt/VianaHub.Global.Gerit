namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.Client;

/// <summary>
/// Response de Client (listagem)
/// </summary>
public class ClientResponse
{
    public int Id { get; set; }
    public string? PartyTypeName { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
}
