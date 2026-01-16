using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities;

/// <summary>
/// Entidade que representa a permissão de uma Role sobre um Resource com uma ActionEntity
/// </summary>
public class RolePermissionEntity : Entity
{
    public int TenantId { get; private set; }
    public int RoleId { get; private set; }
    public int ResourceId { get; private set; }
    public int ActionId { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }
    public RoleEntity Role { get; private set; }
    public ResourceEntity Resource { get; private set; }
    public ActionEntity Action { get; private set; }

    // Construtor protegido para o EF Core
    protected RolePermissionEntity() { }

    /// <summary>
    /// Construtor para criação de uma nova permissão de Role
    /// </summary>
    public RolePermissionEntity(int tenantId, int roleId, int resourceId, int actionId)
    {
        if (tenantId <= 0)
            throw new ArgumentException("TenantId inválido.", nameof(tenantId));

        if (roleId <= 0)
            throw new ArgumentException("RoleId inválido.", nameof(roleId));

        if (resourceId <= 0)
            throw new ArgumentException("ResourceId inválido.", nameof(resourceId));

        if (actionId <= 0)
            throw new ArgumentException("ActionId inválido.", nameof(actionId));

        TenantId = tenantId;
        RoleId = roleId;
        ResourceId = resourceId;
        ActionId = actionId;
    }
}
