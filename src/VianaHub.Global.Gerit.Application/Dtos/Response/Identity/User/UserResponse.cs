namespace VianaHub.Global.Gerit.Application.Dtos.Response.Identity.User;

public class UserResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? LastAccessAt { get; set; }
    public bool IsActive { get; set; }
}
