using System.Threading.RateLimiting;

namespace VianaHub.Global.Gerit.Api.Configuration;

/// <summary>
/// Configuração de Rate Limiting para proteger a API contra abuso
/// </summary>
public static class RateLimitingSetup
{
    /// <summary>
    /// Adiciona configuração de rate limiting ao pipeline
    /// </summary>
    public static IServiceCollection AddRateLimitingConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var rateLimitConfig = configuration.GetSection("RateLimiting");
        var enableRateLimiting = rateLimitConfig.GetValue<bool>("EnableRateLimiting", true);

        if (!enableRateLimiting)
        {
            return services;
        }

        services.AddRateLimiter(options =>
        {
            // Política Global (padrão para todos os endpoints)
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                var tenantId = context.Request.Headers["X-Tenant-Id"].ToString();
                var key = string.IsNullOrEmpty(tenantId)
                    ? context.Connection.RemoteIpAddress?.ToString() ?? "anonymous"
                    : $"tenant-{tenantId}";

                return RateLimitPartition.GetFixedWindowLimiter(key, _ =>
                    new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = rateLimitConfig.GetValue("GeneralRules:PermitLimit", 100),
                        Window = TimeSpan.Parse(rateLimitConfig.GetValue("GeneralRules:Window", "00:01:00")),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = rateLimitConfig.GetValue("GeneralRules:QueueLimit", 10)
                    });
            });

            // Política para Endpoints de Autenticação (mais restritiva)
            options.AddPolicy("authentication", context =>
            {
                var ip = context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";

                return RateLimitPartition.GetFixedWindowLimiter(ip, _ =>
                    new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = rateLimitConfig.GetValue("AuthenticationEndpoints:PermitLimit", 5),
                        Window = TimeSpan.Parse(rateLimitConfig.GetValue("AuthenticationEndpoints:Window", "00:01:00")),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = rateLimitConfig.GetValue("AuthenticationEndpoints:QueueLimit", 2)
                    });
            });

            // Política para Refresh Token (moderadamente restritiva)
            options.AddPolicy("refreshtoken", context =>
            {
                var userId = context.User?.FindFirst("sub")?.Value ??
                             context.Connection.RemoteIpAddress?.ToString() ??
                             "anonymous";

                return RateLimitPartition.GetFixedWindowLimiter(userId, _ =>
                    new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = rateLimitConfig.GetValue("RefreshTokenEndpoints:PermitLimit", 10),
                        Window = TimeSpan.Parse(rateLimitConfig.GetValue("RefreshTokenEndpoints:Window", "00:01:00")),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = rateLimitConfig.GetValue("RefreshTokenEndpoints:QueueLimit", 5)
                    });
            });

            // Política para Operações Administrativas (mais permissiva)
            options.AddPolicy("admin", context =>
            {
                var userId = context.User?.FindFirst("sub")?.Value ?? "anonymous";

                return RateLimitPartition.GetFixedWindowLimiter(userId, _ =>
                    new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 200,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 50
                    });
            });

            // Configuração de resposta quando o limite é atingido
            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

                var retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfterValue)
                    ? retryAfterValue.TotalSeconds
                    : 60;

                context.HttpContext.Response.Headers["Retry-After"] = retryAfter.ToString();
                context.HttpContext.Response.Headers["X-Rate-Limit-Remaining"] = "0";

                await context.HttpContext.Response.WriteAsJsonAsync(new
                {
                    StatusCode = 429,
                    Message = "Limite de requisições excedido. Por favor, tente novamente mais tarde.",
                    RetryAfter = $"{retryAfter} segundos"
                }, cancellationToken);
            };
        });

        return services;
    }

    /// <summary>
    /// Usa o middleware de rate limiting
    /// </summary>
    public static IApplicationBuilder UseRateLimitingConfiguration(this IApplicationBuilder app)
    {
        app.UseRateLimiter();
        return app;
    }
}
