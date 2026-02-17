using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de repositório de dados para InterventionAddress
/// </summary>
public interface IInterventionAddressDataRepository
{
    Task<InterventionAddressEntity?> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<InterventionAddressEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<InterventionAddressEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByInterventionAndAddressAsync(int tenantId, int interventionId, string street, string city, string postalCode, CancellationToken ct);
    Task<InterventionAddressEntity> GetPrimaryAddressByInterventionAsync(int interventionId, CancellationToken ct);
    Task<bool> AddAsync(InterventionAddressEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(InterventionAddressEntity entity, CancellationToken ct);
}
