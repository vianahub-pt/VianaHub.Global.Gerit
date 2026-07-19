using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.StatusDomain;

public class CreateStatusDomainValidator : AbstractValidator<StatusDomainEntity>
{
    public CreateStatusDomainValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.StatusDomain.CodeRequired"))
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.StatusDomain.CodeMaxLength", 100));

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.StatusDomain.CreatedByRequired"));
    }
}
