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

        // Se houver erros, lança exceção
        if (errors.Any())
        {
            var errorMessage = $"Erro(s) de configuração detectado(s):{Environment.NewLine}{string.Join(Environment.NewLine, errors)}";
            throw new InvalidOperationException(errorMessage);
        }
    }

    private static void ValidateConnectionStrings(IConfiguration configuration, List<string> errors)
    {
        var vianaIdConnection = configuration.GetConnectionString("VianaIDConnection");
        if (string.IsNullOrWhiteSpace(vianaIdConnection))
        {
            errors.Add("? ConnectionStrings:VianaIDConnection não está configurada");
        }

        var hangfireConnection = configuration.GetConnectionString("HangfireConnection");
        if (string.IsNullOrWhiteSpace(hangfireConnection))
        {
            errors.Add("? ConnectionStrings:HangfireConnection não está configurada");
        }
    }

    private static void ValidateJwtSettings(IConfiguration configuration, List<string> errors)
    {
        var encryptionKey = configuration["JwtKeyManagement:EncryptionKey"];
        if (string.IsNullOrWhiteSpace(encryptionKey))
        {
            errors.Add("? JwtKeyManagement:EncryptionKey não está configurada");
        }
        else if (encryptionKey.Length < 32)
        {
            errors.Add($"? JwtKeyManagement:EncryptionKey deve ter no mínimo 32 caracteres (atual: {encryptionKey.Length})");
        }

        var issuer = configuration["JwtSettings:Issuer"];
        if (string.IsNullOrWhiteSpace(issuer))
        {
            errors.Add("? JwtSettings:Issuer não está configurado");
        }

        var audience = configuration["JwtSettings:Audience"];
        if (string.IsNullOrWhiteSpace(audience))
        {
            errors.Add("? JwtSettings:Audience não está configurado");
        }

        var accessTokenExpiration = configuration.GetValue<int>("JwtSettings:AccessTokenExpirationMinutes");
        if (accessTokenExpiration <= 0)
        {
            errors.Add("? JwtSettings:AccessTokenExpirationMinutes deve ser maior que zero");
        }

        var refreshTokenExpiration = configuration.GetValue<int>("JwtSettings:RefreshTokenExpirationDays");
        if (refreshTokenExpiration <= 0)
        {
            errors.Add("? JwtSettings:RefreshTokenExpirationDays deve ser maior que zero");
        }
    }

    private static void ValidateRateLimiting(IConfiguration configuration, List<string> errors)
    {
        var rateLimitingEnabled = configuration.GetValue<bool>("RateLimiting:EnableRateLimiting");
        if (!rateLimitingEnabled)
        {
            return; // Rate limiting desabilitado, não valida
        }

        var generalPermitLimit = configuration.GetValue<int>("RateLimiting:GeneralRules:PermitLimit");
        if (generalPermitLimit <= 0)
        {
            errors.Add("? RateLimiting:GeneralRules:PermitLimit deve ser maior que zero");
        }

        var authPermitLimit = configuration.GetValue<int>("RateLimiting:AuthenticationEndpoints:PermitLimit");
        if (authPermitLimit <= 0)
        {
            errors.Add("? RateLimiting:AuthenticationEndpoints:PermitLimit deve ser maior que zero");
        }
    }

    private static void ValidateCors(IConfiguration configuration, IWebHostEnvironment environment, List<string> errors)
    {
        var corsEnabled = configuration.GetValue<bool>("Cors:EnableCors");
        if (!corsEnabled)
        {
            return; // CORS desabilitado, não valida
        }

        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

        // Em produção, não deve permitir origins vazias
        if (environment.IsProduction() && allowedOrigins.Length == 0)
        {
            errors.Add("?? AVISO: Cors:AllowedOrigins está vazia em ambiente de produção. Configure origins específicas.");
        }

        // Verifica se há wildcard (*) em produção
        if (environment.IsProduction() && allowedOrigins.Any(o => o == "*"))
        {
            errors.Add("? CRÍTICO: Cors:AllowedOrigins não deve conter '*' em ambiente de produção");
        }
    }
}
