using Microsoft.AspNetCore.Mvc;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Action;
using VianaHub.Global.Gerit.Application.Interfaces;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Endpoints;

public static class ActionEndpoint
{
    public static void MapActionEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/actions").WithTags("Actions").WithGroupName("v1");

        groupV1.MapGet("/", async (IActionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetAllAsync(ct);
            return notify.CustomResponse(response, 200);
        })
        //.CustomAuthorize("Admin,Manager,Operator", "Action", "GetAllActions")
        .WithName("GetAllActions")
        .WithSummary("Swagger.Endpoint.Action.GetAllActions.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/paged", async ([AsParameters] PagedFilterRequest request, IActionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetPagedAsync(request, ct);
            return notify.CustomResponse(response, 200);
        })
        //.CustomAuthorize("Admin,Manager,Operator", "Action", "GetActionsPaged")
        .WithName("GetActionsPaged")
        .WithSummary("Swagger.Endpoint.Action.GetActionsPaged.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{id}", async (int id, IActionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByIdAsync(id, ct);
            return notify.CustomResponse(response, 200);
        })
        //.CustomAuthorize("Admin,Manager,Operator", "Action", "GetActionById")
        .WithName("GetActionById")
        .WithSummary("Swagger.Endpoint.Action.GetActionById.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/", async ([FromBody] CreateActionRequest request, IActionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var created = await appService.CreateAsync(request, ct);
            return notify.CustomResponse(created ? 201 : 400);
        })
        //.CustomAuthorize("Admin,Manager,Operator", "Action", "CreateAction")
        .WithName("CreateAction")
        .WithSummary("Swagger.Endpoint.Action.CreateAction.Summary")
        .Produces(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPut("/{id}", async (int id, [FromBody] UpdateActionRequest request, IActionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var updated = await appService.UpdateAsync(id, request, ct);
            return notify.CustomResponse(updated, 200);
        })
        //.CustomAuthorize("Admin,Manager,Operator", "Action", "UpdateAction")
        .WithName("UpdateAction")
        .WithSummary("Swagger.Endpoint.Action.UpdateAction.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/activate", async (int id, IActionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.ActivateAsync(id, ct);
            return notify.CustomResponse();
        })
        //.CustomAuthorize("Admin,Manager", "Action", "ActivateAction")
        .WithName("ActivateAction")
        .WithSummary("Swagger.Endpoint.Action.ActivateAction.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/deactivate", async (int id, IActionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeactivateAsync(id, ct);
            return notify.CustomResponse();
        })
        //.CustomAuthorize("Admin,Manager", "Action", "DeactivateAction")
        .WithName("DeactivateAction")
        .WithSummary("Swagger.Endpoint.Action.DeactivateAction.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapDelete("/{id}", async (int id, IActionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeleteAsync(id, ct);
            return notify.CustomResponse();
        })
        //.CustomAuthorize("Admin,Manager", "Action", "DeleteAction")
        .WithName("DeleteAction")
        .WithSummary("Swagger.Endpoint.Action.DeleteAction.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        // Upload massivo de actions via CSV
        groupV1.MapPost("/bulk-upload", async (HttpRequest request, IActionAppService appService, INotify notify, CancellationToken ct) =>
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
        //.CustomAuthorize("Admin,Manager", "Action", "BulkUploadActions")
        .WithName("BulkUploadActions")
        .WithSummary("Swagger.Endpoint.Action.BulkUploadActions.Summary")
        .DisableAntiforgery()
        .Accepts<IFormFile>("multipart/form-data")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
