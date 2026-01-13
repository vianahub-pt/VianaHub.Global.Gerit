using VianaHub.Global.Gerit.Api.Filters;

namespace VianaHub.Global.Gerit.Api.Helpers;

/// <summary>
/// Métodos de extensão para adicionar validação automática aos endpoints
/// </summary>
public static class EndpointValidationExtensions
{
    /// <summary>
    /// Adiciona validação automática ao endpoint usando FluentValidation
    /// </summary>
    /// <typeparam name="TRequest">Tipo do request a ser validado</typeparam>
    /// <param name="builder">Builder do endpoint</param>
    /// <returns>Builder do endpoint com validação configurada</returns>
    public static RouteHandlerBuilder WithValidation<TRequest>(this RouteHandlerBuilder builder)
        where TRequest : class
    {
        return builder.AddEndpointFilter<ValidationFilter<TRequest>>();
    }
}
