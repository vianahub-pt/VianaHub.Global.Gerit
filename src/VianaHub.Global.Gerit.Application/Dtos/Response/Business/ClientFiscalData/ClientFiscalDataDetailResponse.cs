namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientFiscalData;

public class ClientFiscalDataDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int ClientTypeId { get; set; }
    public string ClientType { get; set; }
    public int ClientId { get; set; }
    public string Client { get; set; }
    public string TaxNumber { get; set; }
    public string VatNumber { get; set; }
    public string FiscalCountry { get; set; }
    public bool IsVatRegistered { get; set; }
    public string IBAN { get; set; }
    public string FiscalEmail { get; set; }
    public bool IsActive { get; set; }
}
