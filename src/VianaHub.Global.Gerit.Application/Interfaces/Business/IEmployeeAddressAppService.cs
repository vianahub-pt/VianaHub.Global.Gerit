using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.EmployeeAddress;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.EmployeeAddress;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

/// <summary>
/// Interface de servi�o de aplica��o para EmployeeAddress
/// </summary>
public interface IEmployeeAddressAppService
{
    Task<IEnumerable<EmployeeAddressResponse>> GetAllAsync(CancellationToken ct);
    Task<EmployeeAddressResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<EmployeeAddressResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateEmployeeAddressRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateEmployeeAddressRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
