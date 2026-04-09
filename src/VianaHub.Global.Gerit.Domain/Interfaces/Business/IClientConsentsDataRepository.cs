using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IClientConsentsDataRepository
{
    Task<ClientConsentsEntity?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<ClientConsentsEntity>> GetByClientIdAsync(int clientId, CancellationToken ct = default);
    Task<IEnumerable<ClientConsentsEntity>> GetByConsentTypeIdAsync(int consentTypeId, CancellationToken ct = default);
    Task<ClientConsentsEntity?> GetByClientAndConsentTypeAsync(int clientId, int consentTypeId, CancellationToken ct = default);
    Task<IEnumerable<ClientConsentsEntity>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<ClientConsentsEntity>> GetActiveAsync(CancellationToken ct = default);
    Task<IEnumerable<ClientConsentsEntity>> GetGrantedAsync(CancellationToken ct = default);
    Task<IEnumerable<ClientConsentsEntity>> GetRevokedAsync(CancellationToken ct = default);
    Task<ListPage<ClientConsentsEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct = default);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsByClientAndConsentTypeAsync(int clientId, int consentTypeId, CancellationToken ct = default);
    Task<bool> AddAsync(ClientConsentsEntity entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(ClientConsentsEntity entity, CancellationToken ct = default);
}
