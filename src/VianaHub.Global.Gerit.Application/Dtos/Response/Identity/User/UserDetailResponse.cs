namespace VianaHub.Global.Gerit.Application.Dtos.Response.Identity.User;

/// <summary>
/// DTO de resposta detalhada para User — inclui campos de auditoria.
/// Classe independente — não herda de UserResponse.
/// </summary>
public class UserDetailResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? LastAccessAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
