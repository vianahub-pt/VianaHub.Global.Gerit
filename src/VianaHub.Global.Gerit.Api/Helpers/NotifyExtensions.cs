using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Domain.Interfaces;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Helpers;

/// <summary>
/// Classe responsável por mapear os endpoints de NotifyExtensions.
/// </summary>
public static class NotifyExtensions
{
    /// <summary>
    /// Retorna uma resposta customizada para o padrão de notificação.
    /// </summary>
    public static IResult CustomResponse<T>(this INotify notify, T data, int statusCode = 200)
    {
        if (notify.HasNotify())
        {
            statusCode = (int)notify.GetStatusCode();
            var errorMessages = notify.GetErrorMessage();

            var errorResponse = new ErrorResponse(GetErrorTitle(statusCode));

            foreach (var message in errorMessages)
            {
                if (message.Contains(":"))
                {
                    var parts = message.Split(':', 2);
                    var field = parts[0].Trim();
                    var errorMsg = parts[1].Trim();
                    errorResponse.AddError(field, errorMsg);
                }
                else
                {
                    errorResponse.AddError(GetLocalizedErrorLabel(), message);
                }
            }
            return Results.Json(errorResponse, statusCode: statusCode);
        }

        if (statusCode == 204)
        {
            return Results.NoContent();
        }

        return Results.Json(data, statusCode: statusCode);
    }

    public static IResult CustomResponse(this INotify notify, int statusCode = 200)
    {
        // Se houver notificações de erro
        if (notify.HasNotify())
        {
            statusCode = (int)notify.GetStatusCode();
            var errorMessages = notify.GetErrorMessage();

            var errorResponse = new ErrorResponse(GetErrorTitle(statusCode));

            foreach (var message in errorMessages)
            {
                if (message.Contains(":"))
                {
                    var parts = message.Split(':', 2);
                    errorResponse.AddError(parts[0].Trim(), parts[1].Trim());
                }
                else
                {
                    errorResponse.AddError(GetLocalizedErrorLabel(), message);
                }
            }

            return Results.Json(errorResponse, statusCode: statusCode);
        }

        // Sem erros → devolve o status code solicitado
        if (statusCode == 204)
        {
            return Results.NoContent();
        }

        return Results.StatusCode(statusCode);
    }

    public static IResult CustomResponse(this INotify notify)
    {
        int statusCode = 200;

        if (notify.HasNotify())
        {
            statusCode = (int)notify.GetStatusCode();
            var errorMessages = notify.GetErrorMessage();
            var errorResponse = new ErrorResponse(GetErrorTitle(statusCode));
            foreach (var message in errorMessages)
            {
                if (message.Contains(":"))
                {
                    var parts = message.Split(':', 2);
                    var field = parts[0].Trim();
                    var errorMsg = parts[1].Trim();
                    errorResponse.AddError(field, errorMsg);
                }
                else
                {
                    errorResponse.AddError(GetLocalizedErrorLabel(), message);
                }
            }
            return Results.Json(errorResponse, statusCode: statusCode);
        }

        return Results.Ok();
    }

    /// <summary>
    /// Adiciona um erro de campo à notificação.
    /// </summary>
    public static void AddFieldError(this INotify notify, string field, string message, int statusCode = 400)
    {
        notify.Add($"{field}: {message}", statusCode);
    }

    private static string GetErrorTitle(int statusCode)
    {
        var localization = GetLocalizationService();

        if (localization == null)
        {
            // Fallback para inglês se não houver localização disponível
            return statusCode switch
            {
                400 => "Validation Error",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Resource Not Found",
                409 => "Conflict",
                410 => "Resource Gone",
                422 => "Unprocessable Entity",
                429 => "Too Many Requests",
                500 => "Internal Server Error",
                503 => "Service Unavailable",
                _ => "Error"
            };
        }

        return statusCode switch
        {
            400 => localization.GetMessage("Api.Error.ValidationError"),
            401 => localization.GetMessage("Api.Error.Unauthorized"),
            403 => localization.GetMessage("Api.Error.Forbidden"),
            404 => localization.GetMessage("Api.Error.NotFound"),
            409 => localization.GetMessage("Api.Error.Conflict"),
            410 => localization.GetMessage("Api.Error.Gone"),
            422 => localization.GetMessage("Api.Error.UnprocessableEntity"),
            429 => localization.GetMessage("Api.Error.TooManyRequests"),
            500 => localization.GetMessage("Api.Error.InternalServerError"),
            503 => localization.GetMessage("Api.Error.ServiceUnavailable"),
            _ => localization.GetMessage("Api.Error.Generic")
        };
    }

    private static string GetLocalizedErrorLabel()
    {
        var localization = GetLocalizationService();
        return localization?.GetMessage("Common.Error") ?? "Error";
    }

    private static ILocalizationService GetLocalizationService()
    {
        // Tenta obter o serviço do contexto HTTP atual via IHttpContextAccessor
        var httpContextAccessor = ServiceProviderHolder.ServiceProvider?.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
        return httpContextAccessor?.HttpContext?.RequestServices.GetService(typeof(ILocalizationService)) as ILocalizationService;
    }
}
