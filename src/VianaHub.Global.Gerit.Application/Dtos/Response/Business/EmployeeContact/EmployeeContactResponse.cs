namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.EmployeeContact;

/// <summary>
/// DTO de resposta para EmployeeContact
/// </summary>
public class EmployeeContactResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int EmployeeId { get; set; }
    public string Employee { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
}
