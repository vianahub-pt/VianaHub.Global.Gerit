using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IStatusTypeDomainService
{
    Task<bool> CreateAsync(StatusTypeEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(StatusTypeEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(StatusTypeEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(StatusTypeEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(StatusTypeEntity entity, CancellationToken ct);
}
