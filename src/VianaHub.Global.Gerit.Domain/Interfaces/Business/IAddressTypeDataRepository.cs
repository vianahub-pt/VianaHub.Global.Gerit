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
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct);
    Task<bool> ExistsByNameAsync(string name, int excludeId, CancellationToken ct);
    Task AddAsync(AddressTypeEntity entity, CancellationToken ct);
    Task UpdateAsync(AddressTypeEntity entity, CancellationToken ct);
}
