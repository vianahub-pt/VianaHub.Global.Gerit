namespace VianaHub.Global.Gerit.Application.Dtos.Request.Identity.Auth;

public class RefreshRequest
{
    public int TenantId { get; set; }
    public string RefreshToken { get; set; }
}
