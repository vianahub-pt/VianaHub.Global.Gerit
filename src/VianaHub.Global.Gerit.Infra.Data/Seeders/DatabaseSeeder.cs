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
            CreateSeed("INSTAGRAM", "Instagram", "Aquisicao via Instagram", 1),
            CreateSeed("FACEBOOK", "Facebook", "Aquisicao via Facebook", 1),
            CreateSeed("LINKEDIN", "LinkedIn", "Aquisicao via LinkedIn", 1),
            CreateSeed("GOOGLE", "Google", "Aquisicao via Google", 1),
            CreateSeed("WHATSAPP", "WhatsApp", "Aquisicao via WhatsApp", 1),
            CreateSeed("FRIENDS", "Amigos", "Indicacao de amigos", 1),
            CreateSeed("EVENTS", "Eventos", "Aquisicao em eventos", 1),
            CreateSeed("TELEVISION", "Televisao", "Aquisicao via televisao", 1),
            CreateSeed("RADIO", "Radio", "Aquisicao via radio", 1),
            CreateSeed("NEWSPAPER", "Jornal", "Aquisicao via jornal", 1),
            CreateSeed("MAGAZINE", "Revista", "Aquisicao via revista", 1),
            CreateSeed("OTHER", "Outro", "Outras formas de aquisicao", 1),
        };

        _context.Set<AcquisitionSourceTypeEntity>().AddRange(seeds);
        await _context.SaveChangesAsync(ct);
        _logger.LogInformation("Seed de AcquisitionSourceTypes concluido: {Count} registros.", seeds.Count);
    }

    /// <summary>
    /// Cria uma entidade AcquisitionSourceType com tradução pt-PT embutida para seed.
    /// </summary>
    private static AcquisitionSourceTypeEntity CreateSeed(string code, string name, string description, int createdBy)
    {
        var entity = new AcquisitionSourceTypeEntity(code, createdBy);
        entity.AddTranslation(new AcquisitionSourceTypeTranslationsEntity(0, "pt-PT", name, description));
        return entity;
    }
}
