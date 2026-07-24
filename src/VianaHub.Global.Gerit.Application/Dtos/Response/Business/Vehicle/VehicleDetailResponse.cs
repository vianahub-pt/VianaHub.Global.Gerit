namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.Vehicle;

/// <summary>
/// Resposta detalhada de um Vehicle (inclui campos de auditoria).
/// Classe independente — não herda de VehicleResponse.
/// </summary>
public class VehicleDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int StatusDefinitionId { get; set; }
    public string? StatusDefinitionName { get; set; }
    public int StatusDomainId { get; set; }
    public string? StatusDomainName { get; set; }
    public string? Plate { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public int Year { get; set; }
    public string? Color { get; set; }
    public string? FuelType { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
