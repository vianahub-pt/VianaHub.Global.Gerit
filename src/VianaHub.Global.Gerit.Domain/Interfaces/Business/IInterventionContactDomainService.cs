using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface do serviço de domínio para InterventionContact
/// </summary>
public interface IInterventionContactDomainService
{
    Task<InterventionContactEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<InterventionContactEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<InterventionContactEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);

    Task<bool> CreateAsync(InterventionContactEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(InterventionContactEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(InterventionContactEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(InterventionContactEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(InterventionContactEntity entity, CancellationToken ct);
}
