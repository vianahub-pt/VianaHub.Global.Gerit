using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de serviço de domínio para Client
/// </summary>
public interface IClientDomainService
{
    Task<bool> CreateAsync(ClientEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ClientEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(ClientEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(ClientEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(ClientEntity entity, CancellationToken ct);
}
