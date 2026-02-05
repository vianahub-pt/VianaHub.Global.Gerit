namespace VianaHub.Global.Gerit.Application.Dtos.Request.Identity.RolePermission;

public class BulkUploadRolePermissionItem
{
    public int RoleId { get; set; }
    public int ResourceId { get; set; }
    public int ActionId { get; set; }
}
