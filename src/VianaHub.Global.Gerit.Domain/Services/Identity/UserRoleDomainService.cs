using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Identity;
using VianaHub.Global.Gerit.Domain.Interfaces.Identity;

namespace VianaHub.Global.Gerit.Domain.Services.Identity;

public class UserRoleDomainService : IUserRoleDomainService
{
    private readonly IUserRoleDataRepository _repository;
    private readonly IValidator<UserRoleEntity> _validator;

    public UserRoleDomainService(IUserRoleDataRepository repository, IValidator<UserRoleEntity> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<bool> CreateAsync(UserRoleEntity entity, CancellationToken ct)
    {
        var validation = _validator.Validate(entity);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        await _repository.AddAsync(entity, ct);
        return true;
    }
}
