using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace VianaHub.Global.Gerit.Infra.Data.Context;

/// <summary>
/// Contexto principal do Entity Framework Core para a aplicação Gerit.
/// Gerencia todas as entidades e configurações de mapeamento do banco de dados.
/// </summary>
public class GeritDbContext : DbContext
{
    private readonly ILogger<GeritDbContext>? _logger;

    public GeritDbContext(DbContextOptions<GeritDbContext> options) : base(options)
    {
    }

    public GeritDbContext(DbContextOptions<GeritDbContext> options, ILogger<GeritDbContext> logger)
        : base(options)
    {
        _logger = logger;
    }

    // DbSets serão adicionados aqui conforme as entidades forem criadas
    // Exemplo: public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica todas as configurações de mapeamento do assembly atual
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GeritDbContext).Assembly);

        // Convenções globais
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Remove o plural das tabelas (se aplicável)
            var tableName = entity.GetTableName();
            if (!string.IsNullOrEmpty(tableName))
            {
                entity.SetTableName(tableName);
            }

            // Define DeleteBehavior.Restrict para todas as FKs por padrão
            foreach (var relationship in entity.GetForeignKeys())
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        _logger?.LogInformation("Configurações do modelo EF Core aplicadas com sucesso");
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Audit timestamps (se implementar entidade base com CreatedAt/UpdatedAt)
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Properties.Any(p => p.Metadata.Name == "CreatedAt"))
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                }
            }

            if (entry.State == EntityState.Modified)
            {
                if (entry.Properties.Any(p => p.Metadata.Name == "UpdatedAt"))
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }
            }
        }

        _logger?.LogDebug("Salvando {Count} alterações no banco de dados", entries.Count());

        try
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            _logger?.LogInformation("Alterações salvas com sucesso: {Count} registros afetados", result);
            return result;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Erro ao salvar alterações no banco de dados");
            throw;
        }
    }
}