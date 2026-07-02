namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.Client;

/// <summary>
/// Request para atualiza��o de Client
/// </summary>
public class UpdateClientRequest
{
    public int ClientType { get; set; }
    public int AcquisitionSourceTypeId { get; set; }
    public string? UrlImage { get; set; }
    public string? Note { get; set; }

    public UpdateClientIndividualRequest Individual { get; set; }
    public UpdateClientCompanyRequest Company { get; set; }
}
