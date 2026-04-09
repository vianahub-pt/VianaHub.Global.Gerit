using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitTeamEquipments;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitTeamEquipments;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IVisitTeamEquipmentsAppService
{
    Task<IEnumerable<VisitTeamEquipmentResponse>> GetAllAsync(CancellationToken ct);
    Task<VisitTeamEquipmentResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<VisitTeamEquipmentResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateVisitTeamEquipmentRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateVisitTeamEquipmentRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
