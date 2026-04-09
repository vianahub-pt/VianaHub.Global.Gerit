using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de serviço de domínio para VisitAddress
/// </summary>
public interface IVisitAddressDomainService
{
    Task<VisitAddressEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<VisitAddressEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<VisitAddressEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);

    Task<bool> CreateAsync(VisitAddressEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(VisitAddressEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(VisitAddressEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(VisitAddressEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(VisitAddressEntity entity, CancellationToken ct);
}
