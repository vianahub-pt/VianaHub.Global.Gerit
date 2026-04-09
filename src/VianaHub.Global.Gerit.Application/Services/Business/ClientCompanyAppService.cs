using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientCompany;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientCompany;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Application.Interfaces.Common;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Application.Services.Business;

public class ClientCompanyAppService : IClientCompanyAppService
{
    private readonly IClientCompanyDataRepository _repo;
    private readonly IClientCompanyDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;

    public ClientCompanyAppService(
        IClientCompanyDataRepository repo,
        IClientCompanyDomainService domain,
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

    public async Task<IEnumerable<ClientCompanyResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<ClientCompanyResponse>>(entities);
    }

    public async Task<IEnumerable<ClientCompanyResponse>> GetActiveAsync(CancellationToken ct)
    {
        var entities = await _repo.GetActiveAsync(ct);
        return _mapper.Map<IEnumerable<ClientCompanyResponse>>(entities);
    }

    public async Task<ClientCompanyResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientCompany.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<ClientCompanyResponse>(entity);
    }

    public async Task<ClientCompanyResponse> GetByClientIdAsync(int clientId, CancellationToken ct)
    {
        var entity = await _repo.GetByClientIdAsync(clientId, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientCompany.GetByClientId.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<ClientCompanyResponse>(entity);
    }

    public async Task<ListPageResponse<ClientCompanyResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.IsActive, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<ClientCompanyResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateClientCompanyRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();

        if (await _repo.ExistsByClientIdAsync(request.ClientId, ct))
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientCompany.Create.ClientAlreadyHasCompanyData"), 409);
            return false;
        }

        var entity = new ClientCompanyEntity(
            tenantId,
            request.ClientId,
            request.LegalName,
            request.TradeName,
            request.PhoneNumber,
            request.CellPhoneNumber,
            request.IsWhatsapp,
            request.Email,
            request.Site,
            request.CompanyRegistration,
            request.CAE,
            request.NumberOfEmployee,
            request.LegalRepresentative,
            _currentUser.GetUserId());

        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateClientCompanyRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientCompany.Update.ResourceNotFound"), 410);
            return false;
        }

        entity.Update(
            request.LegalName,
            request.TradeName,
            request.PhoneNumber,
            request.CellPhoneNumber,
            request.IsWhatsapp,
            request.Email,
            request.Site,
            request.CompanyRegistration,
            request.CAE,
            request.NumberOfEmployee,
            request.LegalRepresentative,
            _currentUser.GetUserId());

        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientCompany.Activate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.ClientCompany.Deactivate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.ClientCompany.Delete.ResourceNotFound"), 410);
            return false;
        }

        entity.Delete(_currentUser.GetUserId());
        return await _domain.DeleteAsync(entity, ct);
    }
}
