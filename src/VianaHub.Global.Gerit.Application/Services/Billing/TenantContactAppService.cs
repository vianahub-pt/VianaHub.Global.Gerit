using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Billing.TenantContact;
using VianaHub.Global.Gerit.Application.Dtos.Response.Billing.TenantContact;
using VianaHub.Global.Gerit.Application.Interfaces.Billing;
using VianaHub.Global.Gerit.Application.Interfaces.Common;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;
using VianaHub.Global.Gerit.Domain.Interfaces.Billing;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Application.Services.Billing;

public class TenantContactAppService : ITenantContactAppService
{
    private readonly ITenantContactDataRepository _repo;
    private readonly ITenantContactDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;

    public TenantContactAppService(
        ITenantContactDataRepository repo,
        ITenantContactDomainService domain,
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

    public async Task<IEnumerable<TenantContactResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<TenantContactResponse>>(entities);
    }

    public async Task<IEnumerable<TenantContactResponse>> GetActiveAsync(CancellationToken ct)
    {
        var entities = await _repo.GetActiveAsync(ct);
        return _mapper.Map<IEnumerable<TenantContactResponse>>(entities);
    }

    public async Task<TenantContactResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TenantContact.GetById.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<TenantContactResponse>(entity);
    }

    public async Task<IEnumerable<TenantContactResponse>> GetByTenantIdAsync(int tenantId, CancellationToken ct)
    {
        var entities = await _repo.GetByTenantIdAsync(tenantId, ct);
        return _mapper.Map<IEnumerable<TenantContactResponse>>(entities);
    }

    public async Task<TenantContactResponse> GetPrimaryByTenantIdAsync(int tenantId, CancellationToken ct)
    {
        var entity = await _repo.GetPrimaryByTenantIdAsync(tenantId, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TenantContact.GetPrimaryByTenantId.ResourceNotFound"), 404);
            return null;
        }
        return _mapper.Map<TenantContactResponse>(entity);
    }

    public async Task<ListPageResponse<TenantContactResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.IsActive, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<TenantContactResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateTenantContactRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();

        if (request.IsPrimary && await _repo.ExistsPrimaryContactAsync(tenantId, ct))
        {
            _notify.Add(_localization.GetMessage("Application.Service.TenantContact.Create.PrimaryAlreadyExists"), 409);
            return false;
        }

        var entity = new TenantContactEntity(
            tenantId,
            request.Name,
            request.Email,
            request.Phone,
            request.IsPrimary,
            _currentUser.GetUserId()
        );

        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateTenantContactRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TenantContact.Update.ResourceNotFound"), 410);
            return false;
        }

        entity.Update(request.Name, request.Email, request.Phone, _currentUser.GetUserId());

        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> SetAsPrimaryAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TenantContact.SetAsPrimary.ResourceNotFound"), 410);
            return false;
        }

        var currentPrimary = await _repo.GetPrimaryByTenantIdAsync(entity.TenantId, ct);
        if (currentPrimary != null && currentPrimary.Id != id)
        {
            currentPrimary.RemoveAsPrimary();
            await _repo.UpdateAsync(currentPrimary, ct);
        }

        entity.SetAsPrimary();
        return await _repo.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted)
        {
            _notify.Add(_localization.GetMessage("Application.Service.TenantContact.Activate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.TenantContact.Deactivate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.TenantContact.Delete.ResourceNotFound"), 410);
            return false;
        }

        entity.Delete(_currentUser.GetUserId());
        return await _domain.DeleteAsync(entity, ct);
    }
}
