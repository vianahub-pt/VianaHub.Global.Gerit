namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.Employee;

public class EmployeeResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string Name { get; set; }
    public string TaxNumber { get; set; }
    public bool IsActive { get; set; }
}
