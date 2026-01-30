namespace VianaHub.Global.Gerit.Application.Dtos.Request.Billing.Tenant;

public class CreateTenantRequest
{
    public string Name { get; set; }
    public bool Consent { get; set; } = true;
}
