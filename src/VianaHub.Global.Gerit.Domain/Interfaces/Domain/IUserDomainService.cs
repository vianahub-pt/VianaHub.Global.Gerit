using VianaHub.Global.Gerit.Domain.Entities;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Domain;

public interface IUserDomainService
{
    Task<bool> CreateAsync(UserEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(UserEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(UserEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(UserEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(UserEntity entity, CancellationToken ct);
}
