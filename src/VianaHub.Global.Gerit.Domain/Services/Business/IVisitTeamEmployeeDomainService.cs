using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IVisitTeamEmployeeDomainService
{
    Task<bool> CreateAsync(VisitTeamEmployeeEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(VisitTeamEmployeeEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(VisitTeamEmployeeEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(VisitTeamEmployeeEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(VisitTeamEmployeeEntity entity, CancellationToken ct);
}
