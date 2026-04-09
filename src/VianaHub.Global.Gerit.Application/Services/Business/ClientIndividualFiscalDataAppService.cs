using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientIndividualFiscalData;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientIndividualFiscalData;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Application.Services.Business;

public class ClientIndividualFiscalDataAppService : IClientIndividualFiscalDataAppService
{
    private readonly IClientIndividualFiscalDataDataRepository _repo;
    private readonly IClientIndividualFiscalDataDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;

    public ClientIndividualFiscalDataAppService(
        IClientIndividualFiscalDataDataRepository repo,
        IClientIndividualFiscalDataDomainService domain,
        IMapper mapper,
        INotify notify,
        ILocalizationService localization,
        ICurrentUserService currentUser)
    {
        _repo = repo;
        _domain = domain;
        _mapper = mapper;
        _notify = notify;
        _localization = localization;
        _currentUser = currentUser;
    }

    public async Task<ClientIndividualFiscalDataResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientIndividualFiscalData.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<ClientIndividualFiscalDataResponse>(entity);
    }

    public async Task<ClientIndividualFiscalDataResponse> GetByClientIndividualIdAsync(int clientIndividualId, CancellationToken ct)
    {
        var entity = await _repo.GetByClientIndividualIdAsync(clientIndividualId, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientIndividualFiscalData.GetByClientIndividualId.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<ClientIndividualFiscalDataResponse>(entity);
    }

    public async Task<IEnumerable<ClientIndividualFiscalDataResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<ClientIndividualFiscalDataResponse>>(entities);
    }

    public async Task<IEnumerable<ClientIndividualFiscalDataResponse>> GetActiveAsync(CancellationToken ct)
    {
        var entities = await _repo.GetActiveAsync(ct);
        return _mapper.Map<IEnumerable<ClientIndividualFiscalDataResponse>>(entities);
    }

    public async Task<ListPageResponse<ClientIndividualFiscalDataResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.IsActive, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<ClientIndividualFiscalDataResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateClientIndividualFiscalDataRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var exists = await _repo.ExistsByClientIndividualIdAsync(request.ClientIndividualId, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientIndividualFiscalData.Create.ClientIndividualAlreadyHasFiscalData"), 409);
            return false;
        }

        if (!string.IsNullOrWhiteSpace(request.TaxNumber))
        {
            var taxNumberExists = await _repo.ExistsByTaxNumberAsync(request.TaxNumber, null, ct);
            if (taxNumberExists)
            {
                _notify.Add(_localization.GetMessage("Application.Service.ClientIndividualFiscalData.Create.TaxNumberAlreadyExists"), 409);
                return false;
            }
        }

        var entity = new ClientIndividualFiscalDataEntity(tenantId, request.ClientIndividualId, request.TaxNumber, request.VatNumber, request.FiscalCountry, request.IsVatRegistered, request.IBAN, request.FiscalEmail, _currentUser.GetUserId());
        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateClientIndividualFiscalDataRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientIndividualFiscalData.Update.ResourceNotFound"), 410);
            return false;
        }

        if (!string.IsNullOrWhiteSpace(request.TaxNumber))
        {
            var taxNumberExists = await _repo.ExistsByTaxNumberAsync(request.TaxNumber, id, ct);
            if (taxNumberExists)
            {
                _notify.Add(_localization.GetMessage("Application.Service.ClientIndividualFiscalData.Update.TaxNumberAlreadyExists"), 409);
                return false;
            }
        }

        entity.Update(request.TaxNumber, request.VatNumber, request.FiscalCountry, request.IsVatRegistered, request.IBAN, request.FiscalEmail, _currentUser.GetUserId());

        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientIndividualFiscalData.Activate.ResourceNotFound"), 410);
            return false;
        }

        entity.Activate(_currentUser.GetUserId());
        return await _domain.ActivateAsync(entity, ct);
    }

    public async Task<bool> DeactivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientIndividualFiscalData.Deactivate.ResourceNotFound"), 410);
            return false;
        }

        entity.Deactivate(_currentUser.GetUserId());
        return await _domain.DeactivateAsync(entity, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientIndividualFiscalData.Delete.ResourceNotFound"), 410);
            return false;
        }

        entity.Delete(_currentUser.GetUserId());
        return await _domain.DeleteAsync(entity, ct);
    }
}
