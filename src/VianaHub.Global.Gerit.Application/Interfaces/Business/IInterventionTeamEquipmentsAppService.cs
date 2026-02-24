using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionTeamEquipments;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionTeamEquipments;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IInterventionTeamEquipmentsAppService
{
    Task<IEnumerable<InterventionTeamEquipmentResponse>> GetAllAsync(CancellationToken ct);
    Task<InterventionTeamEquipmentResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<InterventionTeamEquipmentResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateInterventionTeamEquipmentRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateInterventionTeamEquipmentRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
