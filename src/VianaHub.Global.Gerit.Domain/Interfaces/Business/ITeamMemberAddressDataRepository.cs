using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de repositório de dados para TeamMemberAddress
/// </summary>
public interface ITeamMemberAddressDataRepository
{
    Task<TeamMemberAddressEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<TeamMemberAddressEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<TeamMemberAddressEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByTeamMemberAndAddressAsync(int tenantId, int teamMemberId, string street, string city, string postalCode, CancellationToken ct);
    Task<TeamMemberAddressEntity> GetPrimaryAddressByTeamMemberAsync(int teamMemberId, CancellationToken ct);
    Task<bool> AddAsync(TeamMemberAddressEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(TeamMemberAddressEntity entity, CancellationToken ct);
}
