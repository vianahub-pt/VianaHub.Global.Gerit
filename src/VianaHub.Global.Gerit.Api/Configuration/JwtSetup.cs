using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using VianaHub.Global.Gerit.Application.Configuration;
using VianaHub.Global.Gerit.Domain.Interfaces.Repository;
using VianaHub.Global.Gerit.Domain.Tools.Cryptography;

namespace VianaHub.Global.Gerit.Api.Configuration;

/// <summary>
/// Configuração JWT com suporte a chaves dinâmicas e rotação
/// </summary>
public static class JwtSetup
{
    /// <summary>
    /// Adiciona a configuração de autenticação JWT ao pipeline de serviços.
    /// Suporta chaves dinâmicas do banco de dados e rotação sem downtime.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    /// <param name="configuration">Configuração da aplicação.</param>
    /// <returns>A própria coleção de serviços para encadeamento.</returns>
    public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var apiSettingSection = configuration.GetSection("ApplicationSettings");
        services.Configure<ApplicationSettings>(apiSettingSection);

        var jwtSettings = configuration.GetSection("JwtSettings");
        services.Configure<JwtSettings>(jwtSettings);

        var jwtConfig = jwtSettings.Get<JwtSettings>() ?? new JwtSettings();

        // Validar configurações obrigatórias
        ValidateJwtConfiguration(jwtConfig, configuration);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(bearerOptions =>
        {
            bearerOptions.SaveToken = true;

            // HTTPS é obrigatório em produção
            bearerOptions.RequireHttpsMetadata = jwtConfig.RequireHttpsMetadata;

            bearerOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                // Issuer e Audience serão validados dinamicamente por Tenant
                ValidIssuers = jwtConfig.ValidIssuers ?? new[] { jwtConfig.Issuer },
                ValidAudiences = jwtConfig.ValidAudiences ?? new[] { jwtConfig.Audience },

                // Clock skew para compensar diferenças de relógio entre servidores
                ClockSkew = TimeSpan.FromMinutes(jwtConfig.ClockSkewMinutes),

                // Resolver chaves de assinatura dinamicamente do banco de dados
                IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
                {
                    // NOTA: Chaves serão resolvidas no evento OnMessageReceived
                    // que tem acesso ao HttpContext e IServiceProvider
                    return Array.Empty<SecurityKey>();
                }
            };

            // Eventos para logging, auditoria e resolução de chaves
            bearerOptions.Events = new JwtBearerEvents
            {
                OnMessageReceived = async context =>
                {
                    // Aqui temos acesso ao HttpContext e podemos resolver chaves corretamente
                    var keyRepository = context.HttpContext.RequestServices
                        .GetRequiredService<IJwtKeyDataRepository>();
                    var logger = context.HttpContext.RequestServices
                        .GetRequiredService<ILogger<JwtBearerHandler>>();

                    try
                    {
                        // NOTA: Como não temos TenantId no contexto ainda, buscar todas as chaves ativas
                        // Em produção, isso pode ser otimizado com cache ou busca por tenant específico
                        var allKeys = await keyRepository.GetAllAsync(default);
                        var keys = allKeys.Where(k => k.IsValidForValidation());

                        if (!keys.Any())
                        {
                            logger.LogError("❌ Nenhuma chave JWT ativa encontrada no banco de dados");
                            context.Fail("No active JWT signing keys available");
                            return;
                        }

                        // Configurar chaves no contexto de validação
                        var securityKeys = keys.Select(key =>
                        {
                            try
                            {
                                var rsa = System.Security.Cryptography.RSA.Create();

                                // CORREÇÃO: Extrair bytes do formato PEM antes de importar
                                var publicKeyBytes = CryptoRSA.ExtractBytesFromPem(key.PublicKey, "PUBLIC KEY");
                                rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);

                                return new RsaSecurityKey(rsa) { KeyId = key.KeyId.ToString() } as SecurityKey;
                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex, "❌ Erro ao processar chave pública. KeyId={KeyId}", key.KeyId);
                                return null;
                            }
                        })
                        .Where(k => k != null)
                        .ToList();

                        if (!securityKeys.Any())
                        {
                            logger.LogError("❌ Nenhuma chave JWT válida encontrada após processamento");
                            context.Fail("No valid JWT signing keys available");
                            return;
                        }

                        // Atualizar parâmetros de validação com as chaves carregadas
                        context.Options.TokenValidationParameters.IssuerSigningKeys = securityKeys;

                        logger.LogDebug(
                            "✅ {Count} chave(s) JWT carregada(s) para validação",
                            securityKeys.Count);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "❌ Erro ao carregar chaves JWT do banco de dados");
                        context.Fail("Failed to load JWT signing keys");
                    }
                },

                OnAuthenticationFailed = context =>
                {
                    var logger = context.HttpContext.RequestServices
                        .GetRequiredService<ILogger<JwtBearerHandler>>();

                    logger.LogWarning(
                        "❌ JWT Authentication failed: {Error} | Path: {Path} | IP: {IP}",
                        context.Exception.Message,
                        context.Request.Path,
                        context.HttpContext.Connection.RemoteIpAddress);

                    return Task.CompletedTask;
                },

                OnTokenValidated = context =>
                {
                    var logger = context.HttpContext.RequestServices
                        .GetRequiredService<ILogger<JwtBearerHandler>>();

                    var userId = context.Principal?.FindFirst("sub")?.Value;

                    logger.LogInformation(
                        "✅ JWT Token validated successfully | User: {UserId} | Path: {Path}",
                        userId,
                        context.Request.Path);

                    return Task.CompletedTask;
                },

                OnChallenge = context =>
                {
                    var logger = context.HttpContext.RequestServices
                        .GetRequiredService<ILogger<JwtBearerHandler>>();

                    logger.LogWarning(
                        "⚠️ JWT Challenge issued | Path: {Path} | Error: {Error}",
                        context.Request.Path,
                        context.Error);

                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }

    /// <summary>
    /// Valida configurações obrigatórias do JWT
    /// </summary>
    private static void ValidateJwtConfiguration(JwtSettings jwtConfig, IConfiguration configuration)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(jwtConfig.Issuer))
        {
            errors.Add("JwtSettings:Issuer não está configurado");
        }

        if (string.IsNullOrWhiteSpace(jwtConfig.Audience))
        {
            errors.Add("JwtSettings:Audience não está configurado");
        }

        if (jwtConfig.AccessTokenExpirationMinutes <= 0)
        {
            errors.Add("JwtSettings:AccessTokenExpirationMinutes deve ser maior que zero");
        }

        if (jwtConfig.RefreshTokenExpirationDays <= 0)
        {
            errors.Add("JwtSettings:RefreshTokenExpirationDays deve ser maior que zero");
        }

        // Em produção, validações mais rigorosas
        var env = configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT");
        if (env?.Equals("Production", StringComparison.OrdinalIgnoreCase) == true)
        {
            if (jwtConfig.AccessTokenExpirationMinutes > 60)
            {
                errors.Add("⚠️ Em produção, AccessTokenExpirationMinutes não deve exceder 60 minutos");
            }

            if (!jwtConfig.RequireHttpsMetadata)
            {
                errors.Add("❌ CRÍTICO: RequireHttpsMetadata deve ser true em produção");
            }

            if (jwtConfig.ClockSkewMinutes > 5)
            {
                errors.Add("⚠️ Em produção, ClockSkewMinutes recomendado é máximo 5 minutos (configurado: {0})"
                    .Replace("{0}", jwtConfig.ClockSkewMinutes.ToString()));
            }
        }

        if (errors.Any())
        {
            throw new InvalidOperationException(
                $"Configuração JWT inválida:\n{string.Join("\n", errors)}");
        }
    }
}
