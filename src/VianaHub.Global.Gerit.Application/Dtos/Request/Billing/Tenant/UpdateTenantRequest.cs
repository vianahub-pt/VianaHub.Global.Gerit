namespace VianaHub.Global.Gerit.Application.Dtos.Request.Billing.Tenant;

public class UpdateTenantRequest
{
    public int TenantType { get; set; }
    public int OriginType { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
    public string UrlImage { get; set; }
    public string Note { get; set; }
}
