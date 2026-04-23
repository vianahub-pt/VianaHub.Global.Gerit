namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientFiscalData;

public class ClientFiscalDataResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int ClientIndividualId { get; set; }
    public string TaxNumber { get; set; }
    public string VatNumber { get; set; }
    public string FiscalCountry { get; set; }
    public bool IsVatRegistered { get; set; }
    public string IBAN { get; set; }
    public string FiscalEmail { get; set; }
    public bool IsActive { get; set; }
}
