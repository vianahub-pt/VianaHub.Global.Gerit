using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Function;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.Function;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IVisitTeamFunctionAppService
{
    Task<IEnumerable<VisitTeamFunctionResponse>> GetAllAsync(CancellationToken ct);
    Task<VisitTeamFunctionDetailResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<VisitTeamFunctionResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<int> CreateAsync(CreateVisitTeamFunctionRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateVisitTeamFunctionRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
