using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities;

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
    public UserRoleEntity(int tenantId, int userId, int roleId)
    {
        TenantId = tenantId;
        UserId = userId;
        RoleId = roleId;
    }
}
