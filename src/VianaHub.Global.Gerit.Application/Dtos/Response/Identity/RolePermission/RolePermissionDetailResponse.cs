namespace VianaHub.Global.Gerit.Application.Dtos.Response.Identity.RolePermission;

/// <summary>
/// DTO de resposta detalhada para RolePermission — inclui campos de auditoria.
/// Classe independente — não herda de RolePermissionResponse.
/// </summary>
public class RolePermissionDetailResponse
{
    public int Id { get; set; }
    public string? RoleName { get; set; }
    public string? ResourceName { get; set; }
    public string? ActionName { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
