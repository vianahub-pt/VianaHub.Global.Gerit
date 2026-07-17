using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de reposit�rio de dados para VisitAddress
/// </summary>
public interface IVisitAddressDataRepository
{
    Task<VisitAddressesEntity?> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<VisitAddressesEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<VisitAddressesEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByVisitAndAddressAsync(int tenantId, int interventionId, string street, string city, string postalCode, CancellationToken ct);
    Task<VisitAddressesEntity> GetPrimaryAddressByVisitAsync(int interventionId, CancellationToken ct);
    Task<bool> AddAsync(VisitAddressesEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(VisitAddressesEntity entity, CancellationToken ct);
}
