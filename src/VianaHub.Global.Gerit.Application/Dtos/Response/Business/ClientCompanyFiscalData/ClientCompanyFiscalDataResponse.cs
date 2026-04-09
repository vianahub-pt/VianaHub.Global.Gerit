namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientCompanyFiscalData;

public class ClientCompanyFiscalDataResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int ClientCompanyId { get; set; }
    public string TaxNumber { get; set; }
    public string VatNumber { get; set; }
    public string FiscalCountry { get; set; }
    public bool IsVatRegistered { get; set; }
    public string IBAN { get; set; }
    public string FiscalEmail { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
