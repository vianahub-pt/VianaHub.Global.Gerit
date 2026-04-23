using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientCompany;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientCompany;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IClientCompanyAppService
{
    Task<IEnumerable<ClientCompanyResponse>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<ClientCompanyResponse>> GetActiveAsync(CancellationToken ct);
    Task<ClientCompanyResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ClientCompanyResponse> GetByClientIdAsync(int clientId, CancellationToken ct);
    Task<ListPageResponse<ClientCompanyResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateClientCompanyRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateClientCompanyRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
