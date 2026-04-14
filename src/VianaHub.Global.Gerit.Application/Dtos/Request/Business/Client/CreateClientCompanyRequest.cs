namespace VianaHub.Global.Gerit.Application.Dtos.Business.Client;

public class CreateClientCompanyRequest
{
    public string LegalName { get; set; } = null!;
    public string TradeName { get; set; }
    public string Site { get; set; }
    public string CompanyRegistration { get; set; }
    public string Cae { get; set; }
    public int NumberOfEmployee { get; set; }
    public string LegalRepresentative { get; set; }
}
