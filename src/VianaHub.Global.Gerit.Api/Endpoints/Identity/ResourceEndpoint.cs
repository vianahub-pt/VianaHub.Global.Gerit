using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Identity.Resource;
using VianaHub.Global.Gerit.Application.Interfaces.Identity;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Endpoints.Identity;

public static class ResourceEndpoint
{
    public static void MapResourceEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/resources").WithTags("Resources").WithGroupName("v1").RequireAuthorization();

        groupV1.MapGet("/", async (IResourceAppService appService, INotify notify, CancellationToken ct) =>
        {
            var resources = await appService.GetAllAsync(ct);
            return notify.CustomResponse(resources, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "Resources", "GetAll")
        .WithName("GetAllResources")
        .WithSummary("Swagger.Endpoint.Resource.GetAllResources.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{id}", async (int id, IResourceAppService appService, INotify notify, CancellationToken ct) =>
        {
            var resource = await appService.GetByIdAsync(id, ct);
            return notify.CustomResponse(resource, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "Resources", "GetBy")
        .WithName("GetResourceById")
        .WithSummary("Swagger.Endpoint.Resource.GetResourceById.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/paged", async ([AsParameters] PagedFilterRequest request, IResourceAppService appService, INotify notify, CancellationToken ct) =>
        {
            var resources = await appService.GetPagedAsync(request, ct);
            return notify.CustomResponse(resources, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "Resources", "GetPaged")
        .WithName("GetPagedResources")
        .WithSummary("Swagger.Endpoint.Resource.GetPagedResources.Summary")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/", async ([FromBody] CreateResourceRequest request, IResourceAppService appService, INotify notify, CancellationToken ct) =>
        {
            var created = await appService.CreateAsync(request, ct);
            return notify.CustomResponse(201);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "Resources", "Create")
        .WithName("CreateResource")
        .WithSummary("Swagger.Endpoint.Resource.CreateResource.Summary")
        .Produces(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<CreateResourceRequest>();

        groupV1.MapPut("/{id}", async (int id, [FromBody] UpdateResourceRequest request, IResourceAppService appService, INotify notify, CancellationToken ct) =>
        {
            var updated = await appService.UpdateAsync(id, request, ct);
            return notify.CustomResponse(updated, 200);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "Resources", "Update")
        .WithName("UpdateResource")
        .WithSummary("Swagger.Endpoint.Resource.UpdateResource.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<UpdateResourceRequest>();

        groupV1.MapPatch("/{id}/activate", async (int id, IResourceAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.ActivateAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "Resources", "Activate")
        .WithName("ActivateResource")
        .WithSummary("Swagger.Endpoint.Resource.ActivateResource.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/deactivate", async (int id, IResourceAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeactivateAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "Resource", "Deactivate")
        .WithName("DeactivateResource")
        .WithSummary("Swagger.Endpoint.Resource.DeactivateResource.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapDelete("/{id}", async (int id, IResourceAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeleteAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "Resources", "Delete")
        .WithName("DeleteResource")
        .WithSummary("Swagger.Endpoint.Resource.DeleteResource.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        // Upload massivo de resources via CSV
        groupV1.MapPost("/bulk-upload", async (HttpRequest request, IResourceAppService appService, INotify notify, CancellationToken ct) =>
        {
            if (!request.HasFormContentType || request.Form.Files.Count == 0)
            {
                notify.Add("Nenhum arquivo foi enviado", 400);
                return notify.CustomResponse();
            }

            var file = request.Form.Files[0];
            var success = await appService.BulkUploadAsync(file, ct);
            return notify.CustomResponse(success);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "Resource", "BulkUpload")
        .WithName("BulkUploadResources")
        .WithSummary("Swagger.Endpoint.Resource.BulkUploadResources.Summary")
        .DisableAntiforgery()
        .Accepts<IFormFile>("multipart/form-data")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
