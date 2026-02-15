using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IStatusTypeDataRepository
{
    Task<IEnumerable<StatusTypeEntity>> GetAllAsync(CancellationToken ct);
    Task<StatusTypeEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPage<StatusTypeEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByNameAsync(int tenantId, string name, CancellationToken ct);
    Task<bool> ExistsByNameForUpdateAsync(int tenantId, string name, int excludeId, CancellationToken ct);
    Task<bool> AddAsync(StatusTypeEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(StatusTypeEntity entity, CancellationToken ct);
}
