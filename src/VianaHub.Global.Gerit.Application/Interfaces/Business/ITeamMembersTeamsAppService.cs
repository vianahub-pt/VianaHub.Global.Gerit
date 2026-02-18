using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.TeamMembersTeams;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.TeamMembersTeams;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface ITeamMembersTeamsAppService
{
    Task<IEnumerable<TeamMembersTeamResponse>> GetAllAsync(CancellationToken ct);
    Task<TeamMembersTeamResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<TeamMembersTeamResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateTeamMembersTeamRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateTeamMembersTeamRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
