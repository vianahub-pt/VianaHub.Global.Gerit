using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.StatusType;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.StatusType;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IStatusTypeAppService
{
    Task<IEnumerable<StatusTypeResponse>> GetAllAsync(CancellationToken ct);
    Task<StatusTypeResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<StatusTypeResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateStatusTypeRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateStatusTypeRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
