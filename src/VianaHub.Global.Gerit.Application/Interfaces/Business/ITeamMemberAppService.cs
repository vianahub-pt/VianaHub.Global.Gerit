using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.TeamMember;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.TeamMember;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface ITeamMemberAppService
{
    Task<IEnumerable<TeamMemberResponse>> GetAllAsync(CancellationToken ct);
    Task<TeamMemberResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<TeamMemberResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateTeamMemberRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateTeamMemberRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
