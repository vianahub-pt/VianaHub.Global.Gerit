using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Billing;

public interface ITenantContactDomainService
{
    Task<bool> CreateAsync(TenantContactEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(TenantContactEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(TenantContactEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(TenantContactEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(TenantContactEntity entity, CancellationToken ct);
}
