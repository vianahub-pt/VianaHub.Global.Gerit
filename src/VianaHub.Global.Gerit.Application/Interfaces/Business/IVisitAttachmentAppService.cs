using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitAttachment;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitAttachment;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IVisitAttachmentAppService
{
    Task<VisitAttachmentResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<VisitAttachmentResponse> GetByPublicIdAsync(Guid publicId, CancellationToken ct);
    Task<IEnumerable<VisitAttachmentResponse>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<VisitAttachmentResponse>> GetByVisitIdAsync(int visitId, CancellationToken ct);
    Task<IEnumerable<VisitAttachmentResponse>> GetByCategoryIdAsync(int categoryId, CancellationToken ct);
    Task<VisitAttachmentResponse> GetPrimaryByVisitIdAsync(int visitId, CancellationToken ct);
    Task<ListPageResponse<VisitAttachmentResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateVisitAttachmentRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateVisitAttachmentRequest request, CancellationToken ct);
    Task<bool> SetAsPrimaryAsync(int id, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
