using System.Collections.Generic;
using System.Threading.Tasks;
using VianaHub.Global.Gerit.Domain.Entities.Identity;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Identity;

public interface IUserRoleDataRepository
{
    Task AddAsync(UserRoleEntity entity);
    Task<UserRoleEntity> GetByIdAsync(int id, int tenantId);
    Task DeleteAsync(int id, int tenantId);
    Task<IList<UserRoleEntity>> GetByUserAsync(int userId, int tenantId);
    Task<IList<UserRoleEntity>> GetByRoleAsync(int roleId, int tenantId);
    Task<IList<UserRoleEntity>> GetAllAsync(int tenantId);
}
