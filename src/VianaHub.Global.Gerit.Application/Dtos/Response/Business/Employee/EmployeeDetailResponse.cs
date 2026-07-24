namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.Employee;

/// <summary>
/// DTO de resposta detalhada para Employee — inclui campos de auditoria.
/// Classe independente — não herda de EmployeeResponse.
/// </summary>
public class EmployeeDetailResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? CellPhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
