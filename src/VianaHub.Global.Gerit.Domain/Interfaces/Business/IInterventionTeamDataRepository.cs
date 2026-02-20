using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Domain.ReadModels;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IInterventionTeamDataRepository
{
    Task<IEnumerable<InterventionTeamEntity>> GetAllAsync(CancellationToken ct);
    Task<InterventionTeamEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPage<InterventionTeamEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int tenantId, int interventionId, int teamId, CancellationToken ct);
    Task<bool> AddAsync(InterventionTeamEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(InterventionTeamEntity entity, CancellationToken ct);
}
