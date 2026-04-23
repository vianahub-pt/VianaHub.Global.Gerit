using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de repositório de dados para VisitAddress
/// </summary>
public interface IVisitAddressDataRepository
{
    Task<VisitAddressEntity?> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<VisitAddressEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<VisitAddressEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByVisitAndAddressAsync(int tenantId, int interventionId, string street, string city, string postalCode, CancellationToken ct);
    Task<VisitAddressEntity> GetPrimaryAddressByVisitAsync(int interventionId, CancellationToken ct);
    Task<bool> AddAsync(VisitAddressEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(VisitAddressEntity entity, CancellationToken ct);
}
