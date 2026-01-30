namespace VianaHub.Global.Gerit.Application.Dtos.Request.Billing.Tenant;

public class BulkUploadTenantItem
{
    public string Name { get; set; }
    public bool Consent { get; set; } = true;
}
