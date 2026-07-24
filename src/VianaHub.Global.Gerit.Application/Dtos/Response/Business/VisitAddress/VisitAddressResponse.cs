namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitAddress;

/// <summary>
/// DTO de resposta para VisitAddress
/// </summary>
public class VisitAddressResponse
{
    public int Id { get; set; }
    public string? AddressTypeName { get; set; }
    public string? CountryCode { get; set; }
    public string? Street { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
}
