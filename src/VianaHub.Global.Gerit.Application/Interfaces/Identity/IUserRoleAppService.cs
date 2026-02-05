using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Request.Identity.UserRole;
using VianaHub.Global.Gerit.Application.Dtos.Response.Identity.UserRole;

namespace VianaHub.Global.Gerit.Application.Interfaces.Identity;

public interface IUserRoleAppService
{
    Task<UserRoleResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<IList<UserRoleResponse>> GetByUserAsync(int userId, CancellationToken ct);
    Task<IList<UserRoleResponse>> GetByRoleAsync(int roleId, CancellationToken ct);
    Task<IList<UserRoleResponse>> GetAllAsync(CancellationToken ct);
    Task<UserRoleResponse> CreateAsync(CreateUserRoleRequest request, CancellationToken ct);
    Task<UserRoleResponse> UpdateAsync(UpdateUserRoleRequest request, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
