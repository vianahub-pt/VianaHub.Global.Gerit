namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.Client;

/// <summary>
/// Request para criação de Client
/// </summary>
public class CreateClientRequest
{
    public int ClientType { get; set; }
    public int Origin { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
    public string UrlImage { get; set; }
    public int Score { get;  set; }
    public int ConsentType { get; set; }
    public bool Consent { get; set; }
    public DateTime ConsentDate { get; set; }
    public string Remarks { get; set; }
}
