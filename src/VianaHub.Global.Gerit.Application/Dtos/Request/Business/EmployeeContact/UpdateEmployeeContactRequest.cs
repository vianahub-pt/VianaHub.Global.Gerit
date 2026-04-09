namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.EmployeeContact;

/// <summary>
/// DTO para atualiza��o de EmployeeContact
/// </summary>
public class UpdateEmployeeContactRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsPrimary { get; set; }
}
