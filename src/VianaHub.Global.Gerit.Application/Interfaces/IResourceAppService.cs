using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Resource;
using VianaHub.Global.Gerit.Application.Dtos.Response.Resource;

namespace VianaHub.Global.Gerit.Application.Interfaces;

public interface IResourceAppService
{
    Task<IEnumerable<ResourceResponse>> GetAllAsync(CancellationToken ct);
    Task<ResourceResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<ResourceResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateResourceRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateResourceRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
