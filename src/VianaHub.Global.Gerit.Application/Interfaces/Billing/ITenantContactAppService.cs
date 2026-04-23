using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Billing.TenantContact;
using VianaHub.Global.Gerit.Application.Dtos.Response.Billing.TenantContact;
using VianaHub.Global.Gerit.Domain.ReadModels;

namespace VianaHub.Global.Gerit.Application.Interfaces.Billing;

public interface ITenantContactAppService
{
    Task<TenantContactResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<TenantContactResponse>> GetByTenantIdAsync(int tenantId, CancellationToken ct);
    Task<TenantContactResponse> GetPrimaryByTenantIdAsync(int tenantId, CancellationToken ct);
    Task<IEnumerable<TenantContactResponse>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<TenantContactResponse>> GetActiveAsync(CancellationToken ct);
    Task<ListPageResponse<TenantContactResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateTenantContactRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateTenantContactRequest request, CancellationToken ct);
    Task<bool> SetAsPrimaryAsync(int id, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
