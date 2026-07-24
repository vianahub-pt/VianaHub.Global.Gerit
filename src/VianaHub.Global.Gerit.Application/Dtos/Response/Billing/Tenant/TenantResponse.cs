namespace VianaHub.Global.Gerit.Application.Dtos.Response.Billing.Tenant;

public class TenantResponse
{
    public int Id { get; set; }
    public string? AcquisitionSourceTypeName { get; set; }
    public string? Name { get; set; }
    public bool IsActive { get; set; }
}
