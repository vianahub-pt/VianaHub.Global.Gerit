using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.StatusDefinition;

public class UpdateStatusDefinitionValidator : AbstractValidator<StatusDefinitionEntity>
{
    public UpdateStatusDefinitionValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.StatusDefinition.IdRequired"));

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.StatusDefinition.CodeRequired"))
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.StatusDefinition.CodeMaxLength", 100));

        RuleFor(x => x.ModifiedBy)
            .NotNull()
            .WithMessage(localization.GetMessage("Domain.StatusDefinition.ModifiedByRequired"))
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.StatusDefinition.ModifiedByRequired"));
    }
}
