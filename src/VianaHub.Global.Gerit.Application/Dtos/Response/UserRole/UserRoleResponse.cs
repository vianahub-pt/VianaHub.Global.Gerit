namespace VianaHub.Global.Gerit.Application.Dtos.Response.UserRole;

public class UserRoleResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public string UserName { get; set; }
    public string RoleName { get; set; }
}