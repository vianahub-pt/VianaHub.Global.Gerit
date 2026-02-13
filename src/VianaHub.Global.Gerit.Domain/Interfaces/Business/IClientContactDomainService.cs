using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de Domain Service para ClientContact
/// </summary>
public interface IClientContactDomainService
{
    Task<bool> CreateAsync(ClientContactEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ClientContactEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(ClientContactEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(ClientContactEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(ClientContactEntity entity, CancellationToken ct);
}
