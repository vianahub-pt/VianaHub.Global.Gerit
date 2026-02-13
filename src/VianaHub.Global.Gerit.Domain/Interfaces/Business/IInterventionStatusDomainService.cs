using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface para serviço de domínio de InterventionStatus
/// </summary>
public interface IInterventionStatusDomainService
{
    Task<bool> CreateAsync(InterventionStatusEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(InterventionStatusEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(InterventionStatusEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(InterventionStatusEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(InterventionStatusEntity entity, CancellationToken ct);
}
