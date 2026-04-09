using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IOriginTypeDataRepository
{
    Task<OriginTypeEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<OriginTypeEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<OriginTypeEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByNameAsync(int tenantId, string name, CancellationToken ct);
    Task<bool> AddAsync(OriginTypeEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(OriginTypeEntity entity, CancellationToken ct);
}
