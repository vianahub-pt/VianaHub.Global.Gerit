using VianaHub.Global.Gerit.Domain.Entities.Identity;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Identity;

public interface IUserRoleDomainService
{
    Task<bool> CreateAsync(UserRoleEntity entity, CancellationToken ct);
}
