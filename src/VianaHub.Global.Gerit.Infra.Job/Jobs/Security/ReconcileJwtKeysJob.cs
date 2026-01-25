using Microsoft.Extensions.Logging;
using VianaHub.Global.Gerit.Domain.Interfaces.Domain;
using VianaHub.Global.Gerit.Domain.Interfaces.Repository;
using VianaHub.Global.Gerit.Infra.Job.Interfaces;

namespace VianaHub.Global.Gerit.Infra.Job.Jobs.Security;

/// <summary>
/// Job de reconciliação de chaves JWT para aplicações legadas.
/// Garante que todas as combinações Tenant + Application ativas tenham pelo menos uma chave JWT ativa.
/// Executa uma única vez na inicialização ou sob demanda.
/// </summary>
public class ReconcileJwtKeysJob : IJob
{
    private readonly IJwtKeyDomainService _jwtKeyDomain;
    private readonly ITenantDataRepository _tenantRepo;
    private readonly IJwtKeyDataRepository _jwtKeyRepo;
    private readonly ILogger<ReconcileJwtKeysJob> _logger;

    /// <summary>
    /// ID do sistema para operações automatizadas
    /// </summary>
    private static readonly int SystemUserId = 0;

    public ReconcileJwtKeysJob(
        IJwtKeyDomainService jwtKeyDomain,
        ITenantDataRepository tenantRepo,
        IJwtKeyDataRepository jwtKeyRepo,
        ILogger<ReconcileJwtKeysJob> logger)
    {
        _jwtKeyDomain = jwtKeyDomain;
        _tenantRepo = tenantRepo;
        _jwtKeyRepo = jwtKeyRepo;
        _logger = logger;
    }

    public async Task Execute(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("?? [ReconcileJwtKeysJob] Iniciando reconciliação de chaves JWT para aplicações legadas");

        var startTime = DateTime.UtcNow;
        var totalProcessed = 0;
        var totalCreated = 0;
        var totalSkipped = 0;
        var totalErrors = 0;

        // 1. Buscar todos os tenants ativos
        var tenants = await _tenantRepo.GetAllAsync(cancellationToken);
        var activeTenants = tenants.Where(t => t.IsActive && !t.IsDeleted).ToList();

        _logger.LogInformation("?? [ReconcileJwtKeysJob] Encontrados {TenantCount} tenants ativos para processar", activeTenants.Count);

        // 2. Processar tenants ativos
        foreach (var tenant in activeTenants)
        {
            var result = await ProcessTenantAsync(tenant.Id, cancellationToken);

            totalProcessed += result.Processed;
            totalCreated += result.Created;
            totalSkipped += result.Skipped;
            totalErrors += result.Errors;
        }

        var duration = DateTime.UtcNow - startTime;

        _logger.LogInformation(
            "? [ReconcileJwtKeysJob] Reconciliação concluída com sucesso. " +
            "Processados: {Processed}, Criados: {Created}, Pulados: {Skipped}, Erros: {Errors}, Duração: {Duration}s",
            totalProcessed, totalCreated, totalSkipped, totalErrors, duration.TotalSeconds);
    }

    /// <summary>
    /// Processa todas as aplicações de um tenant específico
    /// </summary>
    private async Task<ProcessingTenantResult> ProcessTenantAsync(int tenantId, CancellationToken cancellationToken)
    {
        var result = new ProcessingTenantResult();

        var allTenants = await _tenantRepo.GetAllAsync(cancellationToken);

        foreach (var tenant in allTenants)
        {
            result.Processed++;

            // Verificar se já existe chave ativa
            var existingKey = await _jwtKeyRepo.GetActiveKeyAsync(tenant.Id, cancellationToken);

            if (existingKey != null)
            {
                _logger.LogDebug(
                    "??  [ReconcileJwtKeysJob] Tenant {TenantId}, Application {ApplicationId} ({ApplicationName}): " +
                    "Chave JWT ativa já existe (KeyId: {KeyId}), pulando",
                    tenantId, tenant.Id, tenant.Name, existingKey.KeyId);
                result.Skipped++;
                continue;
            }

            // Criar chave JWT automaticamente
            _logger.LogInformation(
                "?? [ReconcileJwtKeysJob] Tenant {TenantId}, Application {ApplicationId} ({ApplicationName}): " +
                "Nenhuma chave JWT encontrada, criando automaticamente",
                tenantId, tenant.Id, tenant.Name);

            var newKey = await _jwtKeyDomain.EnsureKeyExistsAsync(tenant.Id, SystemUserId, cancellationToken);

            if (newKey != null)
            {
                _logger.LogInformation(
                    "? [ReconcileJwtKeysJob] Tenant {TenantId}, Application {ApplicationId} ({ApplicationName}): " +
                    "Chave JWT criada com sucesso (Id: {KeyId})", tenantId, tenant.Id, tenant.Name, newKey.KeyId);
                result.Created++;
            }
            else
            {
                _logger.LogWarning(
                    "??  [ReconcileJwtKeysJob] Tenant {TenantId}, Application {ApplicationId} ({ApplicationName}): " +
                    "Falha ao criar chave JWT (nenhum erro lançado, mas resultado nulo)",
                    tenantId, tenant.Id, tenant.Name);
                result.Errors++;
            }
        }

        return result;
    }

    /// <summary>
    /// Resultado do processamento de aplicações
    /// </summary>
    private class ProcessingTenantResult
    {
        public int Processed { get; set; }
        public int Created { get; set; }
        public int Skipped { get; set; }
        public int Errors { get; set; }
    }
}

