using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de Data Repository para ClientContact
/// </summary>
public interface IClientContactDataRepository
{
    Task<ClientContactEntity> GetByIdAsync(int clientId, int id, CancellationToken ct);
    Task<IEnumerable<ClientContactEntity>> GetAllAsync(int clientId, CancellationToken ct);
    Task<ListPage<ClientContactEntity>> GetPagedAsync(int clientId, PagedFilter filter, CancellationToken ct);
    Task<bool> ExistsByClientAndEmailAsync(int clientId, string name, string email, CancellationToken ct);
    Task<bool> AddAsync(ClientContactEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ClientContactEntity entity, CancellationToken ct);
}
