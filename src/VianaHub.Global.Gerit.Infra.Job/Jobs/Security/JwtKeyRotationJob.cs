using Hangfire;
using Microsoft.Extensions.Logging;
using VianaHub.Global.Gerit.Domain.Interfaces.Domain;
using VianaHub.Global.Gerit.Infra.Job.Interfaces;
using VianaHub.Global.Gerit.Domain.Interfaces.Repository;

namespace VianaHub.Global.Gerit.Infra.Job.Jobs.Security;

/// <summary>
/// Job de rotação automática de chaves JWT
/// Executado diariamente às 03:00 UTC
/// </summary>
public class JwtKeyRotationJob : IJob
{
    private readonly IJwtKeyDomainService _jwtKeyService;
    private readonly ITenantDataRepository _tenantRepo;
    private readonly ILogger<JwtKeyRotationJob> _logger;

    /// <summary>
    /// ID do sistema para operações automatizadas
    /// </summary>
    private static readonly int SystemUserId = 0;

    public JwtKeyRotationJob(
        IJwtKeyDomainService jwtKeyService,
        ITenantDataRepository tenantRepo,
        ILogger<JwtKeyRotationJob> logger)
    {
        _jwtKeyService = jwtKeyService;
        _tenantRepo = tenantRepo;
        _logger = logger;
    }

    public async Task Execute(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("?? [JwtKeyRotationJob] Iniciando rotação automática de chaves JWT");

        try
        {
            // 0. Garantir que cada tenant ativo possua uma chave ativa (reconciliação mínima)
            var tenants = await _tenantRepo.GetAllAsync(cancellationToken);
            var activeTenants = tenants.Where(t => t.IsActive && !t.IsDeleted).ToList();

            _logger.LogInformation("?? [JwtKeyRotationJob] Verificando existência de chaves para {Count} tenants ativos", activeTenants.Count);

            var createdCount = 0;
            foreach (var tenant in activeTenants)
            {
                if (cancellationToken.IsCancellationRequested) break;

                try
                {
                    var existing = await _jwtKeyService.GetActiveKeyAsync(tenant.Id, cancellationToken);
                    if (existing != null)
                    {
                        _logger.LogDebug("?? [JwtKeyRotationJob] Tenant {TenantId} já possui chave ativa (KeyId={KeyId})", tenant.Id, existing.KeyId);
                        continue;
                    }

                    var newKey = await _jwtKeyService.EnsureKeyExistsAsync(tenant.Id, SystemUserId, cancellationToken);
                    if (newKey != null)
                    {
                        createdCount++;
                        _logger.LogInformation("? [JwtKeyRotationJob] Tenant {TenantId}: chave JWT criada (KeyId={KeyId})", tenant.Id, newKey.KeyId);
                    }
                    else
                    {
                        _logger.LogError("?? [JwtKeyRotationJob] Tenant {TenantId}: falha ao criar chave JWT", tenant.Id);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "? [JwtKeyRotationJob] Erro ao garantir chave para Tenant {TenantId}", tenant.Id);
                }
            }

            if (createdCount > 0)
            {
                _logger.LogInformation("? [JwtKeyRotationJob] Reconciliação de chaves finalizada. Chaves criadas: {CreatedCount}", createdCount);
            }

            // 1. Executar rotação para chaves elegíveis
            var rotatedCount = await _jwtKeyService.RotateKeysAsync(cancellationToken);

            if (rotatedCount > 0)
            {
                _logger.LogInformation(
                    "? [JwtKeyRotationJob] Rotação concluída com sucesso. Chaves rotacionadas: {RotatedCount}",
                    rotatedCount);
            }
            else
            {
                _logger.LogInformation(
                    "?? [JwtKeyRotationJob] Nenhuma chave elegível para rotação encontrada");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "? [JwtKeyRotationJob] Erro durante rotação de chaves JWT");
            throw;
        }
    }
}