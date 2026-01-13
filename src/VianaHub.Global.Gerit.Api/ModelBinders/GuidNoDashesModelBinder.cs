using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VianaHub.Global.Gerit.Api.ModelBinders;

/// <summary>
/// Model binder customizado para aceitar Guids com ou sem hífens em parâmetros de rota.
/// Permite que URLs usem formatos como: /api/resource/4c84e95c4e164d6c927291a0f113e92b
/// </summary>
public class GuidNoDashesModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var modelName = bindingContext.ModelName;
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        var value = valueProviderResult.FirstValue;

        if (string.IsNullOrWhiteSpace(value))
        {
            return Task.CompletedTask;
        }

        // Tenta fazer o parse do Guid com ou sem hífens
        if (Guid.TryParse(value, out var guid))
        {
            bindingContext.Result = ModelBindingResult.Success(guid);
        }
        else
        {
            bindingContext.ModelState.TryAddModelError(
                modelName,
                $"The value '{value}' is not a valid Guid.");
        }

        return Task.CompletedTask;
    }
}
