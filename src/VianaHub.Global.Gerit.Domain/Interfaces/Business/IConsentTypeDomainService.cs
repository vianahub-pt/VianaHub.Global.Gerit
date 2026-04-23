using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IConsentTypeDomainService
{
    Task<ConsentTypeEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<ConsentTypeEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<ConsentTypeEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);

    Task<bool> CreateAsync(ConsentTypeEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ConsentTypeEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(ConsentTypeEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(ConsentTypeEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(ConsentTypeEntity entity, CancellationToken ct);
}
