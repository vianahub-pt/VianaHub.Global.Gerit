using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.Function;

public class CreateFunctionValidator : AbstractValidator<FunctionEntity>
{
    public CreateFunctionValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Function.TenantIdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Function.NameRequired"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Domain.Function.NameMaxLength", 150));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Function.DescriptionRequired"))
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Domain.Function.DescriptionMaxLength", 255));
    }
}
