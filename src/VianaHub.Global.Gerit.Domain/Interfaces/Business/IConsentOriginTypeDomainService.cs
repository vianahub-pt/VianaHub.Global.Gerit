using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IConsentOriginTypeDomainService
{
    Task<ConsentOriginTypeEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<ConsentOriginTypeEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<ConsentOriginTypeEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);

    Task<bool> CreateAsync(ConsentOriginTypeEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ConsentOriginTypeEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(ConsentOriginTypeEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(ConsentOriginTypeEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(ConsentOriginTypeEntity entity, CancellationToken ct);
}
