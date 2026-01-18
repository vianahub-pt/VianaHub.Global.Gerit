using Microsoft.AspNetCore.Mvc;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Subscription;
using VianaHub.Global.Gerit.Application.Dtos.Response.Subscription;
using VianaHub.Global.Gerit.Application.Interfaces;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Endpoints;

public static class SubscriptionEndpoints
{
    public static void MapSubscriptionEndpoints(this IEndpointRouteBuilder app)
    {
        var groupV1 = app.MapGroup("/v1/subscriptions").WithTags("Subscriptions").WithGroupName("v1").RequireAuthorization();

        groupV1.MapGet("/", async (ISubscriptionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var subscriptions = await appService.GetAllAsync(ct);
            return notify.CustomResponse(subscriptions, 200);
        })
        .AllowAnonymous()
        .WithName("GetAllSubscriptions")
        .WithSummary("Swagger.Endpoint.Subscription.GetAllSubscriptions.Summary")
        .Produces<IEnumerable<SubscriptionResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/paged", async ([AsParameters] PagedFilterRequest request, ISubscriptionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var subscriptions = await appService.GetPagedAsync(request, ct);
            return notify.CustomResponse(subscriptions, 200);
        })
        .AllowAnonymous()
        .WithName("GetPagedSubscriptions")
        .WithSummary("Swagger.Endpoint.Subscription.GetPagedSubscriptions.Summary")
        .Produces<ListPageResponse<SubscriptionResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/{id}", async (int id, ISubscriptionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var subscription = await appService.GetByIdAsync(id, ct);
            return notify.CustomResponse(subscription, 200);
        })
        .AllowAnonymous()
        .WithName("GetSubscriptionById")
        .WithSummary("Swagger.Endpoint.Subscription.GetSubscriptionById.Summary")
        .Produces<SubscriptionResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/tenant/{tenantId}", async (int tenantId, ISubscriptionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var subscription = await appService.GetByTenantIdAsync(tenantId, ct);
            return notify.CustomResponse(subscription, 200);
        })
        .AllowAnonymous()
        .WithName("GetSubscriptionByTenantId")
        .WithSummary("Swagger.Endpoint.Subscription.GetSubscriptionByTenantId.Summary")
        .Produces<SubscriptionResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/active", async (ISubscriptionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var subscriptions = await appService.GetActiveSubscriptionsAsync(ct);
            return notify.CustomResponse(subscriptions, 200);
        })
        .AllowAnonymous()
        .WithName("GetActiveSubscriptions")
        .WithSummary("Swagger.Endpoint.Subscription.GetActiveSubscriptions.Summary")
        .Produces<IEnumerable<SubscriptionResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapGet("/expiring/{days}", async (int days, ISubscriptionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var subscriptions = await appService.GetExpiringSubscriptionsAsync(days, ct);
            return notify.CustomResponse(subscriptions, 200);
        })
        .AllowAnonymous()
        .WithName("GetExpiringSubscriptions")
        .WithSummary("Swagger.Endpoint.Subscription.GetExpiringSubscriptions.Summary")
        .Produces<IEnumerable<SubscriptionResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPost("/", async ([FromBody] CreateSubscriptionRequest request, ISubscriptionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.CreateAsync(request, ct);
            return notify.CustomResponse(201);
        })
        .AllowAnonymous()
        .WithName("CreateSubscription")
        .WithSummary("Swagger.Endpoint.Subscription.CreateSubscription.Summary")
        .Produces(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPut("/{id}", async (int id, [FromBody] UpdateSubscriptionRequest request, ISubscriptionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.UpdateAsync(id, request, ct);
            return notify.CustomResponse();
        })
        .AllowAnonymous()
        .WithName("UpdateSubscription")
        .WithSummary("Swagger.Endpoint.Subscription.UpdateSubscription.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/activate", async (int id, ISubscriptionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.ActivateAsync(id, ct);
            return notify.CustomResponse();
        })
        .AllowAnonymous()
        .WithName("ActivateSubscription")
        .WithSummary("Swagger.Endpoint.Subscription.ActivateSubscription.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/deactivate", async (int id, ISubscriptionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeactivateAsync(id, ct);
            return notify.CustomResponse();
        })
        .AllowAnonymous()
        .WithName("DeactivateSubscription")
        .WithSummary("Swagger.Endpoint.Subscription.DeactivateSubscription.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/cancel", async (int id, [FromBody] CancelSubscriptionRequest request, ISubscriptionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.CancelAsync(id, request, ct);
            return notify.CustomResponse();
        })
        .AllowAnonymous()
        .WithName("CancelSubscription")
        .WithSummary("Swagger.Endpoint.Subscription.CancelSubscription.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapPatch("/{id}/renew", async (int id, [FromBody] RenewSubscriptionRequest request, ISubscriptionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.RenewAsync(id, request, ct);
            return notify.CustomResponse();
        })
        .AllowAnonymous()
        .WithName("RenewSubscription")
        .WithSummary("Swagger.Endpoint.Subscription.RenewSubscription.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        groupV1.MapDelete("/{id}", async (int id, ISubscriptionAppService appService, INotify notify, CancellationToken ct) =>
        {
            var ok = await appService.DeleteAsync(id, ct);
            return notify.CustomResponse();
        })
        .AllowAnonymous()
        .WithName("DeleteSubscription")
        .WithSummary("Swagger.Endpoint.Subscription.DeleteSubscription.Summary")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
    }
}
