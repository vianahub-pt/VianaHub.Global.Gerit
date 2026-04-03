using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IClientTypeDomainService
{
    Task<ClientTypeEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<ClientTypeEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<ClientTypeEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);

    Task<bool> CreateAsync(ClientTypeEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ClientTypeEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(ClientTypeEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(ClientTypeEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(ClientTypeEntity entity, CancellationToken ct);
}
