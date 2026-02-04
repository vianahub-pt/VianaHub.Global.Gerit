using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.Function;

public class UpdateFunctionValidator : AbstractValidator<FunctionEntity>
{
    public UpdateFunctionValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Function.InvalidId"));

        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Function.TenantIdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Function.NameRequired"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Domain.Function.NameMaxLength", 150));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.Function.CannotUpdateDeleted"));
    }
}
