using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de serviço de domínio para ClientAddress
/// </summary>
public interface IClientAddressDomainService
{
    Task<bool> CreateAsync(ClientAddressEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ClientAddressEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(ClientAddressEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(ClientAddressEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(ClientAddressEntity entity, CancellationToken ct);
}
