namespace VianaHub.Global.Gerit.Application.Dtos.Request.Identity.User;

public class UpdatePasswordRequest
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}
