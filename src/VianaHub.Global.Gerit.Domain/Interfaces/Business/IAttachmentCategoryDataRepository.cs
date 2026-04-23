using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IAttachmentCategoryDataRepository
{
    Task<AttachmentCategoryEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<AttachmentCategoryEntity>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<AttachmentCategoryEntity>> GetActiveAsync(CancellationToken ct);
    Task<ListPage<AttachmentCategoryEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByNameAsync(int tenantId, string name, CancellationToken ct);
    Task<bool> ExistsByNameAsync(int tenantId, string name, int excludeId, CancellationToken ct);
    Task<bool> AddAsync(AttachmentCategoryEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(AttachmentCategoryEntity entity, CancellationToken ct);
}
