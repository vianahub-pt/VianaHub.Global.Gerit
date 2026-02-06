using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de serviço de domínio para TeamMemberAddress
/// </summary>
public interface ITeamMemberAddressDomainService
{
    Task<bool> CreateAsync(TeamMemberAddressEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(TeamMemberAddressEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(TeamMemberAddressEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(TeamMemberAddressEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(TeamMemberAddressEntity entity, CancellationToken ct);
}
