namespace VianaHub.Global.Gerit.Domain.Interfaces;

public interface ISecretProvider
{
    /// <summary>
    /// Recupera a chave mestra utilizada para criptografar/descriptografar chaves privadas JWT.
    /// Deve ser gerenciada externamente (Key Vault, KMS, variável de ambiente, etc.).
    /// </summary>
    /// <returns>Chave mestra em texto simples ou null se não disponível.</returns>
    string? GetMasterKey();
}
