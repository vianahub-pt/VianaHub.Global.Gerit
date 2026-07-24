using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Billing;

public interface ITenantContactPersonDomainService
{
    Task<bool> CreateAsync(TenantContactPersonsEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(TenantContactPersonsEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(TenantContactPersonsEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(TenantContactPersonsEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(TenantContactPersonsEntity entity, CancellationToken ct);
}
