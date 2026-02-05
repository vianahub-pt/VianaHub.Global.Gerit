using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Identity.UserRole;
using VianaHub.Global.Gerit.Application.Dtos.Response.Identity.UserRole;
using VianaHub.Global.Gerit.Application.Interfaces.Identity;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Endpoints.Identity;

public static class UserRoleEndpoint
{
    public static void MapUserRoleEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/userroles").WithTags("UserRoles").WithGroupName("v1").RequireAuthorization();

        groupV1.MapGet("/all", async (IUserRoleAppService appService, INotify notify, CancellationToken ct) =>
        {
            var result = await appService.GetAllAsync(ct);
            return notify.CustomResponse(result, 200);
        })
        .CustomAuthorize("Admin,Manager", "UserRoles", "GetAll")
        .AllowAnonymous()
        .WithName("GetAllUserRoles")
        .WithSummary("Swagger.Endpoint.UserRole.GetAllUserRoles.Summary")
        .Produces<IEnumerable<UserRoleResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{id}", async (int id, IUserRoleAppService appService, INotify notify, CancellationToken ct) =>
        {
            var result = await appService.GetByIdAsync(id, ct);
            return notify.CustomResponse(result, result != null ? 200 : 404);
        })
        .CustomAuthorize("Admin,Manager", "UserRoles", "GetBy")
        .AllowAnonymous()
        .WithName("GetUserRoleById")
        .WithSummary("Swagger.Endpoint.UserRole.GetUserRoleById.Summary")
        .Produces<UserRoleResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/user/{userId}", async (int userId, IUserRoleAppService appService, INotify notify, CancellationToken ct) =>
        {
            var result = await appService.GetByUserAsync(userId, ct);
            return notify.CustomResponse(result, 200);
        })
        .CustomAuthorize("Admin,Manager,User", "UserRoles", "GetBy")
        .AllowAnonymous()
        .WithName("GetUserRolesByUser")
        .WithSummary("Swagger.Endpoint.UserRole.GetUserRolesByUser.Summary")
        .Produces<IEnumerable<UserRoleResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/role/{roleId}", async (int roleId, IUserRoleAppService appService, INotify notify, CancellationToken ct) =>
        {
            var result = await appService.GetByRoleAsync(roleId, ct);
            return notify.CustomResponse(result, 200);
        })
        .CustomAuthorize("Admin,Manager,User", "UserRoles", "GetBy")
        .AllowAnonymous()
        .WithName("GetUserRolesByRole")
        .WithSummary("Swagger.Endpoint.UserRole.GetUserRolesByRole.Summary")
        .Produces<IEnumerable<UserRoleResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/", async ([FromBody] CreateUserRoleRequest request, IUserRoleAppService appService, INotify notify, CancellationToken ct) =>
        {
            var created = await appService.CreateAsync(request, ct);
            return notify.CustomResponse(201);
        })
        .CustomAuthorize("Admin,Manager", "UserRoles", "Create")
        .AllowAnonymous()
        .WithName("CreateUserRole")
        .WithSummary("Swagger.Endpoint.UserRole.CreateUserRole.Summary")
        .Produces(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<CreateUserRoleRequest>();

        groupV1.MapPut("/{id}", async (int id, [FromBody] UpdateUserRoleRequest request, IUserRoleAppService appService, INotify notify, CancellationToken ct) =>
        {
            request.Id = id;
            var updated = await appService.UpdateAsync(request, ct);
            return notify.CustomResponse(204);
        })
        .CustomAuthorize("Admin,Manager", "UserRoles", "Update")
        .AllowAnonymous()
        .WithName("UpdateUserRole")
        .WithSummary("Swagger.Endpoint.UserRole.UpdateUserRole.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<UpdateUserRoleRequest>();

        groupV1.MapDelete("/{id}", async (int id, IUserRoleAppService appService, INotify notify, CancellationToken ct) =>
        {
            await appService.DeleteAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,Manager", "UserRoles", "Delete")
        .AllowAnonymous()
        .WithName("DeleteUserRole")
        .WithSummary("Swagger.Endpoint.UserRole.DeleteUserRole.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/bulk-upload", async (HttpRequest request, IUserRoleAppService appService, INotify notify, CancellationToken ct) =>
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
        .CustomAuthorize("Admin,BackOffice", "UserRoles", "BulkUpload")
        .AllowAnonymous()
        .WithName("BulkUploadUsers")
        .WithSummary("Swagger.Endpoint.UserRole.BulkUploadUsers.Summary")
        .DisableAntiforgery()
        .Accepts<IFormFile>("multipart/form-data")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
