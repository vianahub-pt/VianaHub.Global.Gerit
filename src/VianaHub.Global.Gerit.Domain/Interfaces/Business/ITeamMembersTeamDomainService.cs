using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface ITeamMembersTeamDomainService
{
    Task<bool> CreateAsync(TeamMembersTeamEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(TeamMembersTeamEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(TeamMembersTeamEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(TeamMembersTeamEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(TeamMembersTeamEntity entity, CancellationToken ct);
}
