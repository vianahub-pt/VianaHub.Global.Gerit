using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IClientCompanyDataRepository
{
    Task<IEnumerable<ClientCompanyEntity>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<ClientCompanyEntity>> GetActiveAsync(CancellationToken ct);
    Task<ClientCompanyEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<ClientCompanyEntity> GetByClientIdAsync(int clientId, CancellationToken ct);
    Task<ClientCompanyEntity> GetByTaxNumberAsync(string taxNumber, CancellationToken ct);
    Task<ListPage<ClientCompanyEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByClientIdAsync(int clientId, CancellationToken ct);
    Task<bool> ExistsByTaxNumberAsync(string taxNumber, CancellationToken ct);

    Task<bool> AddAsync(ClientCompanyEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ClientCompanyEntity entity, CancellationToken ct);
}
