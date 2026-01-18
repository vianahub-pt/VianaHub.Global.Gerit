using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Subscription;
using VianaHub.Global.Gerit.Application.Dtos.Response.Subscription;
using VianaHub.Global.Gerit.Application.Interfaces;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Interfaces;
using VianaHub.Global.Gerit.Domain.Interfaces.Domain;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Application.Services;

public class SubscriptionAppService : ISubscriptionAppService
{
    private readonly ISubscriptionDomainService _domain;
    private readonly ICurrentUserService _currentUser;
    private readonly INotify _notify;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localization;

    public SubscriptionAppService(
        ISubscriptionDomainService domain,
        INotify notify,
        IMapper mapper,
        ICurrentUserService currentUser,
        ILocalizationService localization)
    {
        _domain = domain;
        _notify = notify;
        _mapper = mapper;
        _currentUser = currentUser;
        _localization = localization;
    }

    public async Task<IEnumerable<SubscriptionResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _domain.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<SubscriptionResponse>>(entities);
    }

    public async Task<SubscriptionResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _domain.GetByIdAsync(id, ct);
        return _mapper.Map<SubscriptionResponse>(entity);
    }

    public async Task<SubscriptionResponse> GetByTenantIdAsync(int tenantId, CancellationToken ct)
    {
        var entity = await _domain.GetByTenantIdAsync(tenantId, ct);
        return _mapper.Map<SubscriptionResponse>(entity);
    }

    public async Task<ListPageResponse<SubscriptionResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _domain.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<SubscriptionResponse>>(paged);
    }

    public async Task<IEnumerable<SubscriptionResponse>> GetActiveSubscriptionsAsync(CancellationToken ct)
    {
        var entities = await _domain.GetActiveSubscriptionsAsync(ct);
        return _mapper.Map<IEnumerable<SubscriptionResponse>>(entities);
    }

    public async Task<IEnumerable<SubscriptionResponse>> GetExpiringSubscriptionsAsync(int daysBeforeExpiration, CancellationToken ct)
    {
        var entities = await _domain.GetExpiringSubscriptionsAsync(daysBeforeExpiration, ct);
        return _mapper.Map<IEnumerable<SubscriptionResponse>>(entities);
    }

    public async Task<bool> CreateAsync(CreateSubscriptionRequest request, CancellationToken ct)
    {
        // Verifica se já existe uma subscription ativa para o tenant
        var exists = await _domain.ExistsByTenantIdAsync(request.TenantId, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Subscription.Create.TenantAlreadyHasSubscription"), 400);
            return false;
        }

        var entity = new SubscriptionEntity(
            request.TenantId,
            request.PlanId,
            request.CurrentPeriodStart,
            request.CurrentPeriodEnd,
            _currentUser.GetUserId(),
            request.TrialStart,
            request.TrialEnd,
            request.StripeCustomerId
        );

        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateSubscriptionRequest request, CancellationToken ct)
    {
        var entity = await _domain.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Subscription.Update.ResourceNotFound"), 404);
            return false;
        }

        entity.Update(
            request.PlanId,
            request.CurrentPeriodStart,
            request.CurrentPeriodEnd,
            _currentUser.GetUserId(),
            request.TrialStart,
            request.TrialEnd
        );

        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var entity = await _domain.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Subscription.Activate.ResourceNotFound"), 404);
            return false;
        }

        entity.Activate(_currentUser.GetUserId());
        return await _domain.ActivateAsync(entity, ct);
    }

    public async Task<bool> DeactivateAsync(int id, CancellationToken ct)
    {
        var entity = await _domain.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Subscription.Deactivate.ResourceNotFound"), 404);
            return false;
        }

        entity.Deactivate(_currentUser.GetUserId());
        return await _domain.DeactivateAsync(entity, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await _domain.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Subscription.Delete.ResourceNotFound"), 404);
            return false;
        }

        entity.Delete(_currentUser.GetUserId());
        return await _domain.DeleteAsync(entity, ct);
    }

    public async Task<bool> CancelAsync(int id, CancelSubscriptionRequest request, CancellationToken ct)
    {
        var entity = await _domain.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Subscription.Cancel.ResourceNotFound"), 404);
            return false;
        }

        entity.Cancel(request.CancellationReason, request.CancelAtPeriodEnd, _currentUser.GetUserId());
        return await _domain.CancelAsync(entity, ct);
    }

    public async Task<bool> RenewAsync(int id, RenewSubscriptionRequest request, CancellationToken ct)
    {
        var entity = await _domain.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Subscription.Renew.ResourceNotFound"), 404);
            return false;
        }

        entity.Renew(request.NewPeriodEnd, _currentUser.GetUserId());
        return await _domain.RenewAsync(entity, ct);
    }
}
