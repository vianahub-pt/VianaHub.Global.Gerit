using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Billing;

public interface ITenantFiscalDataDataRepository
{
    Task<TenantFiscalDataEntity?> GetByIdAsync(int tenantId, int id, CancellationToken ct = default);
    Task<IEnumerable<TenantFiscalDataEntity>> GetByTenantIdAsync(int tenantId, CancellationToken ct = default);
    Task<ListPage<TenantFiscalDataEntity>> GetPagedAsync(int tenantId, PagedFilter filter, CancellationToken ct = default);
    Task<bool> ExistsByIdAsync(int tenantId, int id, CancellationToken ct = default);
    Task<bool> ExistsActiveByTenantAsync(int tenantId, CancellationToken ct = default);
    Task<bool> ExistsByTaxNumberAsync(int tenantId, string fiscalCountry, string taxNumber, CancellationToken ct = default);
    Task<bool> AddAsync(TenantFiscalDataEntity entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(TenantFiscalDataEntity entity, CancellationToken ct = default);
}
