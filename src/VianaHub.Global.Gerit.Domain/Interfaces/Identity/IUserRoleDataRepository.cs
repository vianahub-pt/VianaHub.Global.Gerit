using VianaHub.Global.Gerit.Domain.Entities.Identity;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Identity;

public interface IUserRoleDataRepository
{
    Task<IList<UserRoleEntity>> GetAllAsync(int tenantId, CancellationToken ct);
    Task<UserRoleEntity> GetByIdAsync(int tenantId, int userId, int roleId, CancellationToken ct);
    Task<IList<UserRoleEntity>> GetByUserAsync(int tenantId, int userId, CancellationToken ct);
    Task<IList<UserRoleEntity>> GetByRoleAsync(int tenantId, int roleId, CancellationToken ct);
    Task<bool> ExistsAsync(int tenantId, int userId, int roleId, CancellationToken ct);
    
    Task<bool> CreateAsync(UserRoleEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(int tenantId, int userId, int roleId, CancellationToken ct);
}
