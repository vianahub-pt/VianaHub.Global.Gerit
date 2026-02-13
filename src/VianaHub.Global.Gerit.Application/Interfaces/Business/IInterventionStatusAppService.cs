using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionStatus;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionStatus;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

/// <summary>
/// Interface para serviço de aplicação de InterventionStatus
/// </summary>
public interface IInterventionStatusAppService
{
    Task<IEnumerable<InterventionStatusResponse>> GetAllAsync(CancellationToken ct);
    Task<InterventionStatusResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<InterventionStatusResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateInterventionStatusRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateInterventionStatusRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
