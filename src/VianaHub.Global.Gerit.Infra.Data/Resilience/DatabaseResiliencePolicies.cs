using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Timeout;

namespace VianaHub.Global.Gerit.Infra.Data.Resilience;

/// <summary>
/// Políticas de resiliência para operações de banco de dados
/// Implementa retry, circuit breaker e timeout patterns
/// </summary>
public static class DatabaseResiliencePolicies
{
    /// <summary>
    /// Política de retry exponencial para operações transientes
    /// </summary>
    public static IAsyncPolicy CreateRetryPolicy(ILogger logger)
    {
        return Policy
            .Handle<DbUpdateException>()
            .Or<TimeoutException>()
            .Or<InvalidOperationException>(ex =>
                ex.Message.Contains("connection", StringComparison.OrdinalIgnoreCase))
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    logger.LogWarning(
                        exception,
                        "??  Tentativa {RetryCount} após {Delay}s devido a erro transiente: {ErrorMessage}",
                        retryCount,
                        timeSpan.TotalSeconds,
                        exception.Message
                    );
                }
            );
    }

    /// <summary>
    /// Política de timeout para prevenir operações penduradas
    /// </summary>
    public static IAsyncPolicy CreateTimeoutPolicy(ILogger logger, int timeoutSeconds = 30)
    {
        return Policy
            .TimeoutAsync(
                TimeSpan.FromSeconds(timeoutSeconds),
                TimeoutStrategy.Pessimistic,
                onTimeoutAsync: (context, timespan, task) =>
                {
                    logger.LogError(
                        "??  Timeout de {Timeout}s excedido para operação de banco",
                        timespan.TotalSeconds
                    );
                    return Task.CompletedTask;
                }
            );
    }

    /// <summary>
    /// Política combinada (wrap) de todas as políticas de resiliência
    /// Ordem: Timeout -> Retry
    /// </summary>
    public static IAsyncPolicy CreateCombinedPolicy(ILogger logger)
    {
        var timeoutPolicy = CreateTimeoutPolicy(logger, 30);
        var retryPolicy = CreateRetryPolicy(logger);

        // Wrap policies - ordem importa!
        return Policy.WrapAsync(timeoutPolicy, retryPolicy);
    }

    /// <summary>
    /// Executa uma operação de banco com retry automático
    /// </summary>
    public static async Task<T> ExecuteWithRetryAsync<T>(
        Func<Task<T>> operation,
        ILogger logger,
        CancellationToken ct = default)
    {
        var policy = CreateRetryPolicy(logger);
        return await policy.ExecuteAsync(async () => await operation());
    }

    /// <summary>
    /// Executa uma operação de banco com todas as políticas de resiliência
    /// </summary>
    public static async Task<T> ExecuteWithResilienceAsync<T>(
        Func<Task<T>> operation,
        ILogger logger,
        CancellationToken ct = default)
    {
        var policy = CreateCombinedPolicy(logger);
        return await policy.ExecuteAsync(async () => await operation());
    }

    /// <summary>
    /// Executa uma operação void com retry automático
    /// </summary>
    public static async Task ExecuteWithRetryAsync(
        Func<Task> operation,
        ILogger logger,
        CancellationToken ct = default)
    {
        var policy = CreateRetryPolicy(logger);
        await policy.ExecuteAsync(async () => await operation());
    }

    /// <summary>
    /// Executa uma operação void com todas as políticas de resiliência
    /// </summary>
    public static async Task ExecuteWithResilienceAsync(
        Func<Task> operation,
        ILogger logger,
        CancellationToken ct = default)
    {
        var policy = CreateCombinedPolicy(logger);
        await policy.ExecuteAsync(async () => await operation());
    }
}
