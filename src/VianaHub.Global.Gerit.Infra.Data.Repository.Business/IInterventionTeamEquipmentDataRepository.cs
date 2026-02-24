using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IInterventionTeamEquipmentDataRepository
{
    Task<IEnumerable<InterventionTeamEquipmentEntity>> GetAllAsync(CancellationToken ct);
    Task<InterventionTeamEquipmentEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPage<InterventionTeamEquipmentEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int tenantId, int interventionTeamId, int equipmentId, CancellationToken ct);
    Task<bool> AddAsync(InterventionTeamEquipmentEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(InterventionTeamEquipmentEntity entity, CancellationToken ct);
}