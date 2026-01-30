using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Domain.Interfaces;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.Equipment;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Equipment;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Enums;

namespace VianaHub.Global.Gerit.Application.Services.Business;

public class EquipmentAppService : IEquipmentAppService
{
    private readonly IEquipmentDataRepository _repo;
    private readonly IEquipmentDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;

    public EquipmentAppService(
        IEquipmentDataRepository repo,
        IEquipmentDomainService domain,
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

    public async Task<IEnumerable<EquipmentResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<EquipmentResponse>>(entities);
    }

    public async Task<EquipmentResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Equipment.Update.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<EquipmentResponse>(entity);
    }

    public async Task<ListPageResponse<EquipmentResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<EquipmentResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateEquipmentRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var exists = await _repo.ExistsByNameAsync(tenantId, request.Name, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Equipment.Create.ResourceAlreadyExists"), 400);
            return false;
        }

        var entity = new EquipmentEntity(tenantId, request.Name, request.TypeEquipament, request.SerialNumber);
        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateEquipmentRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Equipment.Update.ResourceNotFound"), 410);
            return false;
        }

        entity.Update(request.Name, request.TypeEquipament, request.SerialNumber);

        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Equipment.Activate.ResourceNotFound"), 410);
            return false;
        }

        entity.Activate();
        return await _domain.ActivateAsync(entity, ct);
    }

    public async Task<bool> DeactivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Equipment.Deactivate.ResourceNotFound"), 410);
            return false;
        }

        entity.Deactivate();
        return await _domain.DeactivateAsync(entity, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Equipment.Delete.ResourceNotFound"), 410);
            return false;
        }

        entity.Delete();
        return await _domain.DeleteAsync(entity, ct);
    }
}
