using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.StatusDomain;

public class UpdateStatusDomainValidator : AbstractValidator<StatusDomainEntity>
{
    public UpdateStatusDomainValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.StatusDomain.IdRequired"));

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.StatusDomain.CodeRequired"))
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.StatusDomain.CodeMaxLength", 100));

        RuleFor(x => x.ModifiedBy)
            .NotNull()
            .WithMessage(localization.GetMessage("Domain.StatusDomain.ModifiedByRequired"))
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.StatusDomain.ModifiedByRequired"));
    }
}
