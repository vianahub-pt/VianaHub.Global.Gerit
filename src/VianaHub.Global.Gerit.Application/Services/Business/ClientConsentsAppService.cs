using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientConsents;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientConsents;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Application.Interfaces.Common;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Application.Services.Business;

public class ClientConsentsAppService : IClientConsentsAppService
{
    private readonly IClientConsentsDataRepository _repo;
    private readonly IClientConsentsDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;

    public ClientConsentsAppService(
        IClientConsentsDataRepository repo,
        IClientConsentsDomainService domain,
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

    public async Task<IEnumerable<ClientConsentsResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<ClientConsentsResponse>>(entities);
    }

    public async Task<ClientConsentsResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientConsents.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<ClientConsentsResponse>(entity);
    }

    public async Task<IEnumerable<ClientConsentsResponse>> GetByClientIdAsync(int clientId, CancellationToken ct)
    {
        var entities = await _repo.GetByClientIdAsync(clientId, ct);
        return _mapper.Map<IEnumerable<ClientConsentsResponse>>(entities);
    }

    public async Task<IEnumerable<ClientConsentsResponse>> GetByConsentTypeIdAsync(int consentTypeId, CancellationToken ct)
    {
        var entities = await _repo.GetByConsentTypeIdAsync(consentTypeId, ct);
        return _mapper.Map<IEnumerable<ClientConsentsResponse>>(entities);
    }

    public async Task<ClientConsentsResponse> GetByClientAndConsentTypeAsync(int clientId, int consentTypeId, CancellationToken ct)
    {
        var entity = await _repo.GetByClientAndConsentTypeAsync(clientId, consentTypeId, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientConsents.GetByClientAndConsentType.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<ClientConsentsResponse>(entity);
    }

    public async Task<ListPageResponse<ClientConsentsResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.IsActive, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<ClientConsentsResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateClientConsentsRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var exists = await _repo.ExistsByClientAndConsentTypeAsync(request.ClientId, request.ConsentTypeId, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientConsents.Create.ResourceAlreadyExists"), 409);
            return false;
        }

        var entity = new ClientConsentsEntity(
            tenantId,
            request.ClientId,
            request.ConsentTypeId,
            request.Granted,
            request.GrantedDate,
            request.Origin,
            request.IpAddress,
            request.UserAgent,
            _currentUser.GetUserId());

        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateClientConsentsRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientConsents.Update.ResourceNotFound"), 410);
            return false;
        }

        if (request.Granted && !entity.Granted)
        {
            entity.GrantConsent(_currentUser.GetUserId());
        }
        else if (!request.Granted && entity.Granted)
        {
            entity.RevokeConsent(_currentUser.GetUserId());
        }

        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> RevokeConsentAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientConsents.RevokeConsent.ResourceNotFound"), 410);
            return false;
        }

        entity.RevokeConsent(_currentUser.GetUserId());
        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> GrantConsentAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientConsents.GrantConsent.ResourceNotFound"), 410);
            return false;
        }

        entity.GrantConsent(_currentUser.GetUserId());
        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientConsents.Activate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.ClientConsents.Deactivate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.ClientConsents.Delete.ResourceNotFound"), 410);
            return false;
        }

        entity.Delete(_currentUser.GetUserId());
        return await _domain.DeleteAsync(entity, ct);
    }
}
