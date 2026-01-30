using AutoMapper;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Domain.Interfaces;
using VianaHub.Global.Gerit.Application.Interfaces.Identity;
using VianaHub.Global.Gerit.Application.Dtos.Response.Identity.UserRole;
using VianaHub.Global.Gerit.Application.Dtos.Request.Identity.UserRole;
using VianaHub.Global.Gerit.Domain.Interfaces.Identity;
using VianaHub.Global.Gerit.Domain.Entities.Identity;

namespace VianaHub.Global.Gerit.Application.Services.Identity;

public class UserRoleAppService : IUserRoleAppService
{
    private readonly IUserRoleDomainService _domain;
    private readonly IUserRoleDataRepository _repository;
    private readonly IRoleDataRepository _roleRepository;
    private readonly IMapper _mapper;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;

    public UserRoleAppService(
        IUserRoleDomainService domain,
        IUserRoleDataRepository repository,
        IRoleDataRepository roleRepository,
        IMapper mapper,
        INotify notify,
        ILocalizationService localization,
        ICurrentUserService currentUser)
    {
        _domain = domain;
        _repository = repository;
        _roleRepository = roleRepository;
        _mapper = mapper;
        _notify = notify;
        _localization = localization;
        _currentUser = currentUser;
    }

    public async Task<UserRoleResponse> CreateAsync(CreateUserRoleRequest request)
    {
        var tenantId = _currentUser.GetTenantId();

        // Verifica duplicidade
        var existing = await _repository.GetByUserAsync(request.UserId, tenantId);
        if (existing != null && existing.Any(x => x.RoleId == request.RoleId))
        {
            _notify.Add(_localization.GetMessage("Application.Service.UserRole.Create.ResourceAlreadyExists"), 400);
            return null;
        }

        // Valida se o Role existe
        var roleExists = await _roleRepository.ExistsByIdAsync(request.RoleId, CancellationToken.None);
        if (!roleExists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.UserRole.Create.RoleNotFound"), 404);
            return null;
        }

        var entity = new UserRoleEntity(tenantId, request.UserId, request.RoleId);
        await _domain.CreateAsync(entity);
        return _mapper.Map<UserRoleResponse>(entity);
    }

    public async Task<UserRoleResponse> UpdateAsync(UpdateUserRoleRequest request)
    {
        var tenantId = _currentUser.GetTenantId();
        var entity = await _repository.GetByIdAsync(request.Id, tenantId);
        if (entity == null)
            throw new KeyNotFoundException();
        
        return _mapper.Map<UserRoleResponse>(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var tenantId = _currentUser.GetTenantId();
        var entity = await _repository.GetByIdAsync(id, tenantId);
        if (entity == null)
        {
            // Not found -> notify with 410 Gone
            _notify.Add(_localization.GetMessage("Application.Service.UserRole.Delete.ResourceNotFound"), 410);
            return;
        }

        await _repository.DeleteAsync(id, tenantId);
    }

    public async Task<UserRoleResponse> GetByIdAsync(int id)
    {
        var tenantId = _currentUser.GetTenantId();
        var entity = await _repository.GetByIdAsync(id, tenantId);
        return _mapper.Map<UserRoleResponse>(entity);
    }

    public async Task<IList<UserRoleResponse>> GetByUserAsync(int userId)
    {
        var tenantId = _currentUser.GetTenantId();
        var list = await _repository.GetByUserAsync(userId, tenantId);
        return _mapper.Map<IList<UserRoleResponse>>(list);
    }

    public async Task<IList<UserRoleResponse>> GetByRoleAsync(int roleId)
    {
        var tenantId = _currentUser.GetTenantId();
        var list = await _repository.GetByRoleAsync(roleId, tenantId);
        return _mapper.Map<IList<UserRoleResponse>>(list);
    }

    public async Task<IList<UserRoleResponse>> GetAllAsync()
    {
        var tenantId = _currentUser.GetTenantId();
        var list = await _repository.GetAllAsync(tenantId);
        return _mapper.Map<IList<UserRoleResponse>>(list);
    }
}
