using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace VianaHub.Global.Gerit.Infra.Data.Context;

/// <summary>
/// Factory para criar o DbContext em design-time (usado pelas migrations)
/// </summary>
public class GeritDbContextFactory : IDesignTimeDbContextFactory<GeritDbContext>
{
    public GeritDbContext CreateDbContext(string[] args)
    {
        // Configuração básica para design-time
        var optionsBuilder = new DbContextOptionsBuilder<GeritDbContext>();
        
        // Connection string para design-time (ajuste conforme necessário)
        // Você pode usar uma connection string de desenvolvimento ou obter de appsettings
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=GeritDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        
        // Retorna o contexto sem TenantId (para migrations)
        return new GeritDbContext(optionsBuilder.Options);
    }
}
