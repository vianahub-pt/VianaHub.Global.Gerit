namespace VianaHub.Global.Gerit.Application.Dtos.Request.User;

public class UpdatePasswordRequest
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}
