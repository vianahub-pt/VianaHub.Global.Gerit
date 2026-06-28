using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ConsentOriginType;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ConsentOriginType;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IConsentOriginTypeAppService
{
    Task<IEnumerable<ConsentOriginTypeResponse>> GetAllAsync(CancellationToken ct);
    Task<ConsentOriginTypeResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<ConsentOriginTypeResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<int> CreateAsync(CreateConsentOriginTypeRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateConsentOriginTypeRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
