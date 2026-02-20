using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IInterventionTeamDomainService
{
    Task<InterventionTeamEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<InterventionTeamEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<InterventionTeamEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int tenantId, int interventionId, int teamId, CancellationToken ct);

    Task<bool> CreateAsync(InterventionTeamEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(InterventionTeamEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(InterventionTeamEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(InterventionTeamEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(InterventionTeamEntity entity, CancellationToken ct);
}
