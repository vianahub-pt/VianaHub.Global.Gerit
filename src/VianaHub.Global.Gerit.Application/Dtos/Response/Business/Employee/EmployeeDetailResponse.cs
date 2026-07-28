namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.Employee;

/// <summary>
/// Resposta detalhada de um Employee (inclui campos de auditoria).
/// Classe independente — não herda de EmployeeResponse.
/// </summary>
public class EmployeeDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int StatusDefinitionId { get; set; }
    public string? StatusDefinitionName { get; set; }
    public int StatusDomainId { get; set; }
    public string? StatusDomainName { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CellPhoneNumber { get; set; }
    public bool IsCellPhoneWhatsapp { get; set; }
    public string? Email { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
