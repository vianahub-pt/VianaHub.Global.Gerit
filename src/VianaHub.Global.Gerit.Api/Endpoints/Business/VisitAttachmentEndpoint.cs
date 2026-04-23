using Microsoft.AspNetCore.Mvc;
using VianaHub.Global.Gerit.Api.Endpoints.Base;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitAttachment;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Endpoints.Business;

[EndpointMapper]
public static class VisitAttachmentEndpoint
{
    public static void MapVisitAttachmentEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/visit-attachments").WithTags("VisitAttachments").WithGroupName("v1").RequireAuthorization();

        groupV1.MapGet("/", async ([FromServices] IVisitAttachmentAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetAllAsync(ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitAttachments", "GetAll")
        .WithName("GetVisitAttachments")
        .WithSummary("Swagger.Endpoint.VisitAttachment.GetAll.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{id}", async ([FromRoute] int id, [FromServices] IVisitAttachmentAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByIdAsync(id, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitAttachments", "GetBy")
        .WithName("GetVisitAttachmentById")
        .WithSummary("Swagger.Endpoint.VisitAttachment.GetById.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/public/{publicId}", async ([FromRoute] Guid publicId, [FromServices] IVisitAttachmentAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByPublicIdAsync(publicId, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitAttachments", "GetBy")
        .WithName("GetVisitAttachmentByPublicId")
        .WithSummary("Swagger.Endpoint.VisitAttachment.GetByPublicId.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/visit/{visitId}", async ([FromRoute] int visitId, [FromServices] IVisitAttachmentAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByVisitIdAsync(visitId, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitAttachments", "GetBy")
        .WithName("GetVisitAttachmentsByVisitId")
        .WithSummary("Swagger.Endpoint.VisitAttachment.GetByVisitId.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/category/{categoryId}", async ([FromRoute] int categoryId, [FromServices] IVisitAttachmentAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByCategoryIdAsync(categoryId, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitAttachments", "GetBy")
        .WithName("GetVisitAttachmentsByCategoryId")
        .WithSummary("Swagger.Endpoint.VisitAttachment.GetByCategoryId.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/visit/{visitId}/primary", async ([FromRoute] int visitId, [FromServices] IVisitAttachmentAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetPrimaryByVisitIdAsync(visitId, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitAttachments", "GetBy")
        .WithName("GetPrimaryVisitAttachment")
        .WithSummary("Swagger.Endpoint.VisitAttachment.GetPrimaryByVisitId.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/paged", async ([FromBody] PagedFilterRequest request, [FromServices] IVisitAttachmentAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetPagedAsync(request, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitAttachments", "GetPaged")
        .WithName("GetVisitAttachmentsPaged")
        .WithSummary("Swagger.Endpoint.VisitAttachment.GetPaged.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/", async ([FromBody] CreateVisitAttachmentRequest request, [FromServices] IVisitAttachmentAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var created = await appService.CreateAsync(request, ct);
            return notify.CustomResponse(created, 201);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitAttachments", "Create")
        .WithName("CreateVisitAttachment")
        .WithSummary("Swagger.Endpoint.VisitAttachment.Create.Summary")
        .Produces(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<CreateVisitAttachmentRequest>();

        groupV1.MapPut("/{id}", async ([FromRoute] int id, [FromBody] UpdateVisitAttachmentRequest request, [FromServices] IVisitAttachmentAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var updated = await appService.UpdateAsync(id, request, ct);
            return notify.CustomResponse(updated, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitAttachments", "Update")
        .WithName("UpdateVisitAttachment")
        .WithSummary("Swagger.Endpoint.VisitAttachment.Update.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<UpdateVisitAttachmentRequest>();

        groupV1.MapPatch("/{id}/set-primary", async ([FromRoute] int id, [FromServices] IVisitAttachmentAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.SetAsPrimaryAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitAttachments", "SetPrimary")
        .WithName("SetVisitAttachmentAsPrimary")
        .WithSummary("Swagger.Endpoint.VisitAttachment.SetAsPrimary.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/activate", async ([FromRoute] int id, [FromServices] IVisitAttachmentAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.ActivateAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitAttachments", "Activate")
        .WithName("ActivateVisitAttachment")
        .WithSummary("Swagger.Endpoint.VisitAttachment.Activate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/deactivate", async ([FromRoute] int id, [FromServices] IVisitAttachmentAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeactivateAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitAttachments", "Deactivate")
        .WithName("DeactivateVisitAttachment")
        .WithSummary("Swagger.Endpoint.VisitAttachment.Deactivate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapDelete("/{id}", async ([FromRoute] int id, [FromServices] IVisitAttachmentAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeleteAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitAttachments", "Delete")
        .WithName("DeleteVisitAttachment")
        .WithSummary("Swagger.Endpoint.VisitAttachment.Delete.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
