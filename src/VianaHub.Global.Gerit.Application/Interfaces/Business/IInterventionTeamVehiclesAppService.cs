using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionTeamVehicles;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionTeamVehicles;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IInterventionTeamVehiclesAppService
{
    Task<IEnumerable<InterventionTeamVehicleResponse>> GetAllAsync(CancellationToken ct);
    Task<InterventionTeamVehicleResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<InterventionTeamVehicleResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateInterventionTeamVehicleRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateInterventionTeamVehicleRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
