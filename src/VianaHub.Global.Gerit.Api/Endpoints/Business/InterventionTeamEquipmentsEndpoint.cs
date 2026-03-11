using Microsoft.AspNetCore.Mvc;
using VianaHub.Global.Gerit.Api.Endpoints.Base;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionTeamEquipments;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Endpoints.Business;

[EndpointMapper]
public static class InterventionTeamEquipmentsEndpoint
{
    public static void MapInterventionTeamEquipmentsEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/intervention-team-equipments").WithTags("InterventionTeamEquipments").WithGroupName("v1").RequireAuthorization();

        groupV1.MapGet("/", async ([FromServices] IInterventionTeamEquipmentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetAllAsync(ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "InterventionTeamEquipments", "GetAll")
        .WithName("GetInterventionTeamEquipments")
        .WithSummary("Swagger.Endpoint.InterventionTeamEquipment.GetAll.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{id}", async ([FromRoute] int id, [FromServices] IInterventionTeamEquipmentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByIdAsync(id, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "InterventionTeamEquipments", "GetBy")
        .WithName("GetInterventionTeamEquipmentById")
        .WithSummary("Swagger.Endpoint.InterventionTeamEquipment.GetById.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status410Gone)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/paged", async ([AsParameters] PagedFilterRequest request, [FromServices] IInterventionTeamEquipmentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetPagedAsync(request, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "InterventionTeamEquipments", "GetPaged")
        .WithName("GetInterventionTeamEquipmentsPaged")
        .WithSummary("Swagger.Endpoint.InterventionTeamEquipment.GetPaged.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/", async ([FromBody] CreateInterventionTeamEquipmentRequest request, [FromServices] IInterventionTeamEquipmentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var created = await appService.CreateAsync(request, ct);
            return notify.CustomResponse(created, 201);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "InterventionTeamEquipments", "Create")
        .WithName("CreateInterventionTeamEquipment")
        .WithSummary("Swagger.Endpoint.InterventionTeamEquipment.Create.Summary")
        .Produces(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status409Conflict)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<CreateInterventionTeamEquipmentRequest>();

        groupV1.MapPut("/{id}", async ([FromRoute] int id, [FromBody] UpdateInterventionTeamEquipmentRequest request, [FromServices] IInterventionTeamEquipmentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var updated = await appService.UpdateAsync(id, request, ct);
            return notify.CustomResponse(updated, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "InterventionTeamEquipments", "Update")
        .WithName("UpdateInterventionTeamEquipment")
        .WithSummary("Swagger.Endpoint.InterventionTeamEquipment.Update.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status410Gone)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<UpdateInterventionTeamEquipmentRequest>();

        groupV1.MapPatch("/{id}/activate", async ([FromRoute] int id, [FromServices] IInterventionTeamEquipmentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.ActivateAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "InterventionTeamEquipments", "Activate")
        .WithName("ActivateInterventionTeamEquipment")
        .WithSummary("Swagger.Endpoint.InterventionTeamEquipment.Activate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status410Gone)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/deactivate", async ([FromRoute] int id, [FromServices] IInterventionTeamEquipmentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeactivateAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "InterventionTeamEquipments", "Deactivate")
        .WithName("DeactivateInterventionTeamEquipment")
        .WithSummary("Swagger.Endpoint.InterventionTeamEquipment.Deactivate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status410Gone)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapDelete("/{id}", async ([FromRoute] int id, [FromServices] IInterventionTeamEquipmentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeleteAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "InterventionTeamEquipments", "Delete")
        .WithName("DeleteInterventionTeamEquipment")
        .WithSummary("Swagger.Endpoint.InterventionTeamEquipment.Delete.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status410Gone)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/bulk-upload", async (HttpRequest request, [FromServices] IInterventionTeamEquipmentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            if (!request.HasFormContentType || request.Form.Files.Count == 0)
            {
                notify.Add("Api.Upload.NoFileProvided", 400);
                return notify.CustomResponse();
            }

            var file = request.Form.Files[0];
            var success = await appService.BulkUploadAsync(file, ct);
            return notify.CustomResponse(success);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "InterventionTeamEquipments", "BulkUpload")
        .WithName("BulkUploadInterventionTeamEquipments")
        .WithSummary("Swagger.Endpoint.InterventionTeamEquipment.BulkUpload.Summary")
        .DisableAntiforgery()
        .Accepts<IFormFile>("multipart/form-data")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
