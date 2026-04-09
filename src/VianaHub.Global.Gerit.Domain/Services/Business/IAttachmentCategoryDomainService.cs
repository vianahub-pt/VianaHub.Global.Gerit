using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IAttachmentCategoryDomainService
{
    Task<bool> CreateAsync(AttachmentCategoryEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(AttachmentCategoryEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(AttachmentCategoryEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(AttachmentCategoryEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(AttachmentCategoryEntity entity, CancellationToken ct);
}
