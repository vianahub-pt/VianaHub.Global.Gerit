using Microsoft.AspNetCore.Mvc;
using VianaHub.Global.Gerit.Api.Endpoints.Base;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Billing.TenantContact;
using VianaHub.Global.Gerit.Application.Interfaces.Billing;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Endpoints.Billing;

[EndpointMapper]
public static class TenantContactEndpoint
{
    public static void MapTenantContactEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/tenants").WithTags("TenantContacts").WithGroupName("v1").RequireAuthorization();

        groupV1.MapGet("/{tenantId}/contacts", async ([FromRoute] int tenantId, [FromServices] ITenantContactAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByTenantIdAsync(tenantId, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "TenantContacts", "GetAll")
        .WithName("GetTenantContacts")
        .WithSummary("Swagger.Endpoint.TenantContact.GetAll.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{tenantId}/contacts/{id}", async ([FromRoute] int tenantId, [FromRoute] int id, [FromServices] ITenantContactAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByIdAsync(id, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "TenantContacts", "GetBy")
        .WithName("GetTenantContactById")
        .WithSummary("Swagger.Endpoint.TenantContact.GetById.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status410Gone)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{tenantId}/contacts/primary", async ([FromRoute] int tenantId, [FromServices] ITenantContactAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetPrimaryByTenantIdAsync(tenantId, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "TenantContacts", "GetPrimary")
        .WithName("GetPrimaryTenantContact")
        .WithSummary("Swagger.Endpoint.TenantContact.GetPrimary.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{tenantId}/contacts/paged", async ([FromRoute] int tenantId, [AsParameters] PagedFilterRequest request, [FromServices] ITenantContactAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetPagedAsync(request, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "TenantContacts", "GetPaged")
        .WithName("GetTenantContactsPaged")
        .WithSummary("Swagger.Endpoint.TenantContact.GetPaged.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/{tenantId}/contacts/", async ([FromRoute] int tenantId, [FromBody] CreateTenantContactRequest request, [FromServices] ITenantContactAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var id = await appService.CreateAsync(tenantId, request, ct);
            return notify.CustomResponse(new GenericResponse { Id = id }, 201);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "TenantContacts", "Create")
        .WithName("CreateTenantContact")
        .WithSummary("Swagger.Endpoint.TenantContact.Create.Summary")
        .Produces<GenericResponse>(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status409Conflict)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<CreateTenantContactRequest>();

        groupV1.MapPut("/{tenantId}/contacts/{id}", async ([FromRoute] int tenantId, [FromRoute] int id, [FromBody] UpdateTenantContactRequest request, [FromServices] ITenantContactAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var updated = await appService.UpdateAsync(tenantId, id, request, ct);
            return notify.CustomResponse(updated, 204);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "TenantContacts", "Update")
        .WithName("UpdateTenantContact")
        .WithSummary("Swagger.Endpoint.TenantContact.Update.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status410Gone)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<UpdateTenantContactRequest>();

        groupV1.MapPatch("/{tenantId}/contacts/{id}/set-primary", async ([FromRoute] int tenantId, [FromRoute] int id, [FromServices] ITenantContactAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.SetAsPrimaryAsync(tenantId, id, ct);
            return notify.CustomResponse(ok, 204);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "TenantContacts", "SetAsPrimary")
        .WithName("SetPrimaryTenantContact")
        .WithSummary("Swagger.Endpoint.TenantContact.SetAsPrimary.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status410Gone)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{tenantId}/contacts/{id}/activate", async ([FromRoute] int tenantId, [FromRoute] int id, [FromServices] ITenantContactAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.ActivateAsync(tenantId, id, ct);
            return notify.CustomResponse(ok, 204);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "TenantContacts", "Activate")
        .WithName("ActivateTenantContact")
        .WithSummary("Swagger.Endpoint.TenantContact.Activate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status410Gone)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{tenantId}/contacts/{id}/deactivate", async ([FromRoute] int tenantId, [FromRoute] int id, [FromServices] ITenantContactAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeactivateAsync(tenantId, id, ct);
            return notify.CustomResponse(ok, 204);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "TenantContacts", "Deactivate")
        .WithName("DeactivateTenantContact")
        .WithSummary("Swagger.Endpoint.TenantContact.Deactivate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status410Gone)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapDelete("/{tenantId}/contacts/{id}", async ([FromRoute] int tenantId, [FromRoute] int id, [FromServices] ITenantContactAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeleteAsync(tenantId, id, ct);
            return notify.CustomResponse(ok, 204);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "TenantContacts", "Delete")
        .WithName("DeleteTenantContact")
        .WithSummary("Swagger.Endpoint.TenantContact.Delete.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status410Gone)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
