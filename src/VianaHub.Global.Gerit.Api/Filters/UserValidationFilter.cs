using System.Net;
using VianaHub.Global.Gerit.Application.Dtos.Base;

namespace VianaHub.Global.Gerit.Api.Filters;

public class UserValidationFilter : IEndpointFilter
{
    private readonly ILogger<UserValidationFilter> _logger;

    public UserValidationFilter(ILogger<UserValidationFilter> logger)
    {
        _logger = logger;
    }

    public async ValueTask<object> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var httpContext = context.HttpContext;
        var user = httpContext.User;

        // Extrair userId da rota
        var userIdParam = httpContext.Request.RouteValues["userId"]?.ToString();

        if (string.IsNullOrEmpty(userIdParam))
        {
            _logger.LogWarning("? userId não encontrado na URL da requisição");
            return CreateErrorResponse("userId é obrigatório na URL");
        }

        if (!Guid.TryParse(userIdParam, out var requesteduserId))
        {
            _logger.LogWarning("? userId inválido na URL: {userId}", userIdParam);
            return CreateErrorResponse("userId inválido na URL");
        }

        // Verificar se usuário está autenticado
        if (!user.Identity?.IsAuthenticated ?? true)
        {
            _logger.LogWarning("? Usuário não autenticado tentando acessar user {userId}", requesteduserId);
            return Results.Unauthorized();
        }

        // Extrair userId do token JWT
        // Tentar múltiplas claims para compatibilidade
        var useruserIdClaim = user.FindFirst("user_id")?.Value
            ?? user.FindFirst("tid")?.Value
            ?? user.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(useruserIdClaim))
        {
            _logger.LogWarning(
                "?? Token JWT não contém claim de userId. Usuário: {UserId}",
                user.FindFirst("sub")?.Value ?? "Unknown");

            // TODO: Em produção, isso deveria retornar Unauthorized
            // Por enquanto, permite acesso para compatibilidade com tokens antigos
            _logger.LogWarning(
                "?? MODO DE COMPATIBILIDADE: Permitindo acesso sem validação de user. " +
                "ATENÇÃO: Isso será removido em versões futuras!");

            return await next(context);
        }

        if (!Guid.TryParse(useruserIdClaim, out var useruserId))
        {
            _logger.LogError(
                "? userId no token JWT está em formato inválido: {userId}",
                useruserIdClaim);
            return Results.Unauthorized();
        }

        // Validar se o user da URL corresponde ao user do usuário
        if (requesteduserId != useruserId)
        {
            var userId = user.FindFirst("sub")?.Value ?? "Unknown";
            var userName = user.FindFirst("name")?.Value ?? "Unknown";

            _logger.LogWarning(
                "?? TENTATIVA DE ACESSO CROSS-user DETECTADA! " +
                "Usuário: {UserId} ({UserName}), " +
                "user do Usuário: {UseruserId}, " +
                "user Requisitado: {RequesteduserId}, " +
                "Endpoint: {Endpoint}, " +
                "IP: {IP}",
                userId,
                userName,
                useruserId,
                requesteduserId,
                httpContext.Request.Path,
                httpContext.Connection.RemoteIpAddress);

            return CreateErrorResponse(
                "Acesso Negado",
                "Você não tem permissão para acessar recursos deste user",
                HttpStatusCode.Forbidden);
        }

        _logger.LogDebug(
            "? Validação de user bem-sucedida. Usuário: {UserId}, user: {userId}",
            user.FindFirst("sub")?.Value,
            useruserId);

        return await next(context);
    }

    private IResult CreateErrorResponse(
        string title,
        string? message = null,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        var errorResponse = new ErrorResponse(title);

        if (!string.IsNullOrEmpty(message))
        {
            errorResponse.AddError("userValidation", message);
        }

        return Results.Json(errorResponse, statusCode: (int)statusCode);
    }
}
