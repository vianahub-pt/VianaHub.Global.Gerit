using FluentValidation;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Identity.Role;
using VianaHub.Global.Gerit.Application.Dtos.Response.Identity.Role;
using VianaHub.Global.Gerit.Application.Interfaces.Identity;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Endpoints.Identity;

public static class RoleEndpoint
{
    public static void MapRoleEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/roles").WithTags("Roles").WithGroupName("v1").RequireAuthorization();

        groupV1.MapGet("/", async (IRoleAppService appService, INotify notify, CancellationToken ct) =>
        {
            var result = await appService.GetAllAsync(ct);
            return notify.CustomResponse(result);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "Roles", "GetAll")
        .WithName("GetAllRoles")
        .WithSummary("Swagger.Endpoint.Role.GetAllRoles.Summary")
        .Produces<IEnumerable<RoleResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status401Unauthorized)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{id}", async (int id, IRoleAppService appService, INotify notify, CancellationToken ct) =>
        {
            var result = await appService.GetByIdAsync(id, ct);
            return notify.CustomResponse(result);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "Roles", "GetBy")
        .WithName("GetRoleById")
        .WithSummary("Swagger.Endpoint.Role.GetRoleById.Summary")
        .Produces<RoleResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/paged", async ([AsParameters] PagedFilterRequest request, IRoleAppService appService, INotify notify, CancellationToken ct) =>
        {
            var result = await appService.GetPagedAsync(request, ct);
            return notify.CustomResponse(result);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "Roles", "GetPaged")
        .WithName("GetPagedRoles")
        .WithSummary("Swagger.Endpoint.Role.GetPagedRoles.Summary")
        .Produces<ListPageResponse<RoleResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status401Unauthorized)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);


        groupV1.MapPost("/", async (CreateRoleRequest request, IRoleAppService appService, INotify notify, CancellationToken ct) =>
        {
            var created = await appService.CreateAsync(request, ct);
            return notify.CustomResponse(201);
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "Roles", "Create")
        .WithName("CreateRole")
        .WithSummary("Swagger.Endpoint.Role.CreateRole.Summary")
        .Produces(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<CreateRoleRequest>();

        groupV1.MapPut("/{id}", async (int id, UpdateRoleRequest request, IRoleAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.UpdateAsync(id, request, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "Roles", "Update")
        .WithName("UpdateRole")
        .WithSummary("Swagger.Endpoint.Role.UpdateRole.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithValidation<UpdateRoleRequest>();

        groupV1.MapPatch("/{id}/activate", async (int id, IRoleAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.ActivateAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "Roles", "Activate")
        .WithName("ActivateRole")
        .WithSummary("Swagger.Endpoint.Role.ActivateRole.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/deactivate", async (int id, IRoleAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeactivateAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "Roles", "Deactivate")
        .WithName("DeactivateRole")
        .WithSummary("Swagger.Endpoint.Role.DeactivateRole.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapDelete("/{id}", async (int id, IRoleAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeleteAsync(id, ct);
            return notify.CustomResponse();
        })
        .CustomAuthorize("Admin,BackOffice,Manager", "Roles", "Delete")
        .WithName("DeleteRole")
        .WithSummary("Swagger.Endpoint.Role.DeleteRole.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        // Upload massivo de roles via CSV
        groupV1.MapPost("/bulk-upload", async (HttpRequest request, IRoleAppService appService, INotify notify, CancellationToken ct) =>
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
        .CustomAuthorize("Admin,BackOffice,Manager", "Roles", "BulkUpload")
        .WithName("BulkUploadRoles")
        .WithSummary("Swagger.Endpoint.Role.BulkUploadRoles.Summary")
        .DisableAntiforgery()
        .Accepts<IFormFile>("multipart/form-data")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
