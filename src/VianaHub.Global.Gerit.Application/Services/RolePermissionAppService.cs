using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Request.RolePermission;
using VianaHub.Global.Gerit.Application.Dtos.Response.RolePermission;
using VianaHub.Global.Gerit.Application.Interfaces;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Interfaces.Domain;
using VianaHub.Global.Gerit.Domain.Interfaces.Repository;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Application.Services;

public class RolePermissionAppService : IRolePermissionAppService
{
    private readonly IRolePermissionDomainService _domain;
    private readonly IRolePermissionDataRepository _repository;
    private readonly IRoleDataRepository _roleRepository;
    private readonly IResourceDataRepository _resourceRepository;
    private readonly IActionDataRepository _actionRepository;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;

    public RolePermissionAppService(
        IRolePermissionDomainService domain,
        IRolePermissionDataRepository repository,
        IRoleDataRepository roleRepository,
        IResourceDataRepository resourceRepository,
        IActionDataRepository actionRepository,
        IMapper mapper,
        INotify notify,
        ILocalizationService localization,
        ICurrentUserService currentUser)
    {
        _domain = domain;
        _repository = repository;
        _roleRepository = roleRepository;
        _resourceRepository = resourceRepository;
        _actionRepository = actionRepository;
        _mapper = mapper;
        _notify = notify;
        _localization = localization;
        _currentUser = currentUser;
    }

    public async Task<RolePermissionResponse> CreateAsync(CreateRolePermissionRequest request, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();

        // Valida duplicidade
        var exists = await _repository.ExistsAsync(tenantId, request.RoleId, request.ResourceId, request.ActionId);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.RolePermission.Create.ResourceAlreadyExists"), 400);
            return null;
        }

        // Valida existência de role/resource/action
        var roleExists = await _roleRepository.ExistsByIdAsync(request.RoleId, ct);
        if (!roleExists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.RolePermission.Create.RoleNotFound"), 404);
            return null;
        }

        var resourceExists = await _resourceRepository.ExistsByIdAsync(request.ResourceId, ct);
        if (!resourceExists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.RolePermission.Create.ResourceNotFound"), 404);
            return null;
        }

        var actionExists = await _actionRepository.ExistsByIdAsync(request.ActionId, ct);
        if (!actionExists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.RolePermission.Create.ActionNotFound"), 404);
            return null;
        }

        var entity = new RolePermissionEntity(tenantId, request.RoleId, request.ResourceId, request.ActionId);
        await _domain.CreateAsync(entity);
        return _mapper.Map<RolePermissionResponse>(entity);
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var entity = await _repository.GetByIdAsync(id, tenantId);
        if (entity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.RolePermission.Delete.ResourceNotFound"), 410);
            return;
        }

        await _repository.DeleteAsync(id, tenantId);
    }

    public async Task<RolePermissionResponse> GetByIdAsync(int id, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var entity = await _repository.GetByIdAsync(id, tenantId);
        return _mapper.Map<RolePermissionResponse>(entity);
    }

    public async Task<IList<RolePermissionResponse>> GetByRoleAsync(int roleId, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var list = await _repository.GetByRoleAsync(roleId, tenantId);
        return _mapper.Map<IList<RolePermissionResponse>>(list);
    }

    public async Task<IList<RolePermissionResponse>> GetByResourceAsync(int resourceId, CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var list = await _repository.GetByResourceAsync(resourceId, tenantId);
        return _mapper.Map<IList<RolePermissionResponse>>(list);
    }

    public async Task<IList<RolePermissionResponse>> GetAllAsync(CancellationToken ct)
    {
        var tenantId = _currentUser.GetTenantId();
        var list = await _repository.GetAllAsync(tenantId);
        return _mapper.Map<IList<RolePermissionResponse>>(list);
    }
}
