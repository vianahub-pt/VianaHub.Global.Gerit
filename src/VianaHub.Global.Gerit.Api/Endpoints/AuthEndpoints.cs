using VianaHub.Global.Gerit.Application.Dtos.Request.Auth;
using VianaHub.Global.Gerit.Application.Interfaces;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Domain.Interfaces;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var localization = app.Services.GetService(typeof(ILocalizationService)) as ILocalizationService;

        app.MapPost("/auth/register", async (RegisterRequest request, IAuthAppService authService, INotify notify, CancellationToken ct) =>
        {
            var result = await authService.RegisterAsync(request, ct);
            return notify.CustomResponse(result);
        })
        .WithName("Auth.Register")
        .WithTags("Auth")
        .RequireRateLimiting("authentication")
        .WithValidation<RegisterRequest>()
        .Produces(200)
        .Produces(400)
        .Produces(409)
        .WithSummary(localization?.GetMessage("Swagger.Endpoint.Auth.Register.Summary") ?? "Auth.Register")
        .WithDescription(localization?.GetMessage("Swagger.Endpoint.Auth.Register.Description") ?? "Register new user");

        app.MapPost("/auth/login", async (LoginRequest request, IAuthAppService authService, INotify notify, CancellationToken ct) =>
        {
            var result = await authService.LoginAsync(request, ct);
            return notify.CustomResponse(result);
        })
        .WithName("Auth.Login")
        .WithTags("Auth")
        .RequireRateLimiting("authentication")
        .WithValidation<LoginRequest>()
        .Produces(200)
        .Produces(400)
        .Produces(401)
        .WithSummary(localization?.GetMessage("Swagger.Endpoint.Auth.Login.Summary") ?? "Auth.Login")
        .WithDescription(localization?.GetMessage("Swagger.Endpoint.Auth.Login.Description") ?? "Authenticate user");

        app.MapPost("/auth/refresh", async (RefreshRequest request, IAuthAppService authService, INotify notify, CancellationToken ct) =>
        {
            var result = await authService.RefreshAsync(request, ct);
            return notify.CustomResponse(result);
        })
        .WithName("Auth.Refresh")
        .WithTags("Auth")
        .RequireRateLimiting("refreshtoken")
        .WithValidation<RefreshRequest>()
        .Produces(200)
        .Produces(400)
        .Produces(401)
        .WithSummary(localization?.GetMessage("Swagger.Endpoint.Auth.Refresh.Summary") ?? "Auth.Refresh")
        .WithDescription(localization?.GetMessage("Swagger.Endpoint.Auth.Refresh.Description") ?? "Refresh access token");
    }
}
