using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.AttachmentCategory;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.AttachmentCategory;
using VianaHub.Global.Gerit.Domain.ReadModels;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IAttachmentCategoryAppService
{
    Task<AttachmentCategoryResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<AttachmentCategoryResponse>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<AttachmentCategoryResponse>> GetActiveAsync(CancellationToken ct);
    Task<ListPageResponse<AttachmentCategoryResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateAttachmentCategoryRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateAttachmentCategoryRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
