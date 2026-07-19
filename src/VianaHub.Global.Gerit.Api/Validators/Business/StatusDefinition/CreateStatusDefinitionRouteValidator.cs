using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.StatusDefinition;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.StatusDefinition;

/// <summary>
/// Validador de rota para criação de StatusDefinition.
/// </summary>
public class CreateStatusDefinitionRouteValidator : AbstractValidator<CreateStatusDefinitionRequest>
{
    public CreateStatusDefinitionRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.StatusDomainId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Api.Validator.StatusDefinition.Create.StatusDomainId"));

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.StatusDefinition.Create.Code"))
            .MaximumLength(50)
            .WithMessage(localization.GetMessage("Api.Validator.StatusDefinition.Create.Code.MaximumLength", 50));

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0)
            .WithMessage(localization.GetMessage("Api.Validator.StatusDefinition.Create.DisplayOrder"));
    }
}
