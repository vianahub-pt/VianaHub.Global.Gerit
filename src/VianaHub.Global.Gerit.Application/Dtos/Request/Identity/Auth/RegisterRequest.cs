namespace VianaHub.Global.Gerit.Application.Dtos.Request.Identity.Auth;

public class RegisterRequest
{
    public int TenantId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
}
