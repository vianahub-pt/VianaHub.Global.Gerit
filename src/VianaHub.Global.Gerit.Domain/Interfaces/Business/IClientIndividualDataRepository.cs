using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IClientIndividualDataRepository
{
    Task<ClientIndividualEntity?> GetByIdAsync(int tenantId, int id, CancellationToken ct = default);
    Task<ClientIndividualEntity?> GetByClientIdAsync(int tenantId, int clientId, CancellationToken ct = default);
    Task<ClientIndividualEntity?> GetByDocumentAsync(int tenantId, string documentType, string documentNumber, CancellationToken ct = default);
    Task<ClientIndividualEntity?> GetByEmailAsync(int tenantId, string email, CancellationToken ct = default);
    Task<IEnumerable<ClientIndividualEntity>> GetAllAsync(int tenantId, CancellationToken ct = default);
    Task<IEnumerable<ClientIndividualEntity>> GetActiveAsync(int tenantId, CancellationToken ct = default);
    Task<ListPage<ClientIndividualEntity>> GetPagedAsync(int tenantId, PagedFilter filter, CancellationToken ct = default);
    Task<bool> ExistsByClientIdAsync(int tenantId, int clientId, CancellationToken ct = default);
    Task<bool> ExistsByDocumentAsync(int tenantId, string documentType, string documentNumber, CancellationToken ct = default);
    Task<bool> ExistsByDocumentAsync(int tenantId, string documentType, string documentNumber, int excludeId, CancellationToken ct = default);
    Task<bool> AddAsync(ClientIndividualEntity entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(ClientIndividualEntity entity, CancellationToken ct = default);
}
