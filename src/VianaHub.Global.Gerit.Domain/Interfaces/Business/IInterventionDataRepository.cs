using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de Data Repository para Intervention
/// </summary>
public interface IInterventionDataRepository
{
    Task<InterventionEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<InterventionEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<InterventionEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByTenantAndTitleAsync(int tenantId, string title, int? excludeId, CancellationToken ct);
    Task<bool> AddAsync(InterventionEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(InterventionEntity entity, CancellationToken ct);
}
