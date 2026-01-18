using System.Collections.Generic;
using System.Threading.Tasks;
using VianaHub.Global.Gerit.Application.Dtos.Request.UserRole;
using VianaHub.Global.Gerit.Application.Dtos.Response.UserRole;

namespace VianaHub.Global.Gerit.Application.Interfaces;

public interface IUserRoleAppService
{
    Task<UserRoleResponse> CreateAsync(CreateUserRoleRequest request);
    Task<UserRoleResponse> UpdateAsync(UpdateUserRoleRequest request);
    Task DeleteAsync(int id);
    Task<UserRoleResponse> GetByIdAsync(int id);
    Task<IList<UserRoleResponse>> GetByUserAsync(int userId);
    Task<IList<UserRoleResponse>> GetByRoleAsync(int roleId);
    Task<IList<UserRoleResponse>> GetAllAsync();
}
