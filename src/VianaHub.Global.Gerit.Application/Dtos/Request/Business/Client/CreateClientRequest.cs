namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.Client;

public class CreateClientRequest
{
    public int ClientType { get; set; }
    public int OriginType { get; set; }
    public string UrlImage { get; set; }
    public string Note { get; set; }

    public CreateClientIndividualRequest? Individual { get; set; }
    public CreateClientCompanyRequest? Company { get; set; }
}
