namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.Client;

public class ClientDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int ClientType { get; set; }
    public string ClientTypeDescription { get; set; }
    public int OriginType { get; set; }
    public string OriginTypeDescription { get; set; }
    public string UrlImage { get; set; }
    public string Note { get; set; }
    public bool IsActive { get; set; }

    public ClientIndividualDetailResponse? Individual { get; set; }
    public ClientCompanyDetailResponse? Company { get; set; }
}
