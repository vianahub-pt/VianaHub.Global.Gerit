using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IAddressTypeDataRepository
{
    Task<IEnumerable<AddressTypeEntity>> GetAllAsync(CancellationToken ct);
    Task<AddressTypeEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPage<AddressTypeEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByNameAsync(int tenantId, string name, CancellationToken ct);

    Task<bool> AddAsync(AddressTypeEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(AddressTypeEntity entity, CancellationToken ct);
}
