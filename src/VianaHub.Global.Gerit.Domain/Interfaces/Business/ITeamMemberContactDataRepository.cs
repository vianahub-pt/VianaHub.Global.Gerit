using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de repositório para TeamMemberContact
/// </summary>
public interface ITeamMemberContactDataRepository
{
    Task<TeamMemberContactEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<TeamMemberContactEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<TeamMemberContactEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByEmailAsync(int tenantId, int teamMemberId, string email, CancellationToken ct);
    Task<bool> ExistsByEmailForUpdateAsync(int tenantId, int teamMemberId, string email, int excludeId, CancellationToken ct);
    Task<bool> AddAsync(TeamMemberContactEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(TeamMemberContactEntity entity, CancellationToken ct);
}
