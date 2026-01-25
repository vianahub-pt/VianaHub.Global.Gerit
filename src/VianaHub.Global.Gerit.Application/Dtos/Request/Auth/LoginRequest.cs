namespace VianaHub.Global.Gerit.Application.Dtos.Request.Auth;

public class LoginRequest
{
    public int TenantId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
