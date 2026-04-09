using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IClientCompanyFiscalDataDataRepository
{
    Task<IEnumerable<ClientCompanyFiscalDataEntity>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<ClientCompanyFiscalDataEntity>> GetActiveAsync(CancellationToken ct);
    Task<ClientCompanyFiscalDataEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<ClientCompanyFiscalDataEntity> GetByClientCompanyIdAsync(int clientCompanyId, CancellationToken ct);
    Task<ClientCompanyFiscalDataEntity> GetByTaxNumberAsync(string taxNumber, CancellationToken ct);
    Task<ListPage<ClientCompanyFiscalDataEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByClientCompanyIdAsync(int clientCompanyId, CancellationToken ct);
    Task<bool> ExistsByTaxNumberAsync(string taxNumber, int? excludeId = null, CancellationToken ct = default);

    Task<bool> AddAsync(ClientCompanyFiscalDataEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ClientCompanyFiscalDataEntity entity, CancellationToken ct);
}
