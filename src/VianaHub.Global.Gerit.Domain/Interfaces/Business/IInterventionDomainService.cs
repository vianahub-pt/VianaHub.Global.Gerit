using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IInterventionDomainService
{
    Task<InterventionEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<InterventionEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<InterventionEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> CreateAsync(InterventionEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(InterventionEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(InterventionEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(InterventionEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(InterventionEntity entity, CancellationToken ct);
}
