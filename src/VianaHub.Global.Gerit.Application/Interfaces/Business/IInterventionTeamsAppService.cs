using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionTeams;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionTeams;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IInterventionTeamsAppService
{
    Task<IEnumerable<InterventionTeamResponse>> GetAllAsync(CancellationToken ct);
    Task<InterventionTeamResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<InterventionTeamResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateInterventionTeamRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateInterventionTeamRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
