using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IAcquisitionSourceTypeDataRepository
{
    Task<IEnumerable<AcquisitionSourceTypeEntity>> GetAllAsync(CancellationToken ct);
    Task<AcquisitionSourceTypeEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPage<AcquisitionSourceTypeEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct);

    Task<bool> AddAsync(AcquisitionSourceTypeEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(AcquisitionSourceTypeEntity entity, CancellationToken ct);
}
