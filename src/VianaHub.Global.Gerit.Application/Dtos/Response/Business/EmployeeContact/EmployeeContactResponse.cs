namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.EmployeeContact;

/// <summary>
/// DTO de resposta para EmployeeContact
/// </summary>
public class EmployeeContactResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? CellPhoneNumber { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
}
