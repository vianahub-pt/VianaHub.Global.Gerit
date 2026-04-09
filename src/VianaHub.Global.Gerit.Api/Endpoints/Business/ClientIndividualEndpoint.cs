using Microsoft.AspNetCore.Mvc;
using VianaHub.Global.Gerit.Api.Endpoints.Base;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientIndividual;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Endpoints.Business;

[EndpointMapper]
public static class ClientIndividualEndpoint
{
    public static void MapClientIndividualEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/clients/individuals").WithTags("ClientIndividuals").WithGroupName("v1").RequireAuthorization();

        groupV1.MapGet("/", async ([FromServices] IClientIndividualAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetAllAsync(ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientIndividuals", "GetAll")
        .WithName("GetClientIndividuals")
        .WithSummary("Swagger.Endpoint.ClientIndividual.GetAll.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{id}", async ([FromRoute] int id, [FromServices] IClientIndividualAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByIdAsync(id, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientIndividuals", "GetBy")
        .WithName("GetClientIndividualById")
        .WithSummary("Swagger.Endpoint.ClientIndividual.GetById.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/by-client/{clientId}", async ([FromRoute] int clientId, [FromServices] IClientIndividualAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByClientIdAsync(clientId, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientIndividuals", "GetBy")
        .WithName("GetClientIndividualByClientId")
        .WithSummary("Swagger.Endpoint.ClientIndividual.GetByClientId.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/by-document/{documentType}/{documentNumber}", async ([FromRoute] string documentType, [FromRoute] string documentNumber, [FromServices] IClientIndividualAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByDocumentAsync(documentType, documentNumber, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientIndividuals", "GetBy")
        .WithName("GetClientIndividualByDocument")
        .WithSummary("Swagger.Endpoint.ClientIndividual.GetByDocument.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/paged", async ([FromBody] PagedFilterRequest request, [FromServices] IClientIndividualAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetPagedAsync(request, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientIndividuals", "GetPaged")
        .WithName("GetClientIndividualsPaged")
        .WithSummary("Swagger.Endpoint.ClientIndividual.GetPaged.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/", async ([FromBody] CreateClientIndividualRequest request, [FromServices] IClientIndividualAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var created = await appService.CreateAsync(request, ct);
            return notify.CustomResponse(created, 201);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientIndividuals", "Create")
        .WithName("CreateClientIndividual")
        .WithSummary("Swagger.Endpoint.ClientIndividual.Create.Summary")
        .Produces(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<CreateClientIndividualRequest>();

        groupV1.MapPut("/{id}", async ([FromRoute] int id, [FromBody] UpdateClientIndividualRequest request, [FromServices] IClientIndividualAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var updated = await appService.UpdateAsync(id, request, ct);
            return notify.CustomResponse(updated, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientIndividuals", "Update")
        .WithName("UpdateClientIndividual")
        .WithSummary("Swagger.Endpoint.ClientIndividual.Update.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<UpdateClientIndividualRequest>();

        groupV1.MapPatch("/{id}/activate", async ([FromRoute] int id, [FromServices] IClientIndividualAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.ActivateAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientIndividuals", "Activate")
        .WithName("ActivateClientIndividual")
        .WithSummary("Swagger.Endpoint.ClientIndividual.Activate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/deactivate", async ([FromRoute] int id, [FromServices] IClientIndividualAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeactivateAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientIndividuals", "Deactivate")
        .WithName("DeactivateClientIndividual")
        .WithSummary("Swagger.Endpoint.ClientIndividual.Deactivate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapDelete("/{id}", async ([FromRoute] int id, [FromServices] IClientIndividualAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeleteAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientIndividuals", "Delete")
        .WithName("DeleteClientIndividual")
        .WithSummary("Swagger.Endpoint.ClientIndividual.Delete.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
