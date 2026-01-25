using System;
using System.Threading;

namespace VianaHub.Global.Gerit.Infra.Job.Services;

/// <summary>
/// Contexto ambiente para execução de background jobs. Expor informações como JobName e ExecutionId
/// para interceptors, logs e outros componentes durante a execução do job.
/// </summary>
public static class JobContext
{
    private sealed class Holder
    {
        public string? JobName { get; set; }
        public string? ExecutionId { get; set; }
    }

    private static readonly AsyncLocal<Holder?> _current = new();

    /// <summary>
    /// Entra num escopo de job em background, retornando IDisposable que restaura o contexto anterior ao dispose.
    /// </summary>
    public static IDisposable EnterBackgroundJobScope(string? jobName, string? executionId)
    {
        var previous = _current.Value;
        _current.Value = new Holder { JobName = jobName, ExecutionId = executionId };
        return new Disposable(() => _current.Value = previous);
    }

    public static string? CurrentJobName => _current.Value?.JobName;
    public static string? CurrentExecutionId => _current.Value?.ExecutionId;
    public static bool IsInBackgroundJob => _current.Value != null;

    private sealed class Disposable : IDisposable
    {
        private readonly Action _onDispose;
        private bool _disposed;

        public Disposable(Action onDispose)
        {
            _onDispose = onDispose ?? throw new ArgumentNullException(nameof(onDispose));
        }

        public void Dispose()
        {
            if (_disposed) return;
            _onDispose();
            _disposed = true;
        }
    }
}
