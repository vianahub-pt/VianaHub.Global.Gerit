using Microsoft.AspNetCore.Mvc;
using VianaHub.Global.Gerit.Api.Endpoints.Base;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientHierarchy;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Endpoints.Business;

[EndpointMapper]
public static class ClientHierarchyEndpoint
{
    public static void MapClientHierarchyEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/clients/hierarchies").WithTags("ClientHierarchies").WithGroupName("v1").RequireAuthorization();

        groupV1.MapGet("/", async ([FromServices] IClientHierarchyAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetAllAsync(ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientHierarchies", "GetAll")
        .WithName("GetClientHierarchies")
        .WithSummary("Swagger.Endpoint.ClientHierarchy.GetAll.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{id}", async ([FromRoute] int id, [FromServices] IClientHierarchyAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByIdAsync(id, ct);
            return notify.CustomResponse(response);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientHierarchies", "GetBy")
        .WithName("GetClientHierarchyById")
        .WithSummary("Swagger.Endpoint.ClientHierarchy.GetById.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/by-parent/{parentClientId}", async ([FromRoute] int parentClientId, [FromServices] IClientHierarchyAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByParentClientIdAsync(parentClientId, ct);
            return notify.CustomResponse(response);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientHierarchies", "GetByParent")
        .WithName("GetClientHierarchyByParentClientId")
        .WithSummary("Swagger.Endpoint.ClientHierarchy.GetByParentClientId.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/by-child/{childClientId}", async ([FromRoute] int childClientId, [FromServices] IClientHierarchyAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByChildClientIdAsync(childClientId, ct);
            return notify.CustomResponse(response);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientHierarchies", "GetByChild")
        .WithName("GetClientHierarchyByChildClientId")
        .WithSummary("Swagger.Endpoint.ClientHierarchy.GetByChildClientId.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/paged", async ([AsParameters] PagedFilterRequest request, [FromServices] IClientHierarchyAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetPagedAsync(request, ct);
            return notify.CustomResponse(response);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientHierarchies", "GetPaged")
        .WithName("GetClientHierarchiesPaged")
        .WithSummary("Swagger.Endpoint.ClientHierarchy.GetPaged.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/", async ([FromBody] CreateClientHierarchyRequest request, [FromServices] IClientHierarchyAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var created = await appService.CreateAsync(request, ct);
            return notify.CustomResponse(created, 201);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientHierarchies", "Create")
        .WithName("CreateClientHierarchy")
        .WithSummary("Swagger.Endpoint.ClientHierarchy.Create.Summary")
        .Produces(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<CreateClientHierarchyRequest>();

        groupV1.MapPut("/{id}", async ([FromRoute] int id, [FromBody] UpdateClientHierarchyRequest request, [FromServices] IClientHierarchyAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var updated = await appService.UpdateAsync(id, request, ct);
            return notify.CustomResponse(updated);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientHierarchies", "Update")
        .WithName("UpdateClientHierarchy")
        .WithSummary("Swagger.Endpoint.ClientHierarchy.Update.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<UpdateClientHierarchyRequest>();

        groupV1.MapPatch("/{id}/activate", async ([FromRoute] int id, [FromServices] IClientHierarchyAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.ActivateAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientHierarchies", "Activate")
        .WithName("ActivateClientHierarchy")
        .WithSummary("Swagger.Endpoint.ClientHierarchy.Activate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status410Gone)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/deactivate", async ([FromRoute] int id, [FromServices] IClientHierarchyAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeactivateAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientHierarchies", "Deactivate")
        .WithName("DeactivateClientHierarchy")
        .WithSummary("Swagger.Endpoint.ClientHierarchy.Deactivate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapDelete("/{id}", async ([FromRoute] int id, [FromServices] IClientHierarchyAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeleteAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ClientHierarchies", "Delete")
        .WithName("DeleteClientHierarchy")
        .WithSummary("Swagger.Endpoint.ClientHierarchy.Delete.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status410Gone)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
