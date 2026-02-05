using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Identity;

/// <summary>
/// Entidade que representa a relação entre User e Role
/// </summary>
public class UserRoleEntity : Entity
{
    public int TenantId { get; private set; }
    public int UserId { get; private set; }
    public int RoleId { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }
    public UserEntity User { get; private set; }
    public RoleEntity Role { get; private set; }

    // Construtor protegido para o EF Core
    protected UserRoleEntity() { }

    /// <summary>
    /// Construtor para criação de uma nova relação User-Role
    /// </summary>
    public UserRoleEntity(int tenantId, int userId, int roleId, int createdBy)
    {
        TenantId = tenantId;
        UserId = userId;
        RoleId = roleId;
        CreatedBy = createdBy;
    }
}
