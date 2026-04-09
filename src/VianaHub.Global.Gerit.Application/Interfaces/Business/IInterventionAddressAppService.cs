using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitAddress;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitAddress;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IVisitAddressAppService
{
    Task<IEnumerable<VisitAddressResponse>> GetAllAsync(CancellationToken ct);
    Task<VisitAddressResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<VisitAddressResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateVisitAddressRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateVisitAddressRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
