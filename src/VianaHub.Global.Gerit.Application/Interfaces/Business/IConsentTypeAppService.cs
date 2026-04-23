using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ConsentType;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ConsentType;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IConsentTypeAppService
{
    Task<IEnumerable<ConsentTypeResponse>> GetAllAsync(CancellationToken ct);
    Task<ConsentTypeResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<ConsentTypeResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateConsentTypeRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateConsentTypeRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
