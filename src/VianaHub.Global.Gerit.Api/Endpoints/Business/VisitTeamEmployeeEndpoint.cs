using Microsoft.AspNetCore.Mvc;
using VianaHub.Global.Gerit.Api.Endpoints.Base;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitTeamEmployee;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Endpoints.Business;

[EndpointMapper]
public static class VisitTeamEmployeeEndpoint
{
    public static void MapVisitTeamEmployeeEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/visit-team-employees").WithTags("VisitTeamEmployees").WithGroupName("v1").RequireAuthorization();

        groupV1.MapGet("/", async ([FromServices] IVisitTeamEmployeeAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetAllAsync(ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitTeamEmployees", "GetAll")
        .WithName("GetVisitTeamEmployees")
        .WithSummary("Swagger.Endpoint.VisitTeamEmployee.GetAll.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{id}", async ([FromRoute] int id, [FromServices] IVisitTeamEmployeeAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByIdAsync(id, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitTeamEmployees", "GetBy")
        .WithName("GetVisitTeamEmployeeById")
        .WithSummary("Swagger.Endpoint.VisitTeamEmployee.GetById.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/visit-team/{visitTeamId}", async ([FromRoute] int visitTeamId, [FromServices] IVisitTeamEmployeeAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByVisitTeamIdAsync(visitTeamId, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitTeamEmployees", "GetByVisitTeam")
        .WithName("GetVisitTeamEmployeesByVisitTeamId")
        .WithSummary("Swagger.Endpoint.VisitTeamEmployee.GetByVisitTeamId.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/visit-team/{visitTeamId}/active", async ([FromRoute] int visitTeamId, [FromServices] IVisitTeamEmployeeAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetActiveByVisitTeamIdAsync(visitTeamId, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitTeamEmployees", "GetActiveByVisitTeam")
        .WithName("GetActiveVisitTeamEmployeesByVisitTeamId")
        .WithSummary("Swagger.Endpoint.VisitTeamEmployee.GetActiveByVisitTeamId.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/employee/{employeeId}", async ([FromRoute] int employeeId, [FromServices] IVisitTeamEmployeeAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByEmployeeIdAsync(employeeId, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitTeamEmployees", "GetByEmployee")
        .WithName("GetVisitTeamEmployeesByEmployeeId")
        .WithSummary("Swagger.Endpoint.VisitTeamEmployee.GetByEmployeeId.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/paged", async ([AsParameters] PagedFilterRequest request, [FromServices] IVisitTeamEmployeeAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetPagedAsync(request, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitTeamEmployees", "GetPaged")
        .WithName("GetVisitTeamEmployeesPaged")
        .WithSummary("Swagger.Endpoint.VisitTeamEmployee.GetPaged.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/", async ([FromBody] CreateVisitTeamEmployeeRequest request, [FromServices] IVisitTeamEmployeeAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var created = await appService.CreateAsync(request, ct);
            return notify.CustomResponse(created ? 201 : 400);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitTeamEmployees", "Create")
        .WithName("CreateVisitTeamEmployee")
        .WithSummary("Swagger.Endpoint.VisitTeamEmployee.Create.Summary")
        .Produces(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<CreateVisitTeamEmployeeRequest>();

        groupV1.MapPut("/{id}", async ([FromRoute] int id, [FromBody] UpdateVisitTeamEmployeeRequest request, [FromServices] IVisitTeamEmployeeAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var updated = await appService.UpdateAsync(id, request, ct);
            return notify.CustomResponse(updated ? 204 : 400);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitTeamEmployees", "Update")
        .WithName("UpdateVisitTeamEmployee")
        .WithSummary("Swagger.Endpoint.VisitTeamEmployee.Update.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<UpdateVisitTeamEmployeeRequest>();

        groupV1.MapPatch("/{id}/activate", async ([FromRoute] int id, [FromServices] IVisitTeamEmployeeAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.ActivateAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitTeamEmployees", "Activate")
        .WithName("ActivateVisitTeamEmployee")
        .WithSummary("Swagger.Endpoint.VisitTeamEmployee.Activate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/deactivate", async ([FromRoute] int id, [FromServices] IVisitTeamEmployeeAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeactivateAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitTeamEmployees", "Deactivate")
        .WithName("DeactivateVisitTeamEmployee")
        .WithSummary("Swagger.Endpoint.VisitTeamEmployee.Deactivate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapDelete("/{id}", async ([FromRoute] int id, [FromServices] IVisitTeamEmployeeAppService appService, [FromServices] INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeleteAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "VisitTeamEmployees", "Delete")
        .WithName("DeleteVisitTeamEmployee")
        .WithSummary("Swagger.Endpoint.VisitTeamEmployee.Delete.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
