using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VianaHub.Global.Gerit.Api.Endpoints.Base;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Identity.UserPreferences;
using VianaHub.Global.Gerit.Application.Interfaces.Identity;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Endpoints.Identity;

[EndpointMapper]
public static class UserPreferencesEndpoint
{
    public static void MapUserPreferencesEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/user-preferences").WithTags("UserPreferences").WithGroupName("v1").RequireAuthorization();

        groupV1.MapGet("/", async ([FromServices] IUserPreferencesAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetAllAsync(ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager,Operator,User", "UserPreferences", "GetAll")
        .AllowAnonymous()
        .WithName("GetAllUserPreferences")
        .WithSummary("Swagger.Endpoint.UserPreferences.GetAll.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{id}", async ([FromRoute] int id, [FromServices] IUserPreferencesAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByIdAsync(id, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager,Operator,User", "UserPreferences", "GetBy")
        .AllowAnonymous()
        .WithName("GetUserPreferencesById")
        .WithSummary("Swagger.Endpoint.UserPreferences.GetById.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/user/{userId}", async ([FromRoute] int userId, [FromServices] IUserPreferencesAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByUserAsync(userId, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager,Operator,User", "UserPreferences", "GetBy")
        .AllowAnonymous()
        .WithName("GetUserPreferencesByUser")
        .WithSummary("Swagger.Endpoint.UserPreferences.GetByUser.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/paged", async ([AsParameters] PagedFilterRequest request, [FromServices] IUserPreferencesAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetPagedAsync(request, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager,Operator,User", "UserPreferences", "GetPaged")
        .AllowAnonymous()
        .WithName("GetUserPreferencesPaged")
        .WithSummary("Swagger.Endpoint.UserPreferences.GetPaged.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/", async ([FromBody] CreateUserPreferencesRequest request, [FromServices] IUserPreferencesAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var created = await appService.CreateAsync(request, ct);
            return notify.CustomResponse(created ? 201 : 400);
        })
        .CustomAuthorize("Admin,BackOffice,Manager,Operator,User", "UserPreferences", "Create")
        .AllowAnonymous()
        .WithName("CreateUserPreferences")
        .WithSummary("Swagger.Endpoint.UserPreferences.Create.Summary")
        .Produces(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<CreateUserPreferencesRequest>();

        groupV1.MapPut("/{id}", async ([FromRoute] int id, [FromBody] UpdateUserPreferencesRequest request, [FromServices] IUserPreferencesAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var updated = await appService.UpdateAsync(id, request, ct);
            return notify.CustomResponse(updated, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager,Operator,User", "UserPreferences", "Update")
        .AllowAnonymous()
        .WithName("UpdateUserPreferences")
        .WithSummary("Swagger.Endpoint.UserPreferences.Update.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<UpdateUserPreferencesRequest>();

        groupV1.MapPatch("/{id}/activate", async ([FromRoute] int id, [FromServices] IUserPreferencesAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.ActivateAsync(id, ct);
            return notify.CustomResponse(ok);
        })
        .CustomAuthorize("Admin,BackOffice,Manager,Operator,User", "UserPreferences", "Activate")
        .AllowAnonymous()
        .WithName("ActivateUserPreferences")
        .WithSummary("Swagger.Endpoint.UserPreferences.Activate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/deactivate", async ([FromRoute] int id, [FromServices] IUserPreferencesAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeactivateAsync(id, ct);
            return notify.CustomResponse(ok);
        })
        .CustomAuthorize("Admin,BackOffice,Manager,Operator,User", "UserPreferences", "Deactivate")
        .AllowAnonymous()
        .WithName("DeactivateUserPreferences")
        .WithSummary("Swagger.Endpoint.UserPreferences.Deactivate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapDelete("/{id}", async ([FromRoute] int id, [FromServices] IUserPreferencesAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeleteAsync(id, ct);
            return notify.CustomResponse(ok);
        })
        .CustomAuthorize("Admin,BackOffice,Manager,Operator,User", "UserPreferences", "Delete")
        .AllowAnonymous()
        .WithName("DeleteUserPreferences")
        .WithSummary("Swagger.Endpoint.UserPreferences.Delete.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
