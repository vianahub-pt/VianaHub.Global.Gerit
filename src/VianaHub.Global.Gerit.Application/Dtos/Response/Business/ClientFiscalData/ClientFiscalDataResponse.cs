namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientFiscalData;

public class ClientFiscalDataResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int ClientTypeId { get; set; }
    public string ClientType { get; set; }
    public int ClientId { get; set; }
    public string Client { get; set; }
    public string TaxNumber { get; set; }
    public bool IsActive { get; set; }
}
