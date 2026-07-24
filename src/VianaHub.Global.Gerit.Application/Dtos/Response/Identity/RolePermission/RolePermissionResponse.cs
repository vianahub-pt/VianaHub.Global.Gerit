namespace VianaHub.Global.Gerit.Application.Dtos.Response.Identity.RolePermission;

public class RolePermissionResponse
{
    public int Id { get; set; }
    public string? RoleName { get; set; }
    public string? ResourceName { get; set; }
    public string? ActionName { get; set; }
}
