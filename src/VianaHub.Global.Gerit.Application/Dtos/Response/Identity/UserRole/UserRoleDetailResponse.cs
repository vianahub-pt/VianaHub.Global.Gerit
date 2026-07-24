namespace VianaHub.Global.Gerit.Application.Dtos.Response.Identity.UserRole;

/// <summary>
/// DTO de resposta detalhada para UserRole — inclui campos de auditoria.
/// Classe independente — não herda de UserRoleResponse.
/// </summary>
public class UserRoleDetailResponse
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? RoleName { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
