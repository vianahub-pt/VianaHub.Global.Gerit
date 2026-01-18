using FluentValidation;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Tenant;
using VianaHub.Global.Gerit.Application.Dtos.Response.Tenant;
using VianaHub.Global.Gerit.Application.Interfaces;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Endpoints;

public static class TenantEndpoint
{
    public static void MapTenantEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/tenants").WithTags("Tenants").WithGroupName("v1").RequireAuthorization();

        groupV1.MapGet("/", async (ITenantAppService appService, INotify notify, CancellationToken ct) =>
        {
            var result = await appService.GetAllAsync(ct);
            return notify.CustomResponse(result);
        })
        //.CustomAuthorize("Admin,Manager", "Tenant", "GetAllTenants")
        .AllowAnonymous()
        .WithName("GetAllTenants")
        .WithSummary("Swagger.Endpoint.Tenant.GetAllTenants.Summary")
        .Produces<IEnumerable<TenantResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status401Unauthorized)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/paged", async ([AsParameters] PagedFilterRequest request, ITenantAppService appService, INotify notify, CancellationToken ct) =>
        {
            var result = await appService.GetPagedAsync(request, ct);
            return notify.CustomResponse(result);
        })
        //.CustomAuthorize("Admin,Manager", "Tenant", "GetPagedTenants")
        .AllowAnonymous()
        .WithName("GetPagedTenants")
        .WithSummary("Swagger.Endpoint.Tenant.GetPagedTenants.Summary")
        .Produces<ListPageResponse<TenantResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status401Unauthorized)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{id}", async (int id, ITenantAppService appService, INotify notify, CancellationToken ct) =>
        {
            var result = await appService.GetByIdAsync(id, ct);
            return notify.CustomResponse(result);
        })
        //.CustomAuthorize("Admin,Manager", "Tenant", "GetTenantById")
        .AllowAnonymous()
        .WithName("GetTenantById")
        .WithSummary("Swagger.Endpoint.Tenant.GetTenantById.Summary")
        .Produces<TenantResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/", async (CreateTenantRequest request, ITenantAppService appService, INotify notify, CancellationToken ct) =>
        {
            var created = await appService.CreateAsync(request, ct);
            return notify.CustomResponse(201);
        })
        //.CustomAuthorize("Admin,Manager", "Tenant", "CreateTenant")
        .AllowAnonymous()
        .WithName("CreateTenant")
        .WithSummary("Swagger.Endpoint.Tenant.CreateTenant.Summary")
        .Produces(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<CreateTenantRequest>();

        groupV1.MapPut("/{id}", async (int id, UpdateTenantRequest request, ITenantAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.UpdateAsync(id, request, ct);
            return notify.CustomResponse();
        })
        //.CustomAuthorize("Admin,Manager", "Tenant", "UpdateTenant")
        .AllowAnonymous()
        .WithName("UpdateTenant")
        .WithSummary("Swagger.Endpoint.Tenant.UpdateTenant.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<UpdateTenantRequest>();

        groupV1.MapPatch("/{id}/activate", async (int id, ITenantAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.ActivateAsync(id, ct);
            return notify.CustomResponse();
        })
        //.CustomAuthorize("Admin,Manager", "Tenant", "ActivateTenant")
        .AllowAnonymous()
        .WithName("ActivateTenant")
        .WithSummary("Swagger.Endpoint.Tenant.ActivateTenant.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/deactivate", async (int id, ITenantAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeactivateAsync(id, ct);
            return notify.CustomResponse();
        })
        //.CustomAuthorize("Admin,Manager", "Tenant", "DeactivateTenant")
        .AllowAnonymous()
        .WithName("DeactivateTenant")
        .WithSummary("Swagger.Endpoint.Tenant.DeactivateTenant.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapDelete("/{id}", async (int id, ITenantAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeleteAsync(id, ct);
            return notify.CustomResponse();
        })
        //.CustomAuthorize("Admin,Manager", "Tenant", "DeleteTenant")
        .AllowAnonymous()
        .WithName("DeleteTenant")
        .WithSummary("Swagger.Endpoint.Tenant.DeleteTenant.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        // Upload massivo de tenants via CSV
        groupV1.MapPost("/bulk-upload", async (HttpRequest request, ITenantAppService appService, INotify notify, CancellationToken ct) =>
        {
            if (!request.HasFormContentType || request.Form.Files.Count == 0)
            {
                notify.Add("Nenhum arquivo foi enviado", 400);
                return notify.CustomResponse();
            }

            var file = request.Form.Files[0];
            var success = await appService.BulkUploadAsync(file, ct);
            return notify.CustomResponse(success ? 200 : 400);
        })
        //.CustomAuthorize("Admin,Manager", "Tenant", "BulkUploadTenants")
        .AllowAnonymous()
        .WithName("BulkUploadTenants")
        .WithSummary("Swagger.Endpoint.Tenant.BulkUploadTenants.Summary")
        .DisableAntiforgery()
        .Accepts<IFormFile>("multipart/form-data")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
