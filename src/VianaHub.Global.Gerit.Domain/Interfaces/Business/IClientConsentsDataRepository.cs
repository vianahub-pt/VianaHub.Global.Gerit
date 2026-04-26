using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IClientConsentsDataRepository
{
    Task<IEnumerable<ClientConsentsEntity>> GetAllAsync(int clientId, CancellationToken ct = default);
    Task<ClientConsentsEntity> GetByIdAsync(int clientId, int id, CancellationToken ct = default);
    Task<ListPage<ClientConsentsEntity>> GetPagedAsync(int clientId, PagedFilter filter, CancellationToken ct = default);
    Task<bool> ExistsByIdAsync(int clientId, int id, CancellationToken ct = default);
    Task<bool> ExistsByClientAndConsentTypeAsync(int clientId, int consentTypeId, CancellationToken ct = default);
    Task<bool> AddAsync(ClientConsentsEntity entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(ClientConsentsEntity entity, CancellationToken ct = default);
}
