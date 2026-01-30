using System.Threading.Tasks;
using VianaHub.Global.Gerit.Domain.Entities.Job;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Job;

/// <summary>
/// Abstração para agendamento e execução de jobs (adapter para Hangfire ou outros sistemas).
/// </summary>
public interface IJobSchedulerService
{
    Task RegisterRecurringAsync(JobDefinitionEntity jobDef);

    Task RemoveRecurringAsync(string jobName);

    Task<string> EnqueueJobAsync(JobDefinitionEntity jobDef);
}
