namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientFiscalData;

public class BulkUploadClientFiscalDataItem
{
    public int ClientId { get; set; }
    public string? TaxNumber { get; set; }
    public string? VatNumber { get; set; }
    public string? FiscalCountry { get; set; }
    public bool IsVatRegistered { get; set; }
    public string? IBAN { get; set; }
    public string? FiscalEmail { get; set; }
}
