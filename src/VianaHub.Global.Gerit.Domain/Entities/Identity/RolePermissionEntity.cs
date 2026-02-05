using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Identity;

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
    public RolePermissionEntity(int tenantId, int roleId, int resourceId, int actionId, int createdBy)
    {
        TenantId = tenantId;
        RoleId = roleId;
        ResourceId = resourceId;
        ActionId = actionId;
        CreatedBy = createdBy;
    }
}
