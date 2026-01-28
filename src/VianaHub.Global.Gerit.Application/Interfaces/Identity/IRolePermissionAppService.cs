using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VianaHub.Global.Gerit.Application.Dtos.Request.Identity.RolePermission;
using VianaHub.Global.Gerit.Application.Dtos.Response.Identity.RolePermission;

namespace VianaHub.Global.Gerit.Application.Interfaces.Identity;

public interface IRolePermissionAppService
{
    Task<RolePermissionResponse> CreateAsync(CreateRolePermissionRequest request, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
    Task<RolePermissionResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<IList<RolePermissionResponse>> GetByRoleAsync(int roleId, CancellationToken ct);
    Task<IList<RolePermissionResponse>> GetByResourceAsync(int resourceId, CancellationToken ct);
    Task<IList<RolePermissionResponse>> GetAllAsync(CancellationToken ct);
}
