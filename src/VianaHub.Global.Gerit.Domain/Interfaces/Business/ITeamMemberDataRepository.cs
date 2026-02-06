using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface ITeamMemberDataRepository
{
    Task<TeamMemberEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<TeamMemberEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<TeamMemberEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByTaxNumberAsync(int tenantId, string taxNumber, CancellationToken ct);
    Task<bool> AddAsync(TeamMemberEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(TeamMemberEntity entity, CancellationToken ct);
}
