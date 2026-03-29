namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.Client;

public class ClientDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int ClientType { get; set; }
    public string ClientTypeDescription { get; set; }
    public int Origin { get; set; }
    public string OriginDescription { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
    public string UrlImage { get; set; }
    public int? Score { get; set; }
    public bool Consent { get; set; }
    public string Remarks { get; set; }
    public bool IsActive { get; set; }
}
