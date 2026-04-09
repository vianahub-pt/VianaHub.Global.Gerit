namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitAddress;

/// <summary>
/// DTO de resposta para VisitAddress
/// </summary>
public class VisitAddressResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int VisitId { get; set; }
    public string Visit { get; set; }
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
