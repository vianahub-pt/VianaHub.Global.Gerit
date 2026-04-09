using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IClientIndividualFiscalDataDataRepository
{
    Task<IEnumerable<ClientIndividualFiscalDataEntity>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<ClientIndividualFiscalDataEntity>> GetActiveAsync(CancellationToken ct);
    Task<ClientIndividualFiscalDataEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<ClientIndividualFiscalDataEntity> GetByClientIndividualIdAsync(int clientIndividualId, CancellationToken ct);
    Task<ClientIndividualFiscalDataEntity> GetByTaxNumberAsync(string taxNumber, CancellationToken ct);
    Task<ListPage<ClientIndividualFiscalDataEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByClientIndividualIdAsync(int clientIndividualId, CancellationToken ct);
    Task<bool> ExistsByTaxNumberAsync(string taxNumber, int? excludeId = null, CancellationToken ct = default);

    Task<bool> AddAsync(ClientIndividualFiscalDataEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ClientIndividualFiscalDataEntity entity, CancellationToken ct);
}
