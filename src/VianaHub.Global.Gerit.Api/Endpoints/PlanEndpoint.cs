using Microsoft.AspNetCore.Mvc;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Plan;
using VianaHub.Global.Gerit.Application.Interfaces;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using FluentValidation;

namespace VianaHub.Global.Gerit.Api.Endpoints;

public static class PlanEndpoint
{
    public static void MapPlanEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/plans").WithTags("Plans").WithGroupName("v1").RequireAuthorization();

        groupV1.MapGet("/", async (IPlanAppService appService, INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetAllAsync(ct);
            return notify.CustomResponse(response, 200);
        })
        //.CustomAuthorize("Admin,Manager,Operator", "Plan", "GetAllPlans")
        .AllowAnonymous()
        .WithName("GetAllPlans")
        .WithSummary("Swagger.Endpoint.Plan.GetAllPlans.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/paged", async ([AsParameters] PagedFilterRequest request, IPlanAppService appService, INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetPagedAsync(request, ct);
            return notify.CustomResponse(response, 200);
        })
        //.CustomAuthorize("Admin,Manager,Operator", "Plan", "GetPagedPlans")
        .AllowAnonymous()
        .WithName("GetPagedPlans")
        .WithSummary("Swagger.Endpoint.Plan.GetPagedPlans.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{id}", async (int id, IPlanAppService appService, INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByIdAsync(id, ct);
            return notify.CustomResponse(response, 200);
        })
        //.CustomAuthorize("Admin,Manager,Operator", "Plan", "GetPlanById")
        .AllowAnonymous()
        .WithName("GetPlanById")
        .WithSummary("Swagger.Endpoint.Plan.GetPlanById.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/", async ([FromBody] CreatePlanRequest request, IPlanAppService appService, INotify notify, CancellationToken ct) =>
        {
            var created = await appService.CreateAsync(request, ct);
            return notify.CustomResponse(201);
        })
        //.CustomAuthorize("Admin,Manager", "Plan", "CreatePlan")
        .AllowAnonymous()
        .WithName("CreatePlan")
        .WithSummary("Swagger.Endpoint.Plan.CreatePlan.Summary")
        .Produces(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<CreatePlanRequest>();

        groupV1.MapPut("/{id}", async (int id, [FromBody] UpdatePlanRequest request, IPlanAppService appService, INotify notify, CancellationToken ct) =>
        {
            var updated = await appService.UpdateAsync(id, request, ct);
            return notify.CustomResponse(updated, 200);
        })
        //.CustomAuthorize("Admin,Manager", "Plan", "UpdatePlan")
        .AllowAnonymous()
        .WithName("UpdatePlan")
        .WithSummary("Swagger.Endpoint.Plan.UpdatePlan.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<UpdatePlanRequest>();

        groupV1.MapPatch("/{id}/activate", async (int id, IPlanAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.ActivateAsync(id, ct);
            return notify.CustomResponse();
        })
        //.CustomAuthorize("Admin,Manager", "Plan", "ActivatePlan")
        .AllowAnonymous()
        .WithName("ActivatePlan")
        .WithSummary("Swagger.Endpoint.Plan.ActivatePlan.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/deactivate", async (int id, IPlanAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeactivateAsync(id, ct);
            return notify.CustomResponse();
        })
        //.CustomAuthorize("Admin,Manager", "Plan", "DeactivatePlan")
        .AllowAnonymous()
        .WithName("DeactivatePlan")
        .WithSummary("Swagger.Endpoint.Plan.DeactivatePlan.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapDelete("/{id}", async (int id, IPlanAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeleteAsync(id, ct);
            return notify.CustomResponse();
        })
        //.CustomAuthorize("Admin,Manager", "Plan", "DeletePlan")
        .AllowAnonymous()
        .WithName("DeletePlan")
        .WithSummary("Swagger.Endpoint.Plan.DeletePlan.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
