using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface do serviço de domínio para InterventionContact
/// </summary>
public interface IInterventionContactDomainService
{
    Task<bool> CreateAsync(InterventionContactEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(InterventionContactEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(InterventionContactEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(InterventionContactEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(InterventionContactEntity entity, CancellationToken ct);
}
