using VianaHub.Global.Gerit.Domain.Entities.Identity;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Identity;

public interface IRolePermissionDataRepository
{
    Task AddAsync(RolePermissionEntity entity, CancellationToken ct);
    Task<RolePermissionEntity> GetByIdAsync(int tenantId, int roleId, int resourceId, int actionId, CancellationToken ct);
    Task DeleteAsync(int tenantId, int roleId, int resourceId, int actionId, CancellationToken ct);
    Task<IList<RolePermissionEntity>> GetByRoleAsync(int roleId, int tenantId, CancellationToken ct);
    Task<IList<RolePermissionEntity>> GetByResourceAsync(int resourceId, int tenantId, CancellationToken ct);
    Task<IList<RolePermissionEntity>> GetAllAsync(int tenantId, CancellationToken ct);
    Task<bool> ExistsAsync(int tenantId, int roleId, int resourceId, int actionId, CancellationToken ct);
}
