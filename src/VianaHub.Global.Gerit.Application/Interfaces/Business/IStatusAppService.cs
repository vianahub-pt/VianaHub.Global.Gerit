using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Status;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.Status;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

/// <summary>
/// Interface para serviço de aplicação de Status
/// </summary>
public interface IStatusAppService
{
    Task<IEnumerable<StatusResponse>> GetAllAsync(CancellationToken ct);
    Task<StatusResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<StatusResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateStatusRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateStatusRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
