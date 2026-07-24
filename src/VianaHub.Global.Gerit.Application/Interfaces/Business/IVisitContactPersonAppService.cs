using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitContactPersons;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitContactPersons;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

/// <summary>
/// Interface do servi�o de aplica��o para VisitContact
/// </summary>
public interface IVisitContactPersonAppService
{
    Task<IEnumerable<VisitContactPersonResponse>> GetAllAsync(CancellationToken ct);
    Task<VisitContactPersonResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<VisitContactPersonResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<int> CreateAsync(CreateVisitContactPersonRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateVisitContactPersonRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
