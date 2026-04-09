using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitTeamEmployee;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitTeamEmployee;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IVisitTeamEmployeeAppService
{
    Task<VisitTeamEmployeeResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<VisitTeamEmployeeResponse>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<VisitTeamEmployeeResponse>> GetByVisitTeamIdAsync(int visitTeamId, CancellationToken ct);
    Task<IEnumerable<VisitTeamEmployeeResponse>> GetByEmployeeIdAsync(int employeeId, CancellationToken ct);
    Task<IEnumerable<VisitTeamEmployeeResponse>> GetActiveByVisitTeamIdAsync(int visitTeamId, CancellationToken ct);
    Task<ListPageResponse<VisitTeamEmployeeResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateVisitTeamEmployeeRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateVisitTeamEmployeeRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
