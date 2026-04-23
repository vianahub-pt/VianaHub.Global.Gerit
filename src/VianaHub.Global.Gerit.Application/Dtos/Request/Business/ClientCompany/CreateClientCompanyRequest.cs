namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientCompany;

public class CreateClientCompanyRequest
{
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
}
