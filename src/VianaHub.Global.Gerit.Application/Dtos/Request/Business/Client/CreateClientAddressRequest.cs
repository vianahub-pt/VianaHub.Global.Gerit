namespace VianaHub.Global.Gerit.Application.Dtos.Business.Client;

public class CreateClientAddressRequest
{
    public int AddressTypeId { get; set; }
    public string CountryCode { get; set; } = "PT";
    public string Street { get; set; } = null!;
    public string StreetNumber { get; set; }
    public string Complement { get; set; }
    public string Neighborhood { get; set; } = null!;
    public string City { get; set; } = null!;
    public string District { get; set; }
    public string PostalCode { get; set; } = null!;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string Notes { get; set; }
    public bool IsPrimary { get; set; }
}
