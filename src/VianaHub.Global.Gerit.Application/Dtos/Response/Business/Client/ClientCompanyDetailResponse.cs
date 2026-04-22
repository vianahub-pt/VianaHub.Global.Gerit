namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.Client;

public class ClientCompanyDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int ClientId { get; set; }
    public string LegalName { get; set; }
    public string TradeName { get; set; }
    public string PhoneNumber { get; set; }
    public string CellPhoneNumber { get; set; }
    public bool IsWhatsapp { get; set; }
    public string Email { get; set; }
    public string Site { get; set; }
    public string CompanyRegistration { get; set; }
    public string CAE { get; set; }
    public int? NumberOfEmployee { get; set; }
    public string LegalRepresentative { get; set; }
    public bool IsActive { get; set; }
}
