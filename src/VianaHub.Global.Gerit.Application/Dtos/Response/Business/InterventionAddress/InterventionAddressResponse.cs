namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionAddress;

/// <summary>
/// DTO de resposta para InterventionAddress
/// </summary>
public class InterventionAddressResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int InterventionId { get; set; }
    public string Intervention { get; set; }
    public int AddressTypeId { get; set; }
    public string AddressType { get; set; }
    public string CountryCode { get; set; }
    public string Street { get; set; }
    public string StreetNumber { get; set; }
    public string Complement { get; set; }
    public string Neighborhood { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public string PostalCode { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string Notes { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
}
