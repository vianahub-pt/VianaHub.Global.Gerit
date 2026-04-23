namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientConsents;

public class CreateClientConsentsRequest
{
    public int ClientId { get; set; }
    public int ConsentTypeId { get; set; }
    public bool Granted { get; set; }
    public DateTime GrantedDate { get; set; }
    public string Origin { get; set; }
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
}
