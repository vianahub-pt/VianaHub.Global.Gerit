using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace VianaHub.Global.Gerit.Infra.Data.Telemetry;

/// <summary>
/// Instrumentação de telemetria para operações de banco de dados
/// Segue padrões OpenTelemetry para observabilidade enterprise
/// </summary>
public static class DatabaseTelemetry
{
    // Activity Source para distributed tracing
    public static readonly ActivitySource ActivitySource = new(
        "VianaHub.Global.Gerit.Database",
        "1.0.0"
    );

    // Meter para métricas customizadas
    private static readonly Meter Meter = new(
        "VianaHub.Global.Gerit.Database",
        "1.0.0"
    );

    // ========== COUNTERS ==========

    /// <summary>
    /// Total de queries executadas
    /// </summary>
    public static readonly Counter<long> QueriesExecuted = Meter.CreateCounter<long>(
        "db.queries.executed",
        description: "Total de queries executadas no banco de dados"
    );

    /// <summary>
    /// Total de queries com erro
    /// </summary>
    public static readonly Counter<long> QueriesWithError = Meter.CreateCounter<long>(
        "db.queries.errors",
        description: "Total de queries que resultaram em erro"
    );

    /// <summary>
    /// Total de transações commitadas
    /// </summary>
    public static readonly Counter<long> TransactionsCommitted = Meter.CreateCounter<long>(
        "db.transactions.committed",
        description: "Total de transações commitadas com sucesso"
    );

    /// <summary>
    /// Total de transações revertidas
    /// </summary>
    public static readonly Counter<long> TransactionsRolledBack = Meter.CreateCounter<long>(
        "db.transactions.rolledback",
        description: "Total de transações revertidas"
    );

    /// <summary>
    /// Total de conexões abertas
    /// </summary>
    public static readonly Counter<long> ConnectionsOpened = Meter.CreateCounter<long>(
        "db.connections.opened",
        description: "Total de conexões de banco abertas"
    );

    /// <summary>
    /// Total de conexões fechadas
    /// </summary>
    public static readonly Counter<long> ConnectionsClosed = Meter.CreateCounter<long>(
        "db.connections.closed",
        description: "Total de conexões de banco fechadas"
    );

    // ========== HISTOGRAMS ==========

    /// <summary>
    /// Duração de queries em milissegundos
    /// </summary>
    public static readonly Histogram<double> QueryDuration = Meter.CreateHistogram<double>(
        "db.query.duration",
        unit: "ms",
        description: "Duração de execução de queries"
    );

    /// <summary>
    /// Duração de transações em milissegundos
    /// </summary>
    public static readonly Histogram<double> TransactionDuration = Meter.CreateHistogram<double>(
        "db.transaction.duration",
        unit: "ms",
        description: "Duração de execução de transações"
    );

    /// <summary>
    /// Duração de conexões em milissegundos
    /// </summary>
    public static readonly Histogram<double> ConnectionDuration = Meter.CreateHistogram<double>(
        "db.connection.duration",
        unit: "ms",
        description: "Duração de conexões de banco"
    );

    // ========== OBSERVABLE GAUGES ==========

    private static long _activeConnections = 0;
    private static long _activeTransactions = 0;

    /// <summary>
    /// Número de conexões ativas no momento
    /// </summary>
    public static readonly ObservableGauge<long> ActiveConnections = Meter.CreateObservableGauge(
        "db.connections.active",
        () => _activeConnections,
        description: "Número de conexões ativas no banco de dados"
    );

    /// <summary>
    /// Número de transações ativas no momento
    /// </summary>
    public static readonly ObservableGauge<long> ActiveTransactions = Meter.CreateObservableGauge(
        "db.transactions.active",
        () => _activeTransactions,
        description: "Número de transações ativas"
    );

    // ========== HELPER METHODS ==========

    /// <summary>
    /// Incrementa contador de conexões ativas
    /// </summary>
    public static void IncrementActiveConnections()
    {
        Interlocked.Increment(ref _activeConnections);
        ConnectionsOpened.Add(1);
    }

    /// <summary>
    /// Decrementa contador de conexões ativas
    /// </summary>
    public static void DecrementActiveConnections()
    {
        Interlocked.Decrement(ref _activeConnections);
        ConnectionsClosed.Add(1);
    }

    /// <summary>
    /// Incrementa contador de transações ativas
    /// </summary>
    public static void IncrementActiveTransactions()
    {
        Interlocked.Increment(ref _activeTransactions);
    }

    /// <summary>
    /// Decrementa contador de transações ativas
    /// </summary>
    public static void DecrementActiveTransactions()
    {
        Interlocked.Decrement(ref _activeTransactions);
    }

    /// <summary>
    /// Cria uma activity para tracing de operação de banco
    /// </summary>
    public static Activity? StartDatabaseActivity(
        string operation,
        string? tableName = null,
        string? queryType = null)
    {
        var activity = ActivitySource.StartActivity(
            $"db.{operation}",
            ActivityKind.Client
        );

        if (activity != null)
        {
            activity.SetTag("db.system", "sqlserver");
            activity.SetTag("db.operation", operation);

            if (!string.IsNullOrEmpty(tableName))
                activity.SetTag("db.table", tableName);

            if (!string.IsNullOrEmpty(queryType))
                activity.SetTag("db.query.type", queryType);
        }

        return activity;
    }

    /// <summary>
    /// Registra erro em uma activity
    /// </summary>
    public static void RecordError(Activity? activity, Exception exception)
    {
        if (activity != null)
        {
            activity.SetStatus(ActivityStatusCode.Error, exception.Message);
            activity.AddEvent(new ActivityEvent(
                "exception",
                tags: new ActivityTagsCollection
                {
                    { "exception.type", exception.GetType().FullName },
                    { "exception.message", exception.Message },
                    { "exception.stacktrace", exception.StackTrace }
                }
            ));
        }

        QueriesWithError.Add(1, new KeyValuePair<string, object?>("error.type", exception.GetType().Name));
    }
}
