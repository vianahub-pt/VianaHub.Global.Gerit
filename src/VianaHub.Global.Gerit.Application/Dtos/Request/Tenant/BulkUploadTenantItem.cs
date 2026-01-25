namespace VianaHub.Global.Gerit.Application.Dtos.Request.Tenant;

public class BulkUploadTenantItem
{
    public string Name { get; set; }
    public bool Consent { get; set; } = true;
}
