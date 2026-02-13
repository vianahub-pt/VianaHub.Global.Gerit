namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.Client;

/// <summary>
/// Request para criação de Client
/// </summary>
public class CreateClientRequest
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public bool Consent { get; set; }
}
