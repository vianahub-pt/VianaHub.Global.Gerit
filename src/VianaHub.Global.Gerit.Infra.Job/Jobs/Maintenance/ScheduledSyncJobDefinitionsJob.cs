using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VianaHub.Global.Gerit.Infra.Job.Interfaces;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Infra.Job.Jobs.Maintenance;

/// <summary>
/// Job configurável via JobDefinition que dispara a sincronização de JobDefinitions com o Hangfire.
/// Pode ser ativado/desativado e agendado através de uma definição no banco (JobDefinition).
/// Todas as mensagens são lidas via ILocalizationService para suportar multi-idiomas.
/// </summary>
public class ScheduledSyncJobDefinitionsJob : IJob
{
    private readonly IJobSyncService _jobSyncService;
    private readonly ILogger<ScheduledSyncJobDefinitionsJob> _logger;
    private readonly ILocalizationService _localizer;

    public ScheduledSyncJobDefinitionsJob(
        IJobSyncService jobSyncService,
        ILogger<ScheduledSyncJobDefinitionsJob> logger,
        ILocalizationService localizer)
    {
        _jobSyncService = jobSyncService;
        _logger = logger;
        _localizer = localizer;
    }

    public async Task Execute(CancellationToken cancellationToken = default)
    {
        var starting = _localizer.GetMessage("Job.SyncDefinitions.Starting");
        var completed = _localizer.GetMessage("Job.SyncDefinitions.Completed");
        var cancelled = _localizer.GetMessage("Job.SyncDefinitions.Cancelled");
        var error = _localizer.GetMessage("Job.SyncDefinitions.Error");

        _logger.LogInformation("[{Job}] {Message}", nameof(ScheduledSyncJobDefinitionsJob), starting);

        try
        {
            await _jobSyncService.SyncJobsWithHangfire(cancellationToken);
            _logger.LogInformation("[{Job}] {Message}", nameof(ScheduledSyncJobDefinitionsJob), completed);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("[{Job}] {Message}", nameof(ScheduledSyncJobDefinitionsJob), cancelled);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "[{Job}] {Message}", nameof(ScheduledSyncJobDefinitionsJob), error);
            throw; // rethrow to let Hangfire / scheduler handle retries if configured
        }
    }
}
