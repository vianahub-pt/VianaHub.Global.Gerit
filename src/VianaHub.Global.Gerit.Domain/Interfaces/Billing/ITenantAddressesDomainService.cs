using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Billing;

public interface ITenantAddressesDomainService
{
    Task<bool> CreateAsync(TenantAddressesEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(TenantAddressesEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(TenantAddressesEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(TenantAddressesEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(TenantAddressesEntity entity, CancellationToken ct);
}
