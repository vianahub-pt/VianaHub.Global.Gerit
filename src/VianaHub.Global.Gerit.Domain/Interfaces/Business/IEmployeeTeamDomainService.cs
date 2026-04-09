using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IEmployeeTeamDomainService
{
    Task<EmployeeTeamEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<EmployeeTeamEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<EmployeeTeamEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);

    Task<bool> CreateAsync(EmployeeTeamEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(EmployeeTeamEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(EmployeeTeamEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(EmployeeTeamEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(EmployeeTeamEntity entity, CancellationToken ct);
}
