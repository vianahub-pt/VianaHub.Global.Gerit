namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientAddress;

/// <summary>
/// DTO de resposta detalhada para ClientAddress — inclui campos de auditoria.
/// Classe independente — não herda de ClientAddressResponse.
/// </summary>
public class ClientAddressDetailResponse
{
    public int Id { get; set; }
    public string? AddressTypeName { get; set; }
    public string? CountryCode { get; set; }
    public string? Street { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
