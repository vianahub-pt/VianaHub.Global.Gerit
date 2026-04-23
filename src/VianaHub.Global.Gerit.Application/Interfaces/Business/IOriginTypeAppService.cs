using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.OriginType;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.OriginType;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IOriginTypeAppService
{
    Task<IEnumerable<OriginTypeResponse>> GetAllAsync(CancellationToken ct);
    Task<OriginTypeResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<OriginTypeResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateOriginTypeRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateOriginTypeRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
