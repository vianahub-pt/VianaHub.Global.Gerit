using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitContact;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitContact;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

/// <summary>
/// Interface do servi�o de aplica��o para VisitContact
/// </summary>
public interface IVisitContactAppService
{
    Task<IEnumerable<VisitContactResponse>> GetAllAsync(CancellationToken ct);
    Task<VisitContactResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<VisitContactDetailResponse> GetByIdDetailAsync(int id, CancellationToken ct);
    Task<ListPageResponse<VisitContactResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<int> CreateAsync(CreateVisitContactRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateVisitContactRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
