using Microsoft.AspNetCore.Mvc;
using VianaHub.Global.Gerit.Api.Endpoints.Base;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ConsentOriginType;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Endpoints.Business;

[EndpointMapper]
public static class ConsentOriginTypeEndpoint
{
    public static void MapConsentOriginTypeEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/consent-origin-types").WithTags("ConsentOriginTypes").WithGroupName("v1").RequireAuthorization();

        groupV1.MapGet("/", async ([FromServices] IConsentOriginTypeAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetAllAsync(ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ConsentOriginTypes", "GetAll")
        .WithName("GetConsentOriginTypes")
        .WithSummary("Swagger.Endpoint.ConsentOriginType.GetAll.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{id}", async ([FromRoute] int id, [FromServices] IConsentOriginTypeAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByIdAsync(id, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ConsentOriginTypes", "GetBy")
        .WithName("GetConsentOriginTypeById")
        .WithSummary("Swagger.Endpoint.ConsentOriginType.GetById.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/paged", async ([AsParameters] PagedFilterRequest request, [FromServices] IConsentOriginTypeAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetPagedAsync(request, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ConsentOriginTypes", "GetPaged")
        .WithName("GetConsentOriginTypesPaged")
        .WithSummary("Swagger.Endpoint.ConsentOriginType.GetPaged.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/", async ([FromBody] CreateConsentOriginTypeRequest request, [FromServices] IConsentOriginTypeAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var id = await appService.CreateAsync(request, ct);
            return notify.CustomResponse(new GenericResponse { Id = id }, 201);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ConsentOriginTypes", "Create")
        .WithName("CreateConsentOriginType")
        .WithSummary("Swagger.Endpoint.ConsentOriginType.Create.Summary")
        .Produces<GenericResponse>(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<CreateConsentOriginTypeRequest>();

        groupV1.MapPut("/{id}", async ([FromRoute] int id, [FromBody] UpdateConsentOriginTypeRequest request, [FromServices] IConsentOriginTypeAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var updated = await appService.UpdateAsync(id, request, ct);
            return notify.CustomResponse(updated, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ConsentOriginTypes", "Update")
        .WithName("UpdateConsentOriginType")
        .WithSummary("Swagger.Endpoint.ConsentOriginType.Update.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<UpdateConsentOriginTypeRequest>();

        groupV1.MapPatch("/{id}/activate", async ([FromRoute] int id, [FromServices] IConsentOriginTypeAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            await appService.ActivateAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ConsentOriginTypes", "Activate")
        .WithName("ActivateConsentOriginType")
        .WithSummary("Swagger.Endpoint.ConsentOriginType.Activate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/deactivate", async ([FromRoute] int id, [FromServices] IConsentOriginTypeAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            await appService.DeactivateAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ConsentOriginTypes", "Deactivate")
        .WithName("DeactivateConsentOriginType")
        .WithSummary("Swagger.Endpoint.ConsentOriginType.Deactivate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapDelete("/{id}", async ([FromRoute] int id, [FromServices] IConsentOriginTypeAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            await appService.DeleteAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "ConsentOriginTypes", "Delete")
        .WithName("DeleteConsentOriginType")
        .WithSummary("Swagger.Endpoint.ConsentOriginType.Delete.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
