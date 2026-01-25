using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using VianaHub.Global.Gerit.Infra.Job.Interfaces;

namespace VianaHub.Global.Gerit.Infra.Job.HostedServices;

/// <summary>
/// Serviço hospedado que sincroniza jobs na inicialização da aplicação
/// </summary>
public class JobSyncHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<JobSyncHostedService> _logger;

    public JobSyncHostedService(
        IServiceProvider serviceProvider,
        ILogger<JobSyncHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("JobSyncHostedService starting...");

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var jobSyncService = scope.ServiceProvider.GetRequiredService<IJobSyncService>();

            await jobSyncService.SyncJobsWithHangfire(cancellationToken);

            _logger.LogInformation("JobSyncHostedService completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in JobSyncHostedService");
        }

        return;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("JobSyncHostedService stopping...");
        return Task.CompletedTask;
    }
}
