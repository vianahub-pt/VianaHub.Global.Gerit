namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.Client;

/// <summary>
/// Request para atualização de Client
/// </summary>
public class UpdateClientRequest
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public bool Consent { get; set; }
}
