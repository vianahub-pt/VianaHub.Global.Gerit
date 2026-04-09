using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Billing;

public interface ITenantContactDataRepository
{
    Task<TenantContactEntity?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<TenantContactEntity>> GetByTenantIdAsync(int tenantId, CancellationToken ct = default);
    Task<TenantContactEntity?> GetPrimaryByTenantIdAsync(int tenantId, CancellationToken ct = default);
    Task<IEnumerable<TenantContactEntity>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<TenantContactEntity>> GetActiveAsync(CancellationToken ct = default);
    Task<ListPage<TenantContactEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct = default);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsPrimaryContactAsync(int tenantId, CancellationToken ct = default);
    Task<bool> AddAsync(TenantContactEntity entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(TenantContactEntity entity, CancellationToken ct = default);
}
