using Microsoft.Extensions.Logging;
using VianaHub.Global.Gerit.Infra.Data.Context;

namespace VianaHub.Global.Gerit.Infra.Data.Seeders;

public class DatabaseSeeder
{
    private readonly GeritDbContext _context;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(GeritDbContext context, ILogger<DatabaseSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken ct = default)
    {

    }
}
