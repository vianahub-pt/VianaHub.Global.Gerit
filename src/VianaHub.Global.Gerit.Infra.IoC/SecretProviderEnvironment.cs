using Microsoft.Extensions.Configuration;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Infra.IoC;

public class SecretProviderEnvironment : ISecretProvider
{
    private readonly IConfiguration _configuration;

    public SecretProviderEnvironment(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string? GetMasterKey()
    {
        // 1) Prioriza variável de ambiente JWT_MASTER_KEY
        var env = Environment.GetEnvironmentVariable("JWT_MASTER_KEY");
        if (!string.IsNullOrWhiteSpace(env))
            return env.Trim();

        // 2) Suporte a Docker secrets: verificar arquivo padrão ou caminho definido em JWT_MASTER_KEY_FILE
        var secretFilePath = Environment.GetEnvironmentVariable("JWT_MASTER_KEY_FILE") ?? "/run/secrets/JWT_MASTER_KEY";
        try
        {
            if (!string.IsNullOrWhiteSpace(secretFilePath) && File.Exists(secretFilePath))
            {
                var fileContent = File.ReadAllText(secretFilePath).Trim();
                if (!string.IsNullOrWhiteSpace(fileContent))
                    return fileContent;
            }
        }
        catch
        {
            // Não throw; falha aqui será tratada pelo chamador (notificação/log)
        }

        // 3) Fallback para configurações no appsettings - suportar as chaves utilizadas no projeto
        // Priorizar chaves bem conhecidas:
        // - JwtKeyManagement:EncryptionKey (usado em appsettings.Development.json)
        // - Jwt:MasterKey (possível alternativa)
        // - JWT_MASTER_KEY (fallback de configuração direta)
        var cfg = _configuration["JwtKeyManagement:EncryptionKey"]
                  ?? _configuration["Jwt:MasterKey"]
                  ?? _configuration["JWT_MASTER_KEY"];

        if (!string.IsNullOrWhiteSpace(cfg))
            return cfg.Trim();

        return null;
    }
}
