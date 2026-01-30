using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Domain.Interfaces;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.Vehicle;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Vehicle;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;

namespace VianaHub.Global.Gerit.Application.Services.Business;

public class VehicleAppService : IVehicleAppService
{
    private readonly IVehicleDataRepository _repo;
    private readonly IVehicleDomainService _domain;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;

    public VehicleAppService(
        IVehicleDataRepository repo,
        IVehicleDomainService domain,
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

    public async Task<IEnumerable<VehicleResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<VehicleResponse>>(entities);
    }

    public async Task<VehicleResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null || entity.IsDeleted || !entity.IsActive)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Vehicle.Update.ResourceNotFound"), 410);
            return null;
        }
        return _mapper.Map<VehicleResponse>(entity);
    }

    public async Task<ListPageResponse<VehicleResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct)
    {
        var filter = new PagedFilter(request.Search, request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
        var paged = await _repo.GetPagedAsync(filter, ct);
        return _mapper.Map<ListPageResponse<VehicleResponse>>(paged);
    }

    public async Task<bool> CreateAsync(CreateVehicleRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var exists = await _repo.ExistsByPlateAsync(tenantId, request.Plate, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Vehicle.Create.ResourceAlreadyExists"), 400);
            return false;
        }

        var entity = new VehicleEntity(tenantId, request.Plate, request.Brand, request.Model, request.Year, request.Color, request.FuelType);
        return await _domain.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(int id, UpdateVehicleRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Vehicle.Update.ResourceNotFound"), 410);
            return false;
        }

        entity.SetPlate(request.Plate);
        entity.SetBrand(request.Brand);
        entity.SetModel(request.Model);
        entity.SetYear(request.Year);
        entity.SetColor(request.Color);
        entity.SetFuelType(request.FuelType);
        return await _domain.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Vehicle.Activate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.Vehicle.Deactivate.ResourceNotFound"), 410);
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
            _notify.Add(_localization.GetMessage("Application.Service.Vehicle.Delete.ResourceNotFound"), 410);
            return false;
        }

        entity.Delete();
        return await _domain.DeleteAsync(entity, ct);
    }
}
