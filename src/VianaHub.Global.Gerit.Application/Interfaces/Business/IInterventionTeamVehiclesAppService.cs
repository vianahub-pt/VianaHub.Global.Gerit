using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitTeamVehicles;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitTeamVehicle;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IVisitTeamVehiclesAppService
{
    Task<IEnumerable<VisitTeamVehicleResponse>> GetAllAsync(CancellationToken ct);
    Task<VisitTeamVehicleResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<VisitTeamVehicleResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<int> CreateAsync(CreateVisitTeamVehicleRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateVisitTeamVehicleRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
