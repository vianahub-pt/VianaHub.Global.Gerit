using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Function;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.Function;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IFunctionAppService
{
    Task<IEnumerable<FunctionResponse>> GetAllAsync(CancellationToken ct);
    Task<FunctionResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<FunctionResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateFunctionRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateFunctionRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
