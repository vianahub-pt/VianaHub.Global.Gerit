using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IInterventionTeamVehicleDataRepository
{
    Task<IEnumerable<InterventionTeamVehicleEntity>> GetAllAsync(CancellationToken ct);
    Task<InterventionTeamVehicleEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPage<InterventionTeamVehicleEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int tenantId, int interventionId, int vehicleId, CancellationToken ct);
    Task<bool> AddAsync(InterventionTeamVehicleEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(InterventionTeamVehicleEntity entity, CancellationToken ct);
}
