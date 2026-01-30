using Microsoft.AspNetCore.Mvc;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Job;
using VianaHub.Global.Gerit.Application.Interfaces.Job;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Endpoints.Job;

public static class JobEndpoint
{
    public static void MapJobEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/admin/job-definition").WithTags("JobDefinitions").WithGroupName("v1").RequireAuthorization();

        groupV1.MapGet("/{id}", async (int id, IJobAppService appService, INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetByIdAsync(id, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin", "JobDefinitions", "GetBy")
        .AllowAnonymous()
        .WithName("GetJobById")
        .WithSummary("Swagger.Endpoint.Job.GetById.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/paged", async ([AsParameters] JobPagedFilter request, IJobAppService appService, INotify notify, CancellationToken ct) =>
        {
            var response = await appService.GetPagedAsync(request, ct);
            return notify.CustomResponse(response, 200);
        })
        .CustomAuthorize("Admin", "JobDefinitions", "GetPaged")
        .AllowAnonymous()
        .WithName("GetPagedJobs")
        .WithSummary("Swagger.Endpoint.Job.GetPaged.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/", async ([FromBody] CreateJobRequest request, IJobAppService appService, INotify notify, CancellationToken ct) =>
        {
            var created = await appService.CreateAsync(request, ct);
            return notify.CustomResponse(created ? 201 : 400);
        })
        .CustomAuthorize("Admin", "JobDefinitions", "Create")
        .AllowAnonymous()
        .WithName("CreateJob")
        .WithSummary("Swagger.Endpoint.Job.Create.Summary")
        .Produces(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<CreateJobRequest>();

        groupV1.MapPut("/{id}", async (int id, [FromBody] UpdateJobRequest request, IJobAppService appService, INotify notify, CancellationToken ct) =>
        {
            var updated = await appService.UpdateAsync(id, request, ct);
            return notify.CustomResponse(updated, 200);
        })
        .CustomAuthorize("Admin", "JobDefinitions", "Update")
        .AllowAnonymous()
        .WithName("UpdateJob")
        .WithSummary("Swagger.Endpoint.Job.Update.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<UpdateJobRequest>();

        groupV1.MapPatch("/{id}/activate", async (int id, IJobAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.ActivateAsync(id, ct);
            return notify.CustomResponse(ok ? 200 : 400);
        })
        .CustomAuthorize("Admin", "JobDefinitions", "Activate")
        .AllowAnonymous()
        .WithName("ActivateJob")
        .WithSummary("Swagger.Endpoint.Job.Activate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/deactivate", async (int id, IJobAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeactivateAsync(id, ct);
            return notify.CustomResponse(ok ? 200 : 400);
        })
        .CustomAuthorize("Admin", "JobDefinitions", "Deactivate")
        .AllowAnonymous()
        .WithName("DeactivateJob")
        .WithSummary("Swagger.Endpoint.Job.Deactivate.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/{id}/execute", async (int id, IJobAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.ExecuteAsync(id, ct);
            return notify.CustomResponse(ok ? 200 : 400);
        })
        .CustomAuthorize("Admin", "JobDefinitions", "Execute")
        .AllowAnonymous()
        .WithName("ExecuteJob")
        .WithSummary("Swagger.Endpoint.Job.Execute.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapDelete("/{id}", async (int id, IJobAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeleteAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin", "JobDefinitions", "Delete")
        .AllowAnonymous()
        .WithName("DeleteJob")
        .WithSummary("Swagger.Endpoint.Job.Delete.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
