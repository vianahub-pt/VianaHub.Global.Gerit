using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Billing;

public interface ITenantContactPersonDataRepository
{
    Task<TenantContactPersonsEntity?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<TenantContactPersonsEntity>> GetByTenantIdAsync(int tenantId, CancellationToken ct = default);
    Task<TenantContactPersonsEntity?> GetPrimaryByTenantIdAsync(int tenantId, CancellationToken ct = default);
    Task<IEnumerable<TenantContactPersonsEntity>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<TenantContactPersonsEntity>> GetActiveAsync(CancellationToken ct = default);
    Task<ListPage<TenantContactPersonsEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct = default);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsPrimaryContactAsync(int tenantId, CancellationToken ct = default);
    Task<bool> AddAsync(TenantContactPersonsEntity entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(TenantContactPersonsEntity entity, CancellationToken ct = default);
}
