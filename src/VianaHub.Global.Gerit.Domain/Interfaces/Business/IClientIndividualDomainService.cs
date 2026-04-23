using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IClientIndividualDomainService
{
    Task<bool> CreateAsync(ClientIndividualEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ClientIndividualEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(ClientIndividualEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(ClientIndividualEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(ClientIndividualEntity entity, CancellationToken ct);
}
