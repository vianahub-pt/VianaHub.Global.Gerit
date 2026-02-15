using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface para serviço de domínio de Status
/// </summary>
public interface IStatusDomainService
{
    Task<bool> CreateAsync(StatusEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(StatusEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(StatusEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(StatusEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(StatusEntity entity, CancellationToken ct);
}
