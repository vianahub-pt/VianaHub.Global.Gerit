using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientCompanyFiscalData;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientCompanyFiscalData;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Application.Interfaces.Common;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Application.Services.Business;

public class ClientCompanyFiscalDataAppService : IClientCompanyFiscalDataAppService
{
    private readonly IClientCompanyFiscalDataDataRepository _repo;
    private readonly IClientCompanyFiscalDataDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;

    public ClientCompanyFiscalDataAppService(
        IClientCompanyFiscalDataDataRepository repo,
        IClientCompanyFiscalDataDomainService domain,
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

    public async Task<IEnumerable<ClientCompanyFiscalDataResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<ClientCompanyFiscalDataResponse>>(entities);
    }

    public async Task<ClientCompanyFiscalDataResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientCompanyFiscalData.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<ClientCompanyFiscalDataResponse>(entity);
    }

    public async Task<ClientCompanyFiscalDataResponse> GetByClientCompanyIdAsync(int clientCompanyId, CancellationToken ct)
    {
        var entity = await _repo.GetByClientCompanyIdAsync(clientCompanyId, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientCompanyFiscalData.GetByClientCompanyId.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<ClientCompanyFiscalDataResponse>(entity);
    }

    public async Task<ListPageResponse<ClientCompanyFiscalDataResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.IsActive, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<ClientCompanyFiscalDataResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateClientCompanyFiscalDataRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();

        if (await _repo.ExistsByClientCompanyIdAsync(request.ClientCompanyId, ct))
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientCompanyFiscalData.Create.ClientCompanyAlreadyHasFiscalData"), 409);
            return false;
        }

        if (!string.IsNullOrWhiteSpace(request.TaxNumber) &&
            await _repo.ExistsByTaxNumberAsync(request.TaxNumber, null, ct))
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientCompanyFiscalData.Create.TaxNumberAlreadyExists"), 409);
            return false;
        }

        var entity = new ClientCompanyFiscalDataEntity(
            tenantId,
            request.ClientCompanyId,
            request.TaxNumber,
            request.VatNumber,
            request.FiscalCountry,
            request.IsVatRegistered,
            request.IBAN,
            request.FiscalEmail,
            _currentUser.GetUserId());

        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateClientCompanyFiscalDataRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientCompanyFiscalData.Update.ResourceNotFound"), 410);
            return false;
        }

        if (!string.IsNullOrWhiteSpace(request.TaxNumber) &&
            await _repo.ExistsByTaxNumberAsync(request.TaxNumber, id, ct))
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientCompanyFiscalData.Update.TaxNumberAlreadyExists"), 409);
            return false;
        }

        entity.Update(
            request.TaxNumber,
            request.VatNumber,
            request.FiscalCountry,
            request.IsVatRegistered,
            request.IBAN,
            request.FiscalEmail,
            _currentUser.GetUserId());

        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientCompanyFiscalData.Activate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.ClientCompanyFiscalData.Deactivate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.ClientCompanyFiscalData.Delete.ResourceNotFound"), 410);
            return false;
        }

        entity.Delete(_currentUser.GetUserId());
        return await _domain.DeleteAsync(entity, ct);
    }
}
