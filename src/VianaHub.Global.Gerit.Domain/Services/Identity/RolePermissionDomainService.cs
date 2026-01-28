using System.Threading.Tasks;
using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Interfaces.Domain;
using VianaHub.Global.Gerit.Domain.Interfaces.Repository;

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

    public async Task<bool> CreateAsync(RolePermissionEntity entity)
    {
        var validation = _validator.Validate(entity);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        await _repository.AddAsync(entity);
        return true;
    }
}
