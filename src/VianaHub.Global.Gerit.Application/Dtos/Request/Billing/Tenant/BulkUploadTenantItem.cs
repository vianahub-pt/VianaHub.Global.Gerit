namespace VianaHub.Global.Gerit.Application.Dtos.Request.Billing.Tenant;

public class BulkUploadTenantItem
{
    public int TenantType { get; set; }
    public int OriginType { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
    public string UrlImage { get; set; }
    public string Note { get; set; }
}
