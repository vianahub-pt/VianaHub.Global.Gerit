using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Action;
using VianaHub.Global.Gerit.Application.Dtos.Response.Action;

namespace VianaHub.Global.Gerit.Application.Interfaces;

public interface IActionAppService
{
    Task<IEnumerable<ActionResponse>> GetAllAsync(CancellationToken ct);
    Task<ActionResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<ActionResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateActionRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateActionRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);

}
