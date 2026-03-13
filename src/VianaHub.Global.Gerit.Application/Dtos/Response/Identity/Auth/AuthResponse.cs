namespace VianaHub.Global.Gerit.Application.Dtos.Response.Identity.Auth;

public class AuthResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime AccessTokenExpiresAt { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public int TenantId { get; set; }
    public string TenantName { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
}
