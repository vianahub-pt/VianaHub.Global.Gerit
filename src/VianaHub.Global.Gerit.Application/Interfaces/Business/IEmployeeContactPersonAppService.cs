using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.EmployeeContact;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.EmployeeContact;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

/// <summary>
/// Interface de servi�o de aplica��o para EmployeeContact
/// </summary>
public interface IEmployeeContactPersonAppService
{
    Task<IEnumerable<EmployeeContactPersonResponse>> GetAllAsync(CancellationToken ct);
    Task<EmployeeContactPersonResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<EmployeeContactPersonResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<int> CreateAsync(CreateEmployeeContactPersonRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateEmployeeContactPersonRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
