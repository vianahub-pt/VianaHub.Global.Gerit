namespace VianaHub.Global.Gerit.Application.Dtos.Business.Client;

public class CreateClientConsentRequest
{
    public int ConsentTypeId { get; set; }
    public bool Granted { get; set; }
    public DateTime GrantedDate { get; set; } = DateTime.UtcNow;
    public string Origin { get; set; }
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
}
