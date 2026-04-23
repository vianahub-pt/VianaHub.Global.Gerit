using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientHierarchy;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientHierarchy;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Enums;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Application.Services.Business;

public class ClientHierarchyAppService : IClientHierarchyAppService
{
    private readonly IClientHierarchyDataRepository _repo;
    private readonly IClientHierarchyDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;

    public ClientHierarchyAppService(
        IClientHierarchyDataRepository repo,
        IClientHierarchyDomainService domain,
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

    public async Task<IEnumerable<ClientHierarchyResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<ClientHierarchyResponse>>(entities);
    }

    public async Task<ClientHierarchyResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientHierarchy.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<ClientHierarchyResponse>(entity);
    }

    public async Task<IEnumerable<ClientHierarchyResponse>> GetByParentClientIdAsync(int parentClientId, CancellationToken ct)
    {
        var entities = await _repo.GetByParentClientIdAsync(parentClientId, ct);
        return _mapper.Map<IEnumerable<ClientHierarchyResponse>>(entities);
    }

    public async Task<IEnumerable<ClientHierarchyResponse>> GetByChildClientIdAsync(int childClientId, CancellationToken ct)
    {
        var entities = await _repo.GetByChildClientIdAsync(childClientId, ct);
        return _mapper.Map<IEnumerable<ClientHierarchyResponse>>(entities);
    }

    public async Task<ListPageResponse<ClientHierarchyResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.IsActive, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<ClientHierarchyResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateClientHierarchyRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();

        if (await _repo.ExistsRelationshipAsync(request.ParentId, request.ChildId, ct))
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientHierarchy.Create.RelationshipAlreadyExists"), 409);
            return false;
        }

        var entity = new ClientHierarchyEntity(
            tenantId,
            request.ParentId,
            request.ChildId,
            (RelationshipType)request.RelationshipType,
            _currentUser.GetUserId());

        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateClientHierarchyRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientHierarchy.Update.ResourceNotFound"), 410);
            return false;
        }

        //entity.Update(request.RelationshipType, _currentUser.GetUserId());

        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.ClientHierarchy.Activate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.ClientHierarchy.Deactivate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.ClientHierarchy.Delete.ResourceNotFound"), 410);
            return false;
        }

        entity.Delete(_currentUser.GetUserId());
        return await _domain.DeleteAsync(entity, ct);
    }
}
