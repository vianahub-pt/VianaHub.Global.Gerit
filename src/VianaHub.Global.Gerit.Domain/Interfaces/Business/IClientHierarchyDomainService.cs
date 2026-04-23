using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IClientHierarchyDomainService
{
    Task<bool> CreateAsync(ClientHierarchyEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ClientHierarchyEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(ClientHierarchyEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(ClientHierarchyEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(ClientHierarchyEntity entity, CancellationToken ct);
}
