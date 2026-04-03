using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientType;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientType;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IClientTypeAppService
{
    Task<IEnumerable<ClientTypeResponse>> GetAllAsync(CancellationToken ct);
    Task<ClientTypeResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<ClientTypeResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateClientTypeRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateClientTypeRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
