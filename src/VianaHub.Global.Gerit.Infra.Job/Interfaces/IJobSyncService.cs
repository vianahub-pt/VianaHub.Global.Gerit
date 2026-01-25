namespace VianaHub.Global.Gerit.Infra.Job.Interfaces;

/// <summary>
/// Serviço responsável por sincronizar jobs do banco de dados com Hangfire
/// </summary>
public interface IJobSyncService
{
    /// <summary>
    /// Sincroniza jobs ativos do banco com Hangfire
    /// </summary>
    Task SyncJobsWithHangfire(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executa um job dinamicamente via reflexão
    /// </summary>
    Task ExecuteJob(string jobType, string jobMethod);
}
