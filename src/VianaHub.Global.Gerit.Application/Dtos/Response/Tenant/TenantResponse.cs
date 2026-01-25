namespace VianaHub.Global.Gerit.Application.Dtos.Response.Tenant;

public class TenantResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Consent { get; set; }
    public bool IsActive { get; set; }
}
