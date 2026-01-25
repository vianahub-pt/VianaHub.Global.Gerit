using Microsoft.AspNetCore.Mvc;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Interfaces;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Application.Dtos.Request.Auth;

namespace VianaHub.Global.Gerit.Api.Endpoints;

public static class JwtKeyEndpoints
{
    public static void MapJwtKeyEndpoints(this WebApplication app)
    {
        app.MapGet("/v1/admin/jwtkeys/{tenantId}", async (IJwtKeyAppService service, INotify notify, int tenantId, CancellationToken ct) =>
        {
            var result = await service.GetByTenantAsync(tenantId, ct);
            return notify.CustomResponse(result);
        })
        .WithName("GetJwtKeysByTenant")
        .WithTags("JwtKeys");

        app.MapGet("/v1/admin/jwtkeys/{tenantId}/active", async (IJwtKeyAppService service, INotify notify, int tenantId, CancellationToken ct) =>
        {
            var result = await service.GetActiveKeyAsync(tenantId, ct);
            if (result == null)
            {
                // Service is expected to have added notifications when appropriate
                return notify.CustomResponse(204);
            }
            return notify.CustomResponse(result);
        })
        .WithName("GetActiveJwtKey")
        .WithTags("JwtKeys");

        app.MapPost("/v1/admin/jwtkeys/{tenantId}/create-initial", async (IJwtKeyAppService service, INotify notify, int tenantId, CancellationToken ct) =>
        {
            var ok = await service.CreateInitialIfNotExistsAsync(tenantId, ct);
            if (!ok) return notify.CustomResponse();
            return notify.CustomResponse(201);
        })
        .WithName("CreateInitialJwtKey")
        .WithTags("JwtKeys");

        app.MapPatch("/v1/admin/jwtkeys/{id}/revoke", async (IJwtKeyAppService service, INotify notify, int id, [FromBody] RevokeRequest req, CancellationToken ct) =>
        {
            var ok = await service.RevokeAsync(id, req.Reason, ct);
            if (!ok) return notify.CustomResponse();
            return notify.CustomResponse(200);
        })
        .WithName("RevokeJwtKey")
        .WithTags("JwtKeys");
    }
}
