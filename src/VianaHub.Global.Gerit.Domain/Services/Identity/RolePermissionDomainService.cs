using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Identity;
using VianaHub.Global.Gerit.Domain.Interfaces.Identity;

namespace VianaHub.Global.Gerit.Domain.Services.Identity;

public class RolePermissionDomainService : IRolePermissionDomainService
{
    private readonly IRolePermissionDataRepository _repository;
    private readonly IValidator<RolePermissionEntity> _validator;

    public RolePermissionDomainService(IRolePermissionDataRepository repository, IValidator<RolePermissionEntity> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<IList<RolePermissionEntity>> GetAllAsync(int tenantId, CancellationToken ct)
    {
        return await _repository.GetAllAsync(tenantId, ct);
    }
    public async Task<RolePermissionEntity> GetByIdAsync(int tenantId, int roleId, int resourceId, int actionId, CancellationToken ct)
    {
        return await _repository.GetByIdAsync(tenantId, roleId, resourceId, actionId, ct);
    }
    public async Task<IList<RolePermissionEntity>> GetByRoleAsync(int roleId, int tenantId, CancellationToken ct)
    {
        return await _repository.GetByRoleAsync(roleId, tenantId, ct);
    }
    public async Task<IList<RolePermissionEntity>> GetByResourceAsync(int resourceId, int tenantId, CancellationToken ct)
    {
        return await _repository.GetByResourceAsync(resourceId, tenantId, ct);
    }
    public async Task<bool> ExistsAsync(int tenantId, int roleId, int resourceId, int actionId, CancellationToken ct)
    {
        return await _repository.ExistsAsync(tenantId, roleId, resourceId, actionId, ct);
    }

    public async Task<bool> CreateAsync(RolePermissionEntity entity, CancellationToken ct)
    {
        var validation = _validator.Validate(entity);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        await _repository.CreateAsync(entity, ct);
        return true;
    }
    public async Task<bool> DeleteAsync(int tenantId, int roleId, int resourceId, int actionId, CancellationToken ct)
    {
        return await _repository.DeleteAsync(tenantId, roleId, resourceId, actionId, ct);
    }
}