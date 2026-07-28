namespace VianaHub.Global.Gerit.Application.Dtos.Response.Billing.TenantContactPerson;

public class TenantContactPersonResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? CellPhoneNumber { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
}
