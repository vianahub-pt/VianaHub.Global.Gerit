using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface ITeamMembersTeamDataRepository
{
    Task<TeamMembersTeamEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<TeamMembersTeamEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<TeamMembersTeamEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByTeamAndMemberAsync(int tenantId, int teamId, int teamMemberId, CancellationToken ct);
    Task<bool> AddAsync(TeamMembersTeamEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(TeamMembersTeamEntity entity, CancellationToken ct);
}
