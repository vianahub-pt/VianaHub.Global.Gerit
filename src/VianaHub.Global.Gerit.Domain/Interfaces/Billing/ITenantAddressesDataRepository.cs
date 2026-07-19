using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Billing;

public interface ITenantAddressesDataRepository
{
    Task<TenantAddressesEntity?> GetByIdAsync(int tenantId, int id, CancellationToken ct = default);
    Task<IEnumerable<TenantAddressesEntity>> GetByTenantIdAsync(int tenantId, CancellationToken ct = default);
    Task<ListPage<TenantAddressesEntity>> GetPagedAsync(int tenantId, PagedFilter filter, CancellationToken ct = default);
    Task<bool> ExistsByIdAsync(int tenantId, int id, CancellationToken ct = default);
    Task<bool> ExistsPrimaryByTenantAsync(int tenantId, CancellationToken ct = default);
    Task<bool> AddAsync(TenantAddressesEntity entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(TenantAddressesEntity entity, CancellationToken ct = default);
}
