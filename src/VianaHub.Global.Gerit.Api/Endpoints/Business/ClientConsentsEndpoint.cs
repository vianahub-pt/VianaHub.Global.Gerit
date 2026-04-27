using Microsoft.AspNetCore.Mvc;
using VianaHub.Global.Gerit.Api.Endpoints.Base;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientConsents;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Endpoints.Business;

[EndpointMapper]
public static class ClientConsentEndpoint
{
    public static void MapClientConsentEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/clients").WithTags("ClientConsents").WithGroupName("v1").RequireAuthorization();

        groupV1.MapGet("/{clientId}/consents", async ([FromRoute] int clientId, [FromServices] IClientConsentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetAllAsync(clientId, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientConsents", "GetAll")
        .WithName("GetClientConsents")
        .WithSummary("Swagger.Endpoint.ClientConsents.GetAll.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{clientId}/consents/{id}", async ([FromRoute] int clientId, [FromRoute] int id, [FromServices] IClientConsentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByIdAsync(clientId, id, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientConsents", "GetBy")
        .WithName("GetClientConsentsById")
        .WithSummary("Swagger.Endpoint.ClientConsents.GetById.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{clientId}/consents/paged", async ([FromRoute] int clientId, [AsParameters] PagedFilterRequest request, [FromServices] IClientConsentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetPagedAsync(clientId, request, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientConsents", "GetPaged")
        .WithName("GetClientConsentsPaged")
        .WithSummary("Swagger.Endpoint.ClientConsents.GetPaged.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/{clientId}/consents", async ([FromRoute] int clientId, [FromBody] CreateClientConsentsRequest request, [FromServices] IClientConsentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var created = await appService.CreateAsync(clientId, request, ct);
            return notify.CustomResponse(created ? 201 : 400);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientConsents", "Create")
        .WithName("CreateClientConsents")
        .WithSummary("Swagger.Endpoint.ClientConsents.Create.Summary")
        .Produces(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status409Conflict)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<CreateClientConsentsRequest>();

        groupV1.MapPut("/{clientId}/consents/{id}", async ([FromRoute] int clientId, [FromRoute] int id, [FromBody] UpdateClientConsentsRequest request, [FromServices] IClientConsentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var updated = await appService.UpdateAsync(clientId, id, request, ct);
            return notify.CustomResponse(updated ? 204 : 400);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientConsents", "Update")
        .WithName("UpdateClientConsents")
        .WithSummary("Swagger.Endpoint.ClientConsents.Update.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status409Conflict)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<UpdateClientConsentsRequest>();

        groupV1.MapPatch("/{clientId}/consents/{id}/activate", async ([FromRoute] int clientId, [FromRoute] int id, [FromServices] IClientConsentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.ActivateAsync(clientId, id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientConsents", "Activate")
        .WithName("ActivateClientConsents")
        .WithSummary("Swagger.Endpoint.ClientConsents.Activate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{clientId}/consents/{id}/deactivate", async ([FromRoute] int clientId, [FromRoute] int id, [FromServices] IClientConsentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeactivateAsync(clientId, id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientConsents", "Deactivate")
        .WithName("DeactivateClientConsents")
        .WithSummary("Swagger.Endpoint.ClientConsents.Deactivate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapDelete("/{clientId}/consents/{id}", async ([FromRoute] int clientId, [FromRoute] int id, [FromServices] IClientConsentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeleteAsync(clientId, id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientConsents", "Delete")
        .WithName("DeleteClientConsents")
        .WithSummary("Swagger.Endpoint.ClientConsents.Delete.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
