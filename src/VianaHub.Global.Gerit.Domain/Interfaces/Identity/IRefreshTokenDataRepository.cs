using VianaHub.Global.Gerit.Domain.Entities.Identity;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Identity;

public interface IRefreshTokenDataRepository
{
    Task AddAsync(RefreshTokenEntity entity);
    Task<RefreshTokenEntity> GetByTokenAsync(string token, int tenantId);
    Task<IEnumerable<RefreshTokenEntity>> GetByUserAsync(int userId, int tenantId);
    Task RevokeAsync(string token, int revokedBy, int tenantId);
}
