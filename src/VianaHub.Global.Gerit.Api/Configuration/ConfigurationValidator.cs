namespace VianaHub.Global.Gerit.Api.Configuration;

/// <summary>
/// Validador de configurações críticas da aplicação
/// Garante que todas as configurações necessárias estão presentes antes da inicialização
/// </summary>
public static class ConfigurationValidator
{
    /// <summary>
    /// Valida todas as configurações críticas na inicialização
    /// </summary>
    public static void ValidateConfiguration(IConfiguration configuration, IWebHostEnvironment environment)
    {
        var errors = new List<string>();

        // Valida Connection Strings
        ValidateConnectionStrings(configuration, errors);

        // Valida JWT Settings
        ValidateJwtSettings(configuration, errors);

        // Valida Rate Limiting (se habilitado)
        ValidateRateLimiting(configuration, errors);

        // Valida CORS (se habilitado)
        ValidateCors(configuration, environment, errors);

        // Se houver erros, lança exceção com chaves de mensagem para permitir tradução posterior
        if (errors.Any())
        {
            var errorMessage = string.Join("; ", errors);
            throw new InvalidOperationException(errorMessage);
        }
    }

    private static void ValidateConnectionStrings(IConfiguration configuration, List<string> errors)
    {
        var defaultConnection = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(defaultConnection))
        {
            // Key para mensagem localizada: Config.ConnectionStrings.DefaultConnectionMissing
            errors.Add("Config.ConnectionStrings.DefaultConnectionMissing");
        }
    }

    private static void ValidateJwtSettings(IConfiguration configuration, List<string> errors)
    {
        var encryptionKey = configuration["JwtKeyManagement:EncryptionKey"];
        if (string.IsNullOrWhiteSpace(encryptionKey))
        {
            // Key: Config.Jwt.EncryptionKeyMissing
            errors.Add("Config.Jwt.EncryptionKeyMissing");
        }
        else if (encryptionKey.Length < 32)
        {
            // Key com parâmetro: Config.Jwt.EncryptionKeyTooShort
            errors.Add($"Config.Jwt.EncryptionKeyTooShort:{encryptionKey.Length}");
        }

        var issuer = configuration["JwtSettings:Issuer"];
        if (string.IsNullOrWhiteSpace(issuer))
        {
            // Key: Config.Jwt.IssuerMissing
            errors.Add("Config.Jwt.IssuerMissing");
        }

        var audience = configuration["JwtSettings:Audience"];
        if (string.IsNullOrWhiteSpace(audience))
        {
            // Key: Config.Jwt.AudienceMissing
            errors.Add("Config.Jwt.AudienceMissing");
        }

        var accessTokenExpiration = configuration.GetValue<int>("JwtSettings:AccessTokenExpirationMinutes");
        if (accessTokenExpiration <= 0)
        {
            // Key: Config.Jwt.AccessTokenExpirationInvalid
            errors.Add("Config.Jwt.AccessTokenExpirationInvalid");
        }

        var refreshTokenExpiration = configuration.GetValue<int>("JwtSettings:RefreshTokenExpirationDays");
        if (refreshTokenExpiration <= 0)
        {
            // Key: Config.Jwt.RefreshTokenExpirationInvalid
            errors.Add("Config.Jwt.RefreshTokenExpirationInvalid");
        }
    }

    private static void ValidateRateLimiting(IConfiguration configuration, List<string> errors)
    {
        // Exemplo: validar se a configuração necessária existe quando RateLimiting está habilitado
        var enabled = configuration.GetValue<bool?>("RateLimiting:EnableRateLimiting");
        if (enabled.HasValue && enabled.Value)
        {
            var policy = configuration["RateLimiting:DefaultPolicyName"];
            if (string.IsNullOrWhiteSpace(policy))
            {
                // Key: Config.RateLimiting.PolicyMissing
                errors.Add("Config.RateLimiting.PolicyMissing");
            }
        }
    }

    private static void ValidateCors(IConfiguration configuration, IWebHostEnvironment environment, List<string> errors)
    {
        var corsEnabled = configuration.GetValue<bool?>("Cors:EnableCors");
        if (corsEnabled.HasValue && corsEnabled.Value)
        {
            var policyName = configuration.GetValue<string>("Cors:PolicyName");
            if (string.IsNullOrWhiteSpace(policyName))
            {
                // Key: Config.Cors.PolicyNameMissing
                errors.Add("Config.Cors.PolicyNameMissing");
            }
        }
    }
}
