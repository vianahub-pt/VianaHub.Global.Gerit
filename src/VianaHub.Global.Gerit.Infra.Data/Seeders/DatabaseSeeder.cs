using Microsoft.Extensions.Logging;
using VianaHub.Global.Gerit.Domain.Entities.Business;
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
        await SeedAcquisitionSourceTypesAsync(ct);
    }

    private async Task SeedAcquisitionSourceTypesAsync(CancellationToken ct)
    {
        var existing = _context.Set<AcquisitionSourceTypeEntity>().Any();
        if (existing)
        {
            _logger.LogInformation("AcquisitionSourceTypes ja possui dados. Seed ignorado.");
            return;
        }

        var seeds = new List<AcquisitionSourceTypeEntity>
        {
            new("INSTAGRAM", "Instagram", "Aquisicao via Instagram", 1),
            new("FACEBOOK", "Facebook", "Aquisicao via Facebook", 1),
            new("LINKEDIN", "LinkedIn", "Aquisicao via LinkedIn", 1),
            new("GOOGLE", "Google", "Aquisicao via Google", 1),
            new("WHATSAPP", "WhatsApp", "Aquisicao via WhatsApp", 1),
            new("FRIENDS", "Amigos", "Indicacao de amigos", 1),
            new("EVENTS", "Eventos", "Aquisicao em eventos", 1),
            new("TELEVISION", "Televisao", "Aquisicao via televisao", 1),
            new("RADIO", "Radio", "Aquisicao via radio", 1),
            new("NEWSPAPER", "Jornal", "Aquisicao via jornal", 1),
            new("MAGAZINE", "Revista", "Aquisicao via revista", 1),
            new("OTHER", "Outro", "Outras formas de aquisicao", 1),
        };

        _context.Set<AcquisitionSourceTypeEntity>().AddRange(seeds);
        await _context.SaveChangesAsync(ct);
        _logger.LogInformation("Seed de AcquisitionSourceTypes concluido: {Count} registros.", seeds.Count);
    }
}
