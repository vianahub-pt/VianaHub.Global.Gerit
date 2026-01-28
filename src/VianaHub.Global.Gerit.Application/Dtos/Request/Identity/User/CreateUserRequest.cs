namespace VianaHub.Global.Gerit.Application.Dtos.Request.Identity.User;

public class CreateUserRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}
