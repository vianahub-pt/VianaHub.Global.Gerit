namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.Client;

/// <summary>
/// Request para atualização de Client
/// </summary>
public class UpdateClientRequest
{
    public int ClientType { get; set; }
    public int OriginType { get; set; }
    public string UrlImage { get; set; }
    public string Note { get; set; }

    public UpdateClientIndividualRequest Individual { get; set; }
    public UpdateClientCompanyRequest Company { get; set; }
}
