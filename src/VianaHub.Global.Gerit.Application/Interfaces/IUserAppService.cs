using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.User;
using VianaHub.Global.Gerit.Application.Dtos.Response.User;
using Microsoft.AspNetCore.Http;

namespace VianaHub.Global.Gerit.Application.Interfaces;

public interface IUserAppService
{
    Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken ct);
    Task<UserResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<UserResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateUserRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateUserRequest request, CancellationToken ct);
    Task<bool> UpdatePasswordAsync(int id, UpdatePasswordRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
