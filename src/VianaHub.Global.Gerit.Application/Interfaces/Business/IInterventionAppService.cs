using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Intervention;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.Intervention;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IInterventionAppService
{
    Task<IEnumerable<InterventionResponse>> GetAllAsync(CancellationToken ct);
    Task<InterventionResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<InterventionResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateInterventionRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateInterventionRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
