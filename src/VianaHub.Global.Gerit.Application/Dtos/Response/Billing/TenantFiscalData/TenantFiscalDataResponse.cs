namespace VianaHub.Global.Gerit.Application.Dtos.Response.Billing.TenantFiscalData;

public class TenantFiscalDataResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string? TaxNumber { get; set; }
    public string? FiscalCountry { get; set; }
    public bool IsVatRegistered { get; set; }
    public bool IsActive { get; set; }
}
