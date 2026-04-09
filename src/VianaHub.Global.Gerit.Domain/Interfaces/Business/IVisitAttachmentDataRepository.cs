using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IVisitAttachmentDataRepository
{
    Task<VisitAttachmentEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<VisitAttachmentEntity> GetByPublicIdAsync(Guid publicId, CancellationToken ct);
    Task<IEnumerable<VisitAttachmentEntity>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<VisitAttachmentEntity>> GetByVisitIdAsync(int visitId, CancellationToken ct);
    Task<IEnumerable<VisitAttachmentEntity>> GetByCategoryIdAsync(int categoryId, CancellationToken ct);
    Task<VisitAttachmentEntity> GetPrimaryByVisitIdAsync(int visitId, CancellationToken ct);
    Task<ListPage<VisitAttachmentEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByS3KeyAsync(int tenantId, string s3Key, CancellationToken ct);
    Task<bool> HasPrimaryAttachmentAsync(int visitId, CancellationToken ct);
    Task<bool> AddAsync(VisitAttachmentEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(VisitAttachmentEntity entity, CancellationToken ct);
    Task<long> GetTotalSizeByVisitIdAsync(int visitId, CancellationToken ct);
    Task<int> GetCountByVisitIdAsync(int visitId, CancellationToken ct);
}
