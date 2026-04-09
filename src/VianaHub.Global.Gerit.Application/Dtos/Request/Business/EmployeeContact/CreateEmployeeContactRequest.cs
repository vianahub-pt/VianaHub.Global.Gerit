namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.EmployeeContact;

/// <summary>
/// DTO para cria��o de EmployeeContact
/// </summary>
public class CreateEmployeeContactRequest
{
    public int EmployeeId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsPrimary { get; set; }
}
