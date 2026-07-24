namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.EmployeeContact;

/// <summary>
/// DTO para cria��o de EmployeeContact
/// </summary>
public class CreateEmployeeContactPersonRequest
{
    public int EmployeeId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? JobTitle { get; set; }
    public string? Department { get; set; }
    public string? CellPhoneNumber { get; set; }
    public bool IsCellPhoneWhatsapp { get; set; }
    public bool IsPrimary { get; set; }
}
