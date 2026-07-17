using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IVisitAttachmentDomainService
{
    Task<bool> CreateAsync(VisitAttachmentsEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(VisitAttachmentsEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(VisitAttachmentsEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(VisitAttachmentsEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(VisitAttachmentsEntity entity, CancellationToken ct);
}
