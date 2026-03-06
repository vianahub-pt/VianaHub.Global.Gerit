namespace VianaHub.Global.Gerit.Domain.Interfaces.Base;

/// <summary>
/// Contexto de Tenant para o request atual (scoped).
/// Permite que operações não autenticadas (ex: login) propaguem o TenantId
/// para o interceptor de conexão do EF Core, garantindo que o RLS funcione
/// corretamente mesmo quando não há JWT disponível.
/// </summary>
public interface IRequestTenantContext
{
    /// <summary>
    /// TenantId definido explicitamente para o request atual.
    /// Retorna null se nenhum tenant foi definido via SetTenantId.
    /// </summary>
    int? TenantId { get; }

    /// <summary>
    /// Define o TenantId para o request atual.
    /// Deve ser chamado antes de qualquer acesso ao banco de dados.
    /// </summary>
    void SetTenantId(int tenantId);

    /// <summary>
    /// Remove o TenantId definido para o request atual.
    /// </summary>
    void Clear();
}
