using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Dtos.Request.RolePermission;
using VianaHub.Global.Gerit.Application.Dtos.Response.RolePermission;
using VianaHub.Global.Gerit.Application.Interfaces;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Application.Dtos.Base;

namespace VianaHub.Global.Gerit.Api.Endpoints;

public static class RolePermissionEndpoint
{
    public static void MapRolePermissionEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/rolepermissions").WithTags("RolePermissions").WithGroupName("v1").RequireAuthorization();

        groupV1.MapGet("/", async (IRolePermissionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var result = await appService.GetAllAsync(ct);
            return notify.CustomResponse(result, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "RolePermissions", "GetAll")
        .WithName("GetAllRolePermissions")
        .WithSummary("Swagger.Endpoint.RolePermission.GetAllRolePermissions.Summary")
        .Produces<IEnumerable<RolePermissionResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{id}", async (int id, IRolePermissionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var result = await appService.GetByIdAsync(id, ct);
            return notify.CustomResponse(result, result != null ? 200 : 404);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "RolePermissions", "GetBy")
        .WithName("GetRolePermissionById")
        .WithSummary("Swagger.Endpoint.RolePermission.GetRolePermissionById.Summary")
        .Produces<RolePermissionResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/role/{roleId}", async (int roleId, IRolePermissionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var result = await appService.GetByRoleAsync(roleId, ct);
            return notify.CustomResponse(result, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "RolePermissions", "GetBy")
        .WithName("GetRolePermissionsByRole")
        .WithSummary("Swagger.Endpoint.RolePermission.GetRolePermissionsByRole.Summary")
        .Produces<IEnumerable<RolePermissionResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/resource/{resourceId}", async (int resourceId, IRolePermissionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var result = await appService.GetByResourceAsync(resourceId, ct);
            return notify.CustomResponse(result, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "RolePermissions", "GetBy")
        .WithName("GetRolePermissionsByResource")
        .WithSummary("Swagger.Endpoint.RolePermission.GetRolePermissionsByResource.Summary")
        .Produces<IEnumerable<RolePermissionResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/", async ([FromBody] CreateRolePermissionRequest request, IRolePermissionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var created = await appService.CreateAsync(request, ct);
            return notify.CustomResponse(created != null ? 201 : 400);
        })
        //.CustomAuthorize("Admin,BackOffice,Manager", "RolePermissions", "Create")
        .AllowAnonymous()
        .WithName("CreateRolePermission")
        .WithSummary("Swagger.Endpoint.RolePermission.CreateRolePermission.Summary")
        .Produces(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapDelete("/{id}", async (int id, IRolePermissionAppService appService, INotify notify, CancellationToken ct) =>
        {
            await appService.DeleteAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "RolePermissions", "Delete")
        .WithName("DeleteRolePermission")
        .WithSummary("Swagger.Endpoint.RolePermission.DeleteRolePermission.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
