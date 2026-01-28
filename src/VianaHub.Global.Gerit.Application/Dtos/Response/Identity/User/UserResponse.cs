namespace VianaHub.Global.Gerit.Application.Dtos.Response.Identity.User;

public class UserResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public string PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public DateTime? LastAccessAt { get; set; }
    public bool IsActive { get; set; }
}
