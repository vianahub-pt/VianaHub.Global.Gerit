using Hangfire;
using Microsoft.Extensions.Logging;
using VianaHub.Global.Gerit.Domain.Interfaces.Identity;
using VianaHub.Global.Gerit.Infra.Job.Interfaces;

namespace VianaHub.Global.Gerit.Infra.Job.Jobs.Cleanup;

/// <summary>
/// Job de limpeza de chaves JWT expiradas
/// Executado semanalmente aos domingos às 04:00 UTC
/// </summary>
public class CleanupExpiredJwtKeysJob : IJob
{
    private readonly IJwtKeyDomainService _jwtKeyService;
    private readonly ILogger<CleanupExpiredJwtKeysJob> _logger;
    private const int RetentionDays = 90; // Período de retenção padrão

    public CleanupExpiredJwtKeysJob(
        IJwtKeyDomainService jwtKeyService,
        ILogger<CleanupExpiredJwtKeysJob> logger)
    {
        _jwtKeyService = jwtKeyService;
        _logger = logger;
    }

    public async Task Execute(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("?? [CleanupExpiredJwtKeysJob] Iniciando limpeza de chaves JWT expiradas");

        try
        {
            var deletedCount = await _jwtKeyService.CleanupExpiredKeysAsync(RetentionDays, cancellationToken);

            if (deletedCount > 0)
            {
                _logger.LogInformation(
                    "? [CleanupExpiredJwtKeysJob] Limpeza concluída com sucesso. Chaves removidas: {DeletedCount}",
                    deletedCount);
            }
            else
            {
                _logger.LogInformation(
                    "?? [CleanupExpiredJwtKeysJob] Nenhuma chave expirada encontrada para limpeza");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "? [CleanupExpiredJwtKeysJob] Erro durante limpeza de chaves JWT");
            throw;
        }
    }
}
