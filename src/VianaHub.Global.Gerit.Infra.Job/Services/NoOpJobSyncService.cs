using Microsoft.Extensions.Logging;
using VianaHub.Global.Gerit.Infra.Job.Interfaces;

namespace VianaHub.Global.Gerit.Infra.Job.Services;

/// <summary>
/// Implementação mínima de IJobSyncService que evita falha de resolução no startup.
/// Mantém logs e pode ser substituída por uma implementação completa posteriormente.
/// </summary>
public class NoOpJobSyncService : IJobSyncService
{
    private readonly ILogger<NoOpJobSyncService> _logger;

    public NoOpJobSyncService(ILogger<NoOpJobSyncService> logger)
    {
        _logger = logger;
    }

    public Task ExecuteJob(string jobType, string jobMethod)
    {
        _logger.LogInformation("NoOpJobSyncService.ExecuteJob called for {JobType}.{JobMethod}. No action performed.", jobType, jobMethod);
        return Task.CompletedTask;
    }

    public Task SyncJobsWithHangfire(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("NoOpJobSyncService.SyncJobsWithHangfire called. No synchronization performed.");
        return Task.CompletedTask;
    }
}
