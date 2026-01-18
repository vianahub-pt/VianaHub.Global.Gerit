namespace VianaHub.Global.Gerit.Application.Dtos.Request.Tenant;

public class BulkUploadTenantItem
{
    public string LegalName { get; set; }
    public string TradeName { get; set; }
    public bool Consent { get; set; } = true;
}
