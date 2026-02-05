using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Interfaces.Identity;
using VianaHub.Global.Gerit.Application.Dtos.Response.Identity.RolePermission;
using VianaHub.Global.Gerit.Application.Dtos.Request.Identity.RolePermission;

namespace VianaHub.Global.Gerit.Api.Endpoints.Identity;

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

        // Upload massivo de roles via CSV
        groupV1.MapPost("/bulk-upload", async (HttpRequest request, IRolePermissionAppService appService, INotify notify, CancellationToken ct) =>
        {
            if (!request.HasFormContentType || request.Form.Files.Count == 0)
            {
                notify.Add("Nenhum arquivo foi enviado", 400);
                return notify.CustomResponse();
            }

            var file = request.Form.Files[0];
            var success = await appService.BulkUploadAsync(file, ct);
            return notify.CustomResponse(success);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "Roles", "BulkUpload")
        .WithName("BulkUploadRoles")
        .WithSummary("Swagger.Endpoint.RolePermission.BulkUploadRoles.Summary")
        .DisableAntiforgery()
        .Accepts<IFormFile>("multipart/form-data")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
