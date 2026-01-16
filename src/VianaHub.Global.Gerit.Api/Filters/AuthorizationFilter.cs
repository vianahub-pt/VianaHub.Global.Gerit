using System.Net;
using System.Security.Claims;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Filters;

/// <summary>
/// Filtro de autorização customizado que valida se o usuário tem permissão para acessar o recurso.
/// Verifica os perfis (roles) do usuário no token JWT e valida permissões de recurso e ação
/// diretamente das claims do token (stateless), sem necessidade de consultar o banco de dados.
/// </summary>
public class AuthorizationFilter : IEndpointFilter
{
    private readonly string[] _allowedRoles;
    private readonly string _resource;
    private readonly string _action;
    private readonly ILogger<AuthorizationFilter> _logger;

    /// <summary>
    /// Inicializa uma nova instância do <see cref="AuthorizationFilter"/>.
    /// </summary>
    /// <param name="allowedRoles">Roles permitidas (separadas por vírgula)</param>
    /// <param name="resource">Nome do recurso (ex: "Actions", "Resources", "Permissions")</param>
    /// <param name="action">Ação permitida (ex: "Read", "Write", "Delete")</param>
    /// <param name="logger">Logger</param>
    public AuthorizationFilter(
        string allowedRoles,
        string resource,
        string action,
        ILogger<AuthorizationFilter> logger)
    {
        _allowedRoles = allowedRoles?.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(r => r.Trim().ToLower())
            .ToArray() ?? Array.Empty<string>();
        _resource = resource;
        _action = action;
        _logger = logger;
    }

    /// <summary>
    /// Executa a validação de autorização verificando:
    /// 1. Se o usuário está autenticado
    /// 2. Se possui uma das roles permitidas
    /// 3. Se tem permissão para o recurso e ação especificados (via claims do token)
    /// </summary>
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var httpContext = context.HttpContext;
        var user = httpContext.User;

        _logger.LogDebug("AuthorizationFilter executando para Resource: {Resource}, ActionEntity: {ActionEntity}", _resource, _action);

        // Verificar se o usuário está autenticado
        if (!user.Identity?.IsAuthenticated ?? true)
        {
            _logger.LogWarning("Acesso negado: Usuário não autenticado");
            return Results.Unauthorized();
        }

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? user.FindFirst("sub")?.Value
            ?? user.FindFirst("user_id")?.Value;

        var userName = user.FindFirst(ClaimTypes.Name)?.Value
            ?? user.FindFirst("name")?.Value
            ?? user.FindFirst("preferred_username")?.Value;

        var userRoles = user.FindAll(ClaimTypes.Role)
            .Select(c => c.Value.ToLower())
            .ToList();

        if (!userRoles.Any())
        {
            // Tentar claims alternativas para roles
            userRoles = user.FindAll("role")
                .Select(c => c.Value.ToLower())
                .ToList();
        }

        _logger.LogInformation("Usuário: {UserId} ({UserName}), Roles: [{Roles}]",
            userId, userName, string.Join(", ", userRoles));

        // Verificar se o usuário possui alguma das roles permitidas (se configurado)
        if (_allowedRoles.Any() && !_allowedRoles.Any(allowedRole => userRoles.Contains(allowedRole)))
        {
            _logger.LogWarning(
                "Acesso negado: Usuário {UserId} não possui nenhuma das roles permitidas. " +
                "Roles do usuário: [{UserRoles}], Roles permitidas: [{AllowedRoles}]",
                userId,
                string.Join(", ", userRoles),
                string.Join(", ", _allowedRoles));

            var notify = httpContext.RequestServices.GetService<INotify>();
            if (notify != null)
            {
                notify.Add(
                    $"Você não tem permissão para acessar este recurso. Perfil necessário: {string.Join(", ", _allowedRoles)}",
                    (int)HttpStatusCode.Forbidden);
            }

            var errorResponse = new ErrorResponse("Acesso Negado");
            errorResponse.AddError("Authorization", $"Você não tem permissão para acessar este recurso. Perfis necessários: {string.Join(", ", _allowedRoles)}");

            return Results.Json(errorResponse, statusCode: (int)HttpStatusCode.Forbidden);
        }

        // Verificar permissões específicas do recurso e ação (se configurado)
        // As permissões já estão no token como claims no formato "resource:action"
        if (!string.IsNullOrEmpty(_resource) && !string.IsNullOrEmpty(_action))
        {
            var hasPermission = ValidateResourcePermissionFromClaims(user, _resource, _action);

            if (!hasPermission)
            {
                _logger.LogWarning(
                    "Acesso negado: Usuário {UserId} não tem permissão para {ActionEntity} em {Resource}",
                    userId, _action, _resource);

                var notify = httpContext.RequestServices.GetService<INotify>();
                if (notify != null)
                {
                    notify.Add(
                        $"Você não tem permissão para executar a ação '{_action}' no recurso '{_resource}'",
                        (int)HttpStatusCode.Forbidden);
                }

                var errorResponse = new ErrorResponse("Acesso Negado");
                errorResponse.AddError("Authorization", $"Você não tem permissão para executar a ação '{_action}' no recurso '{_resource}'");

                return Results.Json(errorResponse, statusCode: (int)HttpStatusCode.Forbidden);
            }
        }

        _logger.LogInformation("Autorização concedida para usuário {UserId}", userId);
        return await next(context);
    }

    /// <summary>
    /// Valida se o usuário tem permissão específica para o recurso e ação
    /// verificando diretamente as claims do token JWT (stateless).
    /// 
    /// Esta abordagem segue as melhores práticas de sistemas IAM modernos como
    /// Auth0, Okta, Azure AD e Keycloak, onde as permissões são incluídas no token
    /// durante a autenticação, eliminando a necessidade de consultas ao banco de dados
    /// a cada requisição.
    /// 
    /// Benefícios:
    /// - Performance até 1000x melhor (sem I/O de banco de dados)
    /// - Escalabilidade horizontal simples
    /// - Latência mínima (~0.1ms vs ~50-200ms)
    /// - Alinhado com padrões OAuth2/OIDC
    /// </summary>
    /// <param name="user">ClaimsPrincipal do usuário autenticado</param>
    /// <param name="resource">Nome do recurso (ex: "users", "resources", "actions")</param>
    /// <param name="action">Nome da ação (ex: "read", "write", "delete")</param>
    /// <returns>True se o usuário tem a permissão, false caso contrário</returns>
    private bool ValidateResourcePermissionFromClaims(
        ClaimsPrincipal user,
        string resource,
        string action)
    {
        // Formato padrão: "resource:action" (case-insensitive)
        var requiredPermission = $"{resource}:{action}".ToLower();

        // Extrair todas as permissões das claims do token
        var userPermissions = user.FindAll("permission")
            .Select(c => c.Value.ToLower())
            .ToHashSet();

        var hasPermission = userPermissions.Contains(requiredPermission);

        if (hasPermission)
        {
            _logger.LogDebug(
                "Permissão '{Permission}' encontrada nas claims do token",
                requiredPermission);
        }
        else
        {
            _logger.LogDebug(
                "Permissão '{Permission}' NÃO encontrada. Permissões disponíveis: [{Permissions}]",
                requiredPermission,
                string.Join(", ", userPermissions));
        }

        return hasPermission;
    }
}
