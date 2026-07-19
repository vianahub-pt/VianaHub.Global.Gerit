namespace VianaHub.Global.Gerit.Application.Dtos.Response.Billing.TenantAddress;

public class TenantAddressDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int AddressTypeId { get; set; }
    public string? CountryCode { get; set; }
    public string? Street { get; set; }
    public string? Neighborhood { get; set; }
    public string? City { get; set; }
    public string? District { get; set; }
    public string? PostalCode { get; set; }
    public string? StreetNumber { get; set; }
    public string? Complement { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? Note { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
