using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Application.Interfaces.Identity;
using VianaHub.Global.Gerit.Application.Dtos.Request.Identity.Auth;
using VianaHub.Global.Gerit.Api.Endpoints.Base;

namespace VianaHub.Global.Gerit.Api.Endpoints.Identity;

[EndpointMapper]
public static class AuthEndpoint
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/auth")
            .WithTags("Auth")
            .WithGroupName("v1")
            .AllowAnonymous(); // Endpoints de autenticação devem ser públicos

        groupV1.MapPost("/register", async (RegisterRequest request, IAuthAppService authService, INotify notify, CancellationToken ct) =>
        {
            var result = await authService.RegisterAsync(request, ct);
            return notify.CustomResponse(result);
        })
        .WithName("Auth.Register")
        .RequireRateLimiting("authentication")
        .WithValidation<RegisterRequest>()
        .WithSummary("Swagger.Endpoint.Auth.Register.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict);

        groupV1.MapPost("/login", async (LoginRequest request, IAuthAppService authService, INotify notify, CancellationToken ct) =>
        {
            var result = await authService.LoginAsync(request, ct);
            return notify.CustomResponse(result);
        })
        .WithName("Auth.Login")
        .RequireRateLimiting("authentication")
        .WithValidation<LoginRequest>()
        .WithSummary("Swagger.Endpoint.Auth.Login.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized);

        groupV1.MapPost("/refresh", async (RefreshRequest request, IAuthAppService authService, INotify notify, CancellationToken ct) =>
        {
            var result = await authService.RefreshAsync(request, ct);
            return notify.CustomResponse(result);
        })
        .WithName("Auth.Refresh")
        .RequireRateLimiting("refreshtoken")
        .WithValidation<RefreshRequest>()
        .WithSummary("Swagger.Endpoint.Auth.Refresh.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}
