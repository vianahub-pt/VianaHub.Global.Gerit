using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.StatusDefinition;

public class CreateStatusDefinitionValidator : AbstractValidator<StatusDefinitionEntity>
{
    public CreateStatusDefinitionValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.StatusDefinition.TenantIdRequired"));

        RuleFor(x => x.StatusDomainId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.StatusDefinition.StatusDomainIdRequired"));

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.StatusDefinition.CodeRequired"))
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.StatusDefinition.CodeMaxLength", 100));

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.StatusDefinition.CreatedByRequired"));
    }
}
