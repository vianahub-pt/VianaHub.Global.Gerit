using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.TeamMemberContact;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.TeamMemberContact;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

/// <summary>
/// Interface de serviço de aplicação para TeamMemberContact
/// </summary>
public interface ITeamMemberContactAppService
{
    Task<IEnumerable<TeamMemberContactResponse>> GetAllAsync(CancellationToken ct);
    Task<TeamMemberContactResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<TeamMemberContactResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateTeamMemberContactRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateTeamMemberContactRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
