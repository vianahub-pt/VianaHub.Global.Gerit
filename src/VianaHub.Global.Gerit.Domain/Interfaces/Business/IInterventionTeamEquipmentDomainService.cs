using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IInterventionTeamEquipmentDomainService
{
    Task<IEnumerable<InterventionTeamEquipmentEntity>> GetAllAsync(CancellationToken ct);
    Task<InterventionTeamEquipmentEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPage<InterventionTeamEquipmentEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int tenantId, int interventionTeamId, int equipmentId, CancellationToken ct);
    Task<bool> CreateAsync(InterventionTeamEquipmentEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(InterventionTeamEquipmentEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(InterventionTeamEquipmentEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(InterventionTeamEquipmentEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(InterventionTeamEquipmentEntity entity, CancellationToken ct);
}
