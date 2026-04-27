using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientConsents;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientConsents;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Application.Services.Business;

public class ClientConsentsAppService : IClientConsentsAppService
{
    public int TenantId { get; set; }
    public int UserId { get; set; }
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
        TenantId = _currentUser.GetTenantId();
        UserId = _currentUser.GetUserId();
    }

    public async Task<IEnumerable<ClientConsentsResponse>> GetAllAsync(int clientId, CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(clientId, ct);
        return _mapper.Map<IEnumerable<ClientConsentsResponse>>(entities);
    }

    public async Task<ClientConsentsResponse> GetByIdAsync(int clientId, int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(clientId, id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientConsents.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<ClientConsentsResponse>(entity);
    }

    public async Task<ListPageResponse<ClientConsentsResponse>> GetPagedAsync(int clientId, PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.IsActive, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(clientId, filter, ct);
        return _mapper.Map<ListPageResponse<ClientConsentsResponse>>(paged);
    }

    public async Task<bool> CreateAsync(int clientId, CreateClientConsentsRequest request, CancellationToken ct)
    {
        var exists = await _repo.ExistsByClientAndConsentTypeAsync(clientId, request.ConsentTypeId, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientConsents.Create.ResourceAlreadyExists"), 409);
            return false;
        }

        var entity = new ClientConsentsEntity(
            TenantId,
            clientId,
            request.ConsentTypeId,
            request.Granted,
            request.GrantedDate,
            request.Origin,
            request.IpAddress,
            request.UserAgent,
            UserId);

        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int clientId, int id, UpdateClientConsentsRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(clientId, id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientConsents.Update.ResourceNotFound"), 410);
            return false;
        }

        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(int clientId, int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(clientId, id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientConsents.Activate.ResourceNotFound"), 410);
            return false;
        }

        entity.Activate(_currentUser.GetUserId());
        return await _domain.ActivateAsync(entity, ct);
    }

    public async Task<bool> DeactivateAsync(int clientId, int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(clientId, id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientConsents.Deactivate.ResourceNotFound"), 410);
            return false;
        }

        entity.Deactivate(_currentUser.GetUserId());
        return await _domain.DeactivateAsync(entity, ct);
    }

    public async Task<bool> DeleteAsync(int clientId, int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(clientId, id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientConsents.Delete.ResourceNotFound"), 410);
            return false;
        }

        entity.Delete(_currentUser.GetUserId());
        return await _domain.DeleteAsync(entity, ct);
    }
}
