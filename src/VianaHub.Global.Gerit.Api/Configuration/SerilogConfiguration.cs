using Serilog;
using Serilog.Events;

namespace VianaHub.Global.Gerit.Api.Configuration;

/// <summary>
/// Configuração centralizada do Serilog para logging estruturado e contextualizado.
/// </summary>
public static class SerilogConfiguration
{
    public static void ConfigureSerilog(this WebApplicationBuilder builder)
    {
        // Remove o provider padrão do ASP.NET Core
        builder.Logging.ClearProviders();

        // Configura o Serilog
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .Enrich.WithCorrelationId()
            .Enrich.WithProperty("Application", "Gerit")
            .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Code)
            .WriteTo.File(
                path: "logs/gerit-.log",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30,
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                shared: true)
            .CreateLogger();

        // Adiciona o Serilog ao pipeline de logging do ASP.NET Core
        builder.Host.UseSerilog();

        Log.Information("=== Aplicação Gerit iniciando ===");
        Log.Information("Ambiente: {Environment}", builder.Environment.EnvironmentName);
        Log.Information("Versão .NET: {DotNetVersion}", Environment.Version);
    }

    public static void UseSerilogRequestLogging(this WebApplication app)
    {
        // Adiciona logging contextualizado de requisições HTTP
        app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} respondeu {StatusCode} em {Elapsed:0.0000}ms";
            options.GetLevel = (httpContext, elapsed, ex) =>
            {
                if (ex != null) return LogEventLevel.Error;
                if (httpContext.Response.StatusCode >= 500) return LogEventLevel.Error;
                if (httpContext.Response.StatusCode >= 400) return LogEventLevel.Warning;
                return LogEventLevel.Information;
            };

            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
                diagnosticContext.Set("RemoteIP", httpContext.Connection.RemoteIpAddress?.ToString());

                if (httpContext.User.Identity?.IsAuthenticated == true)
                {
                    diagnosticContext.Set("UserName", httpContext.User.Identity.Name);
                }
            };
        });
    }
}
