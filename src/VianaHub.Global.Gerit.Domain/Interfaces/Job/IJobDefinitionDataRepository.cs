using VianaHub.Global.Gerit.Domain.Entities.Job;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Job;

public interface IJobDefinitionDataRepository
{
    Task<JobDefinitionEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<JobDefinitionEntity> GetByNameAsync(string jobName, CancellationToken ct);
    Task<IEnumerable<JobDefinitionEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<JobDefinitionEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct);
    Task<bool> ExistsByNameAsync(string jobName, CancellationToken ct);
    Task<bool> CreateAsync(JobDefinitionEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(JobDefinitionEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(JobDefinitionEntity entity, CancellationToken ct);
}
