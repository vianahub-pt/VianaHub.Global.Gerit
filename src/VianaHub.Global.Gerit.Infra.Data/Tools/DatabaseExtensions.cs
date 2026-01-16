using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VianaHub.Global.Gerit.Infra.Data.Context;
using VianaHub.Global.Gerit.Infra.Data.Seeders;

namespace VianaHub.Global.Gerit.Infra.Data.Tools;

/// <summary>
/// Extensões para configuração e inicialização do banco de dados
/// </summary>
public static class DatabaseExtensions
{
    /// <summary>
    /// Aplica migrations automaticamente (apenas Dev/Staging)
    /// </summary>
    public static async Task MigrateDatabaseAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<GeritDbContext>>();

        try
        {
            logger.LogInformation("?? Verificando estado do banco de dados...");

            var context = services.GetRequiredService<GeritDbContext>();

            // Verifica se o banco existe
            var canConnect = await context.Database.CanConnectAsync();
            if (!canConnect)
            {
                logger.LogInformation("?? Banco de dados não existe, criando...");
                await context.Database.MigrateAsync();
                logger.LogInformation("? Banco de dados criado com sucesso");
                return;
            }

            // Verifica migrations aplicadas
            var appliedMigrations = await context.Database.GetAppliedMigrationsAsync();
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

            logger.LogInformation("??  Migrations aplicadas: {Count}", appliedMigrations.Count());

            if (pendingMigrations.Any())
            {
                logger.LogWarning("??  Migrations pendentes encontradas: {Migrations}",
                    string.Join(", ", pendingMigrations));

                logger.LogInformation("?? Aplicando migrations pendentes...");
                await context.Database.MigrateAsync();
                logger.LogInformation("? Migrations aplicadas com sucesso");
            }
            else
            {
                logger.LogInformation("?  Banco de dados está atualizado, nenhuma migration pendente");
            }
        }
        catch (Microsoft.Data.SqlClient.SqlException sqlEx) when (sqlEx.Message.Contains("already an object named") ||
                                                                    sqlEx.Message.Contains("does not exist or you do not have permission"))
        {
            logger.LogError(sqlEx, "? Erro de conflito de schema ao aplicar migrations");
            logger.LogWarning("???????????????????????????????????????????????????????????????");
            logger.LogWarning("?? PROBLEMA DETECTADO: Conflito de schema do banco de dados");
            logger.LogWarning("???????????????????????????????????????????????????????????????");
            logger.LogWarning("");
            logger.LogWarning("?? OPÇÕES DE SOLUÇÃO:");
            logger.LogWarning("");
            logger.LogWarning("1??  OPÇÃO 1 - Reset Completo (Desenvolvimento):");
            logger.LogWarning("   • Deletar o banco de dados completamente");
            logger.LogWarning("   • Reiniciar a aplicação (migrations serão aplicadas do zero)");
            logger.LogWarning("");
            logger.LogWarning("2??  OPÇÃO 2 - Marcar migrations como aplicadas:");
            logger.LogWarning("   • Executar no SQL Server:");
            logger.LogWarning("   INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])");
            logger.LogWarning("   VALUES ('20251230180000_AddApplicationIdToUserSessions', '8.0.0');");
            logger.LogWarning("   • Reiniciar a aplicação");
            logger.LogWarning("");
            logger.LogWarning("3??  OPÇÃO 3 - Desabilitar migrations automáticas:");
            logger.LogWarning("   • No appsettings.json, definir:");
            logger.LogWarning("   \"Database\": {{ \"AutoMigrate\": false }}");
            logger.LogWarning("   • Aplicar migrations manualmente via dotnet ef");
            logger.LogWarning("");
            logger.LogWarning("???????????????????????????????????????????????????????????????");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "? Erro inesperado ao aplicar migrations");
            throw;
        }
    }

    /// <summary>
    /// Executa o seeding de dados iniciais
    /// </summary>
    public static async Task SeedDatabaseAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<DatabaseSeeder>>();

        try
        {
            var context = services.GetRequiredService<GeritDbContext>();
            var seeder = new DatabaseSeeder(context, logger);

            await seeder.SeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "? Erro ao executar seeding do banco");
            throw;
        }
    }

    /// <summary>
    /// Inicializa o banco de dados (migrations + seeding)
    /// </summary>
    public static async Task InitializeDatabaseAsync(this IHost host, bool applyMigrations = false)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<GeritDbContext>>();

        try
        {
            logger.LogInformation("?? Inicializando banco de dados...");

            if (applyMigrations)
            {
                await host.MigrateDatabaseAsync();
            }
            else
            {
                logger.LogInformation("??  Migrations automáticas desabilitadas, pulando...");
            }

            await host.SeedDatabaseAsync();

            logger.LogInformation("? Banco de dados inicializado com sucesso");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "? Erro ao inicializar banco de dados");
            throw;
        }
    }

    /// <summary>
    /// Verifica a saúde da conexão do banco de dados
    /// </summary>
    public static async Task<bool> CanConnectAsync(this DbContext context, CancellationToken ct = default)
    {
        try
        {
            return await context.Database.CanConnectAsync(ct);
        }
        catch
        {
            return false;
        }
    }
}
