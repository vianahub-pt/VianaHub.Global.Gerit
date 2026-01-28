namespace VianaHub.Global.Gerit.Application.Dtos.Request.Identity.UserRole;

public class UpdateUserRoleRequest
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RoleId { get; set; }
}