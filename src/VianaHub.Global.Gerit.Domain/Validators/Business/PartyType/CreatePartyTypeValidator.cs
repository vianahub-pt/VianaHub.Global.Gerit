using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.PartyType;

public class CreatePartyTypeValidator : AbstractValidator<PartyTypeEntity>
{
    public CreatePartyTypeValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.PartyType.CodeRequired"))
            .MaximumLength(50)
            .WithMessage(localization.GetMessage("Domain.PartyType.CodeMaxLength", 50));

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.PartyType.CreatedByRequired"));
    }
}
