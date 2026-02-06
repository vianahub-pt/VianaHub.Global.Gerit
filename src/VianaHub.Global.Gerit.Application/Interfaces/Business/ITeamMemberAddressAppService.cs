using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.TeamMemberAddress;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.TeamMemberAddress;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

/// <summary>
/// Interface de serviço de aplicação para TeamMemberAddress
/// </summary>
public interface ITeamMemberAddressAppService
{
    Task<IEnumerable<TeamMemberAddressResponse>> GetAllAsync(CancellationToken ct);
    Task<TeamMemberAddressResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<TeamMemberAddressResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateTeamMemberAddressRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateTeamMemberAddressRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
