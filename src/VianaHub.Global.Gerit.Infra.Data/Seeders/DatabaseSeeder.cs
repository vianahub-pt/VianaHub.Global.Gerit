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
        await SeedConsentOriginTypesAsync(ct);
    }

    private async Task SeedAcquisitionSourceTypesAsync(CancellationToken ct)
    {
        var existing = _context.Set<AcquisitionSourceTypeEntity>().Any();
        if (existing)
        {
            _logger.LogInformation("AcquisitionSourceTypes já possui dados. Seed ignorado.");
            return;
        }

        var seeds = new List<AcquisitionSourceTypeEntity>
        {
            new("Instagram", "Aquisição via Instagram", 1),
            new("Facebook", "Aquisição via Facebook", 1),
            new("LinkedIn", "Aquisição via LinkedIn", 1),
            new("Google", "Aquisição via Google", 1),
            new("WhatsApp", "Aquisição via WhatsApp", 1),
            new("Amigos", "Indicação de amigos", 1),
            new("Eventos", "Aquisição em eventos", 1),
            new("Televisão", "Aquisição via televisão", 1),
            new("Rádio", "Aquisição via rádio", 1),
            new("Jornal", "Aquisição via jornal", 1),
            new("Revista", "Aquisição via revista", 1),
            new("Outro", "Outras formas de aquisição", 1),
        };

        _context.Set<AcquisitionSourceTypeEntity>().AddRange(seeds);
        await _context.SaveChangesAsync(ct);
        _logger.LogInformation("Seed de AcquisitionSourceTypes concluído: {Count} registros.", seeds.Count);
    }

    private async Task SeedConsentOriginTypesAsync(CancellationToken ct)
    {
        var existing = _context.Set<ConsentOriginTypeEntity>().Any();
        if (existing)
        {
            _logger.LogInformation("ConsentOriginTypes já possui dados. Seed ignorado.");
            return;
        }

        var seeds = new List<ConsentOriginTypeEntity>
        {
            new("Web", "Consentimento obtido via website", 1),
            new("Mobile", "Consentimento obtido via aplicativo móvel", 1),
            new("Papel", "Consentimento obtido em formulário físico", 1),
            new("API", "Consentimento obtido via integração de API", 1),
            new("Backoffice", "Consentimento registrado pelo backoffice", 1),
            new("E-mail", "Consentimento obtido via e-mail", 1),
            new("SMS", "Consentimento obtido via SMS", 1),
            new("WhatsApp", "Consentimento obtido via WhatsApp", 1),
            new("Call Center", "Consentimento obtido via call center", 1),
        };

        _context.Set<ConsentOriginTypeEntity>().AddRange(seeds);
        await _context.SaveChangesAsync(ct);
        _logger.LogInformation("Seed de ConsentOriginTypes concluído: {Count} registros.", seeds.Count);
    }
}
