using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de serviço de domínio para TeamMemberContact
/// </summary>
public interface ITeamMemberContactDomainService
{
    Task<TeamMemberContactEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<TeamMemberContactEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<TeamMemberContactEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> CreateAsync(TeamMemberContactEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(TeamMemberContactEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(TeamMemberContactEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(TeamMemberContactEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(TeamMemberContactEntity entity, CancellationToken ct);
}
