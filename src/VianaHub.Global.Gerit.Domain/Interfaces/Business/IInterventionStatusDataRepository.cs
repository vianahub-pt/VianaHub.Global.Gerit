using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Domain.ReadModels;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface para repositório de dados de InterventionStatus
/// </summary>
public interface IInterventionStatusDataRepository
{
    Task<InterventionStatusEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<InterventionStatusEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<InterventionStatusEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByNameAsync(int tenantId, string name, CancellationToken ct);
    Task<bool> ExistsByNameForUpdateAsync(int tenantId, string name, int id, CancellationToken ct);
    Task<bool> AddAsync(InterventionStatusEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(InterventionStatusEntity entity, CancellationToken ct);
}
