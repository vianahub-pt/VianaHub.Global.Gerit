using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientCompanyFiscalData;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientCompanyFiscalData;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IClientCompanyFiscalDataAppService
{
    Task<IEnumerable<ClientCompanyFiscalDataResponse>> GetAllAsync(CancellationToken ct);
    Task<ClientCompanyFiscalDataResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ClientCompanyFiscalDataResponse> GetByClientCompanyIdAsync(int clientCompanyId, CancellationToken ct);
    Task<ListPageResponse<ClientCompanyFiscalDataResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateClientCompanyFiscalDataRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateClientCompanyFiscalDataRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
