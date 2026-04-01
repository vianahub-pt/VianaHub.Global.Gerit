using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de Domain Service para ClientContact
/// </summary>
public interface IClientContactDomainService
{
    Task<ClientContactEntity> GetByIdAsync(int clientId, int id, CancellationToken ct);
    Task<IEnumerable<ClientContactEntity>> GetAllAsync(int clientId, CancellationToken ct);
    Task<ListPage<ClientContactEntity>> GetPagedAsync(int clientId, PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int clientId, int id, CancellationToken ct);

    Task<bool> CreateAsync(ClientContactEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ClientContactEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(ClientContactEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(ClientContactEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(ClientContactEntity entity, CancellationToken ct);
}
