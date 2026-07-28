namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientAddress;

/// <summary>
/// DTO de resposta detalhada para ClientAddress — inclui campos de auditoria.
/// Classe independente — não herda de ClientAddressResponse.
/// </summary>
public class ClientAddressDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; private set; }
    public string? TenantName { get; private set; }
    public int ClientId { get; private set; }
    public string? ClientName { get; private set; }
    public int AddressTypeId { get; private set; }
    public string? AddressTypeName { get; private set; }

    public string? CountryCode { get; private set; } = null!;
    public string? Street { get; private set; } = null!;
    public string? StreetNumber { get; private set; }
    public string? Complement { get; private set; }
    public string? Neighborhood { get; private set; } = null!;
    public string? City { get; private set; } = null!;
    public string? District { get; private set; }
    public string? PostalCode { get; private set; } = null!;
    public decimal? Latitude { get; private set; }
    public decimal? Longitude { get; private set; }
    public string? Note { get; private set; }

    public bool IsPrimary { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; set; }
}
