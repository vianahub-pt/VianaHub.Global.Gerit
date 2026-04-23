namespace VianaHub.Global.Gerit.Application.Dtos.Request.Billing.TenantContact;

public class CreateTenantContactRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsPrimary { get; set; }
}
