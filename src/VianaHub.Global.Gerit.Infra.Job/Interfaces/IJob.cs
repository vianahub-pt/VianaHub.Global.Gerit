namespace VianaHub.Global.Gerit.Infra.Job.Interfaces;

public interface IJob
{
    /// <summary>
    /// Executa o job
    /// </summary>
    Task Execute(CancellationToken cancellationToken = default);
}
