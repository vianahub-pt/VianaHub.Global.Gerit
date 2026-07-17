using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Billing;

public interface ITenantFiscalDataDomainService
{
    Task<bool> CreateAsync(TenantFiscalDataEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(TenantFiscalDataEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(TenantFiscalDataEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(TenantFiscalDataEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(TenantFiscalDataEntity entity, CancellationToken ct);
}
