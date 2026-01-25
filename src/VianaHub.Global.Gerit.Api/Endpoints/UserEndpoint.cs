using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.User;
using VianaHub.Global.Gerit.Application.Interfaces;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Endpoints;

public static class UserEndpoint
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/users").WithTags("Users").WithGroupName("v1").RequireAuthorization();

        groupV1.MapGet("/", async (IUserAppService appService, INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetAllAsync(ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,Manager,Operator", "Users", "GetAll")
        .AllowAnonymous()
        .WithName("GetAllUsers")
        .WithSummary("Swagger.Endpoint.User.GetAllUsers.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{id}", async (int id, IUserAppService appService, INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByIdAsync(id, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,Manager,Operator", "Users", "GetBy")
        .AllowAnonymous()
        .WithName("GetUserById")
        .WithSummary("Swagger.Endpoint.User.GetUserById.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/paged", async ([AsParameters] PagedFilterRequest request, IUserAppService appService, INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetPagedAsync(request, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,Manager,Operator", "User", "GetPaged")
        .AllowAnonymous()
        .WithName("GetUsersPaged")
        .WithSummary("Swagger.Endpoint.User.GetUsersPaged.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/", async ([FromBody] CreateUserRequest request, IUserAppService appService, INotify notify, CancellationToken ct) =>
        {
            var created = await appService.CreateAsync(request, ct);
            return notify.CustomResponse(201);
        })
        .CustomAuthorize("Admin,Manager", "Users", "Create")
        .AllowAnonymous()
        .WithName("CreateUser")
        .WithSummary("Swagger.Endpoint.User.CreateUser.Summary")
        .Produces(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<CreateUserRequest>();

        groupV1.MapPut("/{id}", async (int id, [FromBody] UpdateUserRequest request, IUserAppService appService, INotify notify, CancellationToken ct) =>
        {
            var updated = await appService.UpdateAsync(id, request, ct);
            return notify.CustomResponse(updated, 200);
        })
        .CustomAuthorize("Admin,Manager", "Users", "Update")
        .AllowAnonymous()
        .WithName("UpdateUser")
        .WithSummary("Swagger.Endpoint.User.UpdateUser.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<UpdateUserRequest>();

        groupV1.MapPatch("/{id}/password", async (int id, [FromBody] UpdatePasswordRequest request, IUserAppService appService, INotify notify, CancellationToken ct) =>
        {
            var updated = await appService.UpdatePasswordAsync(id, request, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,Manager,User", "Users", "Update")
        .AllowAnonymous()
        .WithName("UpdateUserPassword")
        .WithSummary("Swagger.Endpoint.User.UpdateUserPassword.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/activate", async (int id, IUserAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.ActivateAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,Manager", "Users", "Activate")
        .AllowAnonymous()
        .WithName("ActivateUser")
        .WithSummary("Swagger.Endpoint.User.ActivateUser.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/deactivate", async (int id, IUserAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeactivateAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,Manager", "Users", "Deactivate")
        .AllowAnonymous()
        .WithName("DeactivateUser")
        .WithSummary("Swagger.Endpoint.User.DeactivateUser.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapDelete("/{id}", async (int id, IUserAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeleteAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin", "Users", "Delete")
        .AllowAnonymous()
        .WithName("DeleteUser")
        .WithSummary("Swagger.Endpoint.User.DeleteUser.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/bulk-upload", async (HttpRequest request, IUserAppService appService, INotify notify, CancellationToken ct) =>
        {
            if (!request.HasFormContentType || request.Form.Files.Count == 0)
            {
                notify.Add("Nenhum arquivo foi enviado", 400);
                return notify.CustomResponse();
            }

            var file = request.Form.Files[0];
            var success = await appService.BulkUploadAsync(file, ct);
            return notify.CustomResponse(success ? 200 : 400);
        })
        .CustomAuthorize("Admin", "Users", "BulkUpload")
        .AllowAnonymous()
        .WithName("BulkUploadUsers")
        .WithSummary("Swagger.Endpoint.User.BulkUploadUsers.Summary")
        .DisableAntiforgery()
        .Accepts<IFormFile>("multipart/form-data")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
