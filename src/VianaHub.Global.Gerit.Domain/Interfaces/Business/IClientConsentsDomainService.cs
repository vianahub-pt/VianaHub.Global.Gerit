using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IClientConsentsDomainService
{
    Task<bool> CreateAsync(ClientConsentsEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ClientConsentsEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(ClientConsentsEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(ClientConsentsEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(ClientConsentsEntity entity, CancellationToken ct);
}
