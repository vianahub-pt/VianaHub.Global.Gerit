namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.EmployeeAddress;

/// <summary>
/// DTO de resposta de detalhe para EmployeeAddress (inclui campos de auditoria e campos completos do endere├ºo).
/// Classe independente ÔÇö n├úo herda de EmployeeAddressResponse.
/// </summary>
public class EmployeeAddressDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int EmployeeId { get; set; }
    public string? Employee { get; set; }
    public int AddressTypeId { get; set; }
    public string? AddressType { get; set; }
    public string? Street { get; set; }
    public string? StreetNumber { get; set; }
    public string? Complement { get; set; }
    public string? Neighborhood { get; set; }
    public string? City { get; set; }
    public string? District { get; set; }
    public string? PostalCode { get; set; }
    public string? CountryCode { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? Note { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
