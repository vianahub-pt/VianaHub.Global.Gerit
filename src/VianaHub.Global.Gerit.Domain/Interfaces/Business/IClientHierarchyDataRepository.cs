using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IClientHierarchyDataRepository
{
    Task<IEnumerable<ClientHierarchyEntity>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<ClientHierarchyEntity>> GetActiveAsync(CancellationToken ct);
    Task<ClientHierarchyEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<ClientHierarchyEntity>> GetByParentClientIdAsync(int parentClientId, CancellationToken ct);
    Task<IEnumerable<ClientHierarchyEntity>> GetByChildClientIdAsync(int childClientId, CancellationToken ct);
    Task<ListPage<ClientHierarchyEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsRelationshipAsync(int parentClientId, int childClientId, CancellationToken ct);

    Task<bool> AddAsync(ClientHierarchyEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ClientHierarchyEntity entity, CancellationToken ct);
}
