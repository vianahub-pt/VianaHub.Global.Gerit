using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Infra.Data.Context;

/// <summary>
/// Implementação scoped do contexto de Tenant para o request atual.
/// Armazena o TenantId em memória durante o ciclo de vida do request HTTP,
/// permitindo que o TenantSessionConnectionInterceptor use o valor correto
/// mesmo em operações não autenticadas (ex: login, registro).
/// </summary>
public sealed class RequestTenantContext : IRequestTenantContext
{
    private int? _tenantId;

    public int? TenantId => _tenantId;

    public void SetTenantId(int tenantId) => _tenantId = tenantId;

    public void Clear() => _tenantId = null;
}
