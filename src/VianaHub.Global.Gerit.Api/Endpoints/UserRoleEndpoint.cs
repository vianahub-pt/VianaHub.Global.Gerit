using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.UserRole;
using VianaHub.Global.Gerit.Application.Dtos.Response.UserRole;
using VianaHub.Global.Gerit.Application.Interfaces;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Endpoints;

public static class UserRoleEndpoint
{
    public static void MapUserRoleEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/userroles").WithTags("UserRoles").WithGroupName("v1").RequireAuthorization();

        groupV1.MapGet("/all", async (IUserRoleAppService appService, INotify notify, CancellationToken ct) =>
        {
            var result = await appService.GetAllAsync();
            return notify.CustomResponse(result, 200);
        })
        //.CustomAuthorize("Admin,Manager", "UserRole", "GetAllUserRoles")
        .AllowAnonymous()
        .WithName("GetAllUserRoles")
        .WithSummary("Swagger.Endpoint.UserRole.GetAllUserRoles.Summary")
        .Produces<IEnumerable<UserRoleResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{id}", async (int id, IUserRoleAppService appService, INotify notify, CancellationToken ct) =>
        {
            var result = await appService.GetByIdAsync(id);
            return notify.CustomResponse(result, result != null ? 200 : 404);
        })
        //.CustomAuthorize("Admin,Manager", "UserRole", "GetUserRoleById")
        .AllowAnonymous()
        .WithName("GetUserRoleById")
        .WithSummary("Swagger.Endpoint.UserRole.GetUserRoleById.Summary")
        .Produces<UserRoleResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/user/{userId}", async (int userId, IUserRoleAppService appService, INotify notify, CancellationToken ct) =>
        {
            var result = await appService.GetByUserAsync(userId);
            return notify.CustomResponse(result, 200);
        })
        //.CustomAuthorize("Admin,Manager,User", "UserRole", "GetUserRolesByUser")
        .AllowAnonymous()
        .WithName("GetUserRolesByUser")
        .WithSummary("Swagger.Endpoint.UserRole.GetUserRolesByUser.Summary")
        .Produces<IEnumerable<UserRoleResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/role/{roleId}", async (int roleId, IUserRoleAppService appService, INotify notify, CancellationToken ct) =>
        {
            var result = await appService.GetByRoleAsync(roleId);
            return notify.CustomResponse(result, 200);
        })
        //.CustomAuthorize("Admin,Manager,User", "UserRole", "GetUserRolesByRole")
        .AllowAnonymous()
        .WithName("GetUserRolesByRole")
        .WithSummary("Swagger.Endpoint.UserRole.GetUserRolesByRole.Summary")
        .Produces<IEnumerable<UserRoleResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/", async ([FromBody] CreateUserRoleRequest request, IUserRoleAppService appService, INotify notify, CancellationToken ct) =>
        {
            var created = await appService.CreateAsync(request);
            return notify.CustomResponse(201);
        })
        //.CustomAuthorize("Admin,Manager", "UserRole", "CreateUserRole")
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
            var updated = await appService.UpdateAsync(request);
            return notify.CustomResponse(204);
        })
        //.CustomAuthorize("Admin,Manager", "UserRole", "UpdateUserRole")
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
            await appService.DeleteAsync(id);
            return notify.CustomResponse();
        })
        //.CustomAuthorize("Admin,Manager", "UserRole", "DeleteUserRole")
        .AllowAnonymous()
        .WithName("DeleteUserRole")
        .WithSummary("Swagger.Endpoint.UserRole.DeleteUserRole.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
