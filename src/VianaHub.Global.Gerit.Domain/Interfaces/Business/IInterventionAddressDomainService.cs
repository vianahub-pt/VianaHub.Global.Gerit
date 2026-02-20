using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de serviço de domínio para InterventionAddress
/// </summary>
public interface IInterventionAddressDomainService
{
    Task<InterventionAddressEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<InterventionAddressEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<InterventionAddressEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);

    Task<bool> CreateAsync(InterventionAddressEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(InterventionAddressEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(InterventionAddressEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(InterventionAddressEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(InterventionAddressEntity entity, CancellationToken ct);
}
