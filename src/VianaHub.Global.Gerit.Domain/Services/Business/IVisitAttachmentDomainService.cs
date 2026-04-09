using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IVisitAttachmentDomainService
{
    Task<bool> CreateAsync(VisitAttachmentEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(VisitAttachmentEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(VisitAttachmentEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(VisitAttachmentEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(VisitAttachmentEntity entity, CancellationToken ct);
}
