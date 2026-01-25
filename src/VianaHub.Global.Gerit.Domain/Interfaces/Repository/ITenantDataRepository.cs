using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Repository;

public interface ITenantDataRepository
{
    Task<TenantEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<TenantEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<TenantEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct);
    Task<bool> AddAsync(TenantEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(TenantEntity entity, CancellationToken ct);
}
