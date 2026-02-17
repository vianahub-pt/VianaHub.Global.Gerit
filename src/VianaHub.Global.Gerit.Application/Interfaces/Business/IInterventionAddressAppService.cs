using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionAddress;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionAddress;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IInterventionAddressAppService
{
    Task<IEnumerable<InterventionAddressResponse>> GetAllAsync(CancellationToken ct);
    Task<InterventionAddressResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<InterventionAddressResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateInterventionAddressRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateInterventionAddressRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
