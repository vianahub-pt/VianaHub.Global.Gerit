using Microsoft.AspNetCore.Mvc;
using VianaHub.Global.Gerit.Api.Endpoints.Base;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientFiscalData;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Endpoints.Business;

[EndpointMapper]
public static class ClientFiscalDataEndpoint
{
    public static void MapClientFiscalDataEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/clients").WithTags("ClientFiscalData").WithGroupName("v1").RequireAuthorization();

        groupV1.MapGet("/{clientId}/fiscal-data", async ([FromRoute] int clientId, [FromServices] IClientFiscalDataAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetAllAsync(clientId, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientFiscalData", "GetAll")
        .WithName("GetClientFiscalData")
        .WithSummary("Swagger.Endpoint.ClientFiscalData.GetAll.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);


        groupV1.MapGet("/{clientId}/fiscal-data/{id}", async ([FromRoute] int clientId, [FromRoute] int id, [FromServices] IClientFiscalDataAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByIdAsync(clientId, id, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientFiscalData", "GetBy")
        .WithName("GetClientFiscalDataById")
        .WithSummary("Swagger.Endpoint.ClientFiscalData.GetById.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{clientId}/fiscal-data/paged", async ([FromRoute] int clientId, [AsParameters] PagedFilterRequest request, [FromServices] IClientFiscalDataAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetPagedAsync(clientId, request, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientFiscalData", "GetPaged")
        .WithName("GetClientFiscalDataPaged")
        .WithSummary("Swagger.Endpoint.ClientFiscalData.GetPaged.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/{clientId}/fiscal-data/", async ([FromRoute] int clientId, [FromBody] CreateClientFiscalDataRequest request, [FromServices] IClientFiscalDataAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var created = await appService.CreateAsync(clientId, request, ct);
            return notify.CustomResponse(created ? 201 : 400);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientFiscalData", "Create")
        .WithName("CreateClientFiscalData")
        .WithSummary("Swagger.Endpoint.ClientFiscalData.Create.Summary")
        .Produces(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<CreateClientFiscalDataRequest>();

        groupV1.MapPut("/{clientId}/fiscal-data/{id}", async ([FromRoute] int clientId, [FromRoute] int id, [FromBody] UpdateClientFiscalDataRequest request, [FromServices] IClientFiscalDataAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var updated = await appService.UpdateAsync(clientId, id, request, ct);
            return notify.CustomResponse(updated ? 204 : 400);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientFiscalData", "Update")
        .WithName("UpdateClientFiscalData")
        .WithSummary("Swagger.Endpoint.ClientFiscalData.Update.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<UpdateClientFiscalDataRequest>();

        groupV1.MapPatch("/{clientId}/fiscal-data/{id}/activate", async ([FromRoute] int clientId, [FromRoute] int id, [FromServices] IClientFiscalDataAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.ActivateAsync(clientId, id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientFiscalData", "Activate")
        .WithName("ActivateClientFiscalData")
        .WithSummary("Swagger.Endpoint.ClientFiscalData.Activate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{clientId}/fiscal-data/{id}/deactivate", async ([FromRoute] int clientId, [FromRoute] int id, [FromServices] IClientFiscalDataAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeactivateAsync(clientId, id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientFiscalData", "Deactivate")
        .WithName("DeactivateClientFiscalData")
        .WithSummary("Swagger.Endpoint.ClientFiscalData.Deactivate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapDelete("/{clientId}/fiscal-data/{id}", async ([FromRoute] int clientId, [FromRoute] int id, [FromServices] IClientFiscalDataAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeleteAsync(clientId, id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientFiscalData", "Delete")
        .WithName("DeleteClientFiscalData")
        .WithSummary("Swagger.Endpoint.ClientFiscalData.Delete.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
