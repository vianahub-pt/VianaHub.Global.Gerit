using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de serviço de domínio para TeamMemberAddress
/// </summary>
public interface ITeamMemberAddressDomainService
{
    Task<TeamMemberAddressEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<TeamMemberAddressEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<TeamMemberAddressEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);

    Task<bool> CreateAsync(TeamMemberAddressEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(TeamMemberAddressEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(TeamMemberAddressEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(TeamMemberAddressEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(TeamMemberAddressEntity entity, CancellationToken ct);
}
