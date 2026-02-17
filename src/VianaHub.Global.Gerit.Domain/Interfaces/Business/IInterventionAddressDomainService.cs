using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de serviço de domínio para InterventionAddress
/// </summary>
public interface IInterventionAddressDomainService
{
    Task<bool> CreateAsync(InterventionAddressEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(InterventionAddressEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(InterventionAddressEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(InterventionAddressEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(InterventionAddressEntity entity, CancellationToken ct);
}
