using System.Collections.Generic;
using System.Threading.Tasks;
using VianaHub.Global.Gerit.Domain.Entities;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Repository;

public interface IRolePermissionDataRepository
{
    Task AddAsync(RolePermissionEntity entity);
    Task<RolePermissionEntity> GetByIdAsync(int id, int tenantId);
    Task DeleteAsync(int id, int tenantId);
    Task<IList<RolePermissionEntity>> GetByRoleAsync(int roleId, int tenantId);
    Task<IList<RolePermissionEntity>> GetByResourceAsync(int resourceId, int tenantId);
    Task<IList<RolePermissionEntity>> GetAllAsync(int tenantId);
    Task<bool> ExistsAsync(int tenantId, int roleId, int resourceId, int actionId);
}
