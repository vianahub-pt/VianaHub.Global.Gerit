using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.EmployeeContact;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.EmployeeContact;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

/// <summary>
/// Interface de servi�o de aplica��o para EmployeeContact
/// </summary>
public interface IEmployeeContactAppService
{
    Task<IEnumerable<EmployeeContactResponse>> GetAllAsync(CancellationToken ct);
    Task<EmployeeContactResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<EmployeeContactResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateEmployeeContactRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateEmployeeContactRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
