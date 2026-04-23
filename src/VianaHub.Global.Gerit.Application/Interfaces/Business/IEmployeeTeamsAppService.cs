using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.EmployeeTeams;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.EmployeeTeams;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IEmployeeTeamsAppService
{
    Task<IEnumerable<EmployeeTeamResponse>> GetAllAsync(CancellationToken ct);
    Task<EmployeeTeamResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<EmployeeTeamResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateEmployeeTeamRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateEmployeeTeamRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
