using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientIndividualFiscalData;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientIndividualFiscalData;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IClientIndividualFiscalDataAppService
{
    Task<ClientIndividualFiscalDataResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ClientIndividualFiscalDataResponse> GetByClientIndividualIdAsync(int clientIndividualId, CancellationToken ct);
    Task<IEnumerable<ClientIndividualFiscalDataResponse>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<ClientIndividualFiscalDataResponse>> GetActiveAsync(CancellationToken ct);
    Task<ListPageResponse<ClientIndividualFiscalDataResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateClientIndividualFiscalDataRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateClientIndividualFiscalDataRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
