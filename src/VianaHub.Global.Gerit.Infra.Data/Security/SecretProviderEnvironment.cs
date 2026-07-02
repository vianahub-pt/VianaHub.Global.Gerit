using Microsoft.Extensions.Configuration;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Infra.Data.Security;

public class SecretProviderEnvironment : ISecretProvider
{
    private readonly IConfiguration _configuration;

    public SecretProviderEnvironment(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string? GetMasterKey()
    {
        // Ler apenas de configuração (appsettings / providers adicionados ao IConfiguration)
        // Prioridade: JwtKeyManagement:EncryptionKey, Jwt:MasterKey, fallback JWT_MASTER_KEY
        var cfg = _configuration["JwtKeyManagement:EncryptionKey"]
                  ?? _configuration["Jwt:MasterKey"]
                  ?? _configuration["JWT_MASTER_KEY"];

        if (!string.IsNullOrWhiteSpace(cfg))
            return cfg.Trim();

        return null;
    }
}
