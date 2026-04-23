using Microsoft.AspNetCore.Mvc;
using VianaHub.Global.Gerit.Api.Endpoints.Base;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientConsents;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Endpoints.Business;

[EndpointMapper]
public static class ClientConsentsEndpoint
{
    public static void MapClientConsentsEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/clients/consents").WithTags("ClientConsents").WithGroupName("v1").RequireAuthorization();

        groupV1.MapGet("/", async ([FromServices] IClientConsentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetAllAsync(ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientConsents", "GetAll")
        .WithName("GetClientConsents")
        .WithSummary("Swagger.Endpoint.ClientConsents.GetAll.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{id}", async ([FromRoute] int id, [FromServices] IClientConsentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByIdAsync(id, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientConsents", "GetBy")
        .WithName("GetClientConsentsById")
        .WithSummary("Swagger.Endpoint.ClientConsents.GetById.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/by-client/{clientId}", async ([FromRoute] int clientId, [FromServices] IClientConsentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByClientIdAsync(clientId, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientConsents", "GetBy")
        .WithName("GetClientConsentsByClientId")
        .WithSummary("Swagger.Endpoint.ClientConsents.GetByClientId.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/by-consent-type/{consentTypeId}", async ([FromRoute] int consentTypeId, [FromServices] IClientConsentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByConsentTypeIdAsync(consentTypeId, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientConsents", "GetBy")
        .WithName("GetClientConsentsByConsentTypeId")
        .WithSummary("Swagger.Endpoint.ClientConsents.GetByConsentTypeId.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/by-client/{clientId}/consent-type/{consentTypeId}", async ([FromRoute] int clientId, [FromRoute] int consentTypeId, [FromServices] IClientConsentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByClientAndConsentTypeAsync(clientId, consentTypeId, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientConsents", "GetBy")
        .WithName("GetClientConsentsByClientAndConsentType")
        .WithSummary("Swagger.Endpoint.ClientConsents.GetByClientAndConsentType.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/paged", async ([AsParameters] PagedFilterRequest request, [FromServices] IClientConsentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetPagedAsync(request, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientConsents", "GetPaged")
        .WithName("GetClientConsentsPaged")
        .WithSummary("Swagger.Endpoint.ClientConsents.GetPaged.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/", async ([FromBody] CreateClientConsentsRequest request, [FromServices] IClientConsentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var created = await appService.CreateAsync(request, ct);
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

        groupV1.MapPut("/{id}", async ([FromRoute] int id, [FromBody] UpdateClientConsentsRequest request, [FromServices] IClientConsentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var updated = await appService.UpdateAsync(id, request, ct);
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

        groupV1.MapPatch("/{id}/revoke", async ([FromRoute] int id, [FromServices] IClientConsentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var revoked = await appService.RevokeConsentAsync(id, ct);
            return notify.CustomResponse(revoked ? 200 : 400);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientConsents", "Revoke")
        .WithName("RevokeClientConsent")
        .WithSummary("Swagger.Endpoint.ClientConsents.Revoke.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/grant", async ([FromRoute] int id, [FromServices] IClientConsentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var granted = await appService.GrantConsentAsync(id, ct);
            return notify.CustomResponse(granted ? 200 : 400);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientConsents", "Grant")
        .WithName("GrantClientConsent")
        .WithSummary("Swagger.Endpoint.ClientConsents.Grant.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/activate", async ([FromRoute] int id, [FromServices] IClientConsentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.ActivateAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientConsents", "Activate")
        .WithName("ActivateClientConsents")
        .WithSummary("Swagger.Endpoint.ClientConsents.Activate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/deactivate", async ([FromRoute] int id, [FromServices] IClientConsentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeactivateAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientConsents", "Deactivate")
        .WithName("DeactivateClientConsents")
        .WithSummary("Swagger.Endpoint.ClientConsents.Deactivate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapDelete("/{id}", async ([FromRoute] int id, [FromServices] IClientConsentsAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeleteAsync(id, ct);
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
