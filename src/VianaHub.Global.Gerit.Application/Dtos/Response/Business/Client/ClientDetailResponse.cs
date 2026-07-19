namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.Client;

public class ClientDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string? Tenant { get; set; }
    public int PartyTypeId { get; set; }
    public string? PartyType { get; set; }
    public int AcquisitionSourceTypeId { get; set; }
    public string? AcquisitionSourceType { get; set; }
    public string? UrlImage { get; set; }
    public string? Note { get; set; }

    // Campos unificados
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CellPhoneNumber { get; set; }
    public bool IsCellPhoneWhatsapp { get; set; }
    public string? Email { get; set; }
    public string? WebsiteUrl { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Gender { get; set; }
    public string? Nationality { get; set; }
    public string? CompanyRegistrationNumber { get; set; }
    public string? EconomicActivityCode { get; set; }
    public int? NumberOfEmployees { get; set; }
    public int? StatusDefinitionId { get; set; }
    public string? StatusDefinitionName { get; set; }
    public int? StatusDomainId { get; set; }
    public string? StatusDomainName { get; set; }

    public bool IsActive { get; set; }
}
