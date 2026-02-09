using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IAddressTypeDomainService
{
    Task<bool> CreateAsync(AddressTypeEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(AddressTypeEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(AddressTypeEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(AddressTypeEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(AddressTypeEntity entity, CancellationToken ct);
}
