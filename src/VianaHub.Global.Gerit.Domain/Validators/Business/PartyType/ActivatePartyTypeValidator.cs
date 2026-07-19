using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.PartyType;

public class ActivatePartyTypeValidator : AbstractValidator<PartyTypeEntity>
{
    public ActivatePartyTypeValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .NotEqual((byte)0)
            .WithMessage(localization.GetMessage("Domain.PartyType.IdRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.PartyType.CannotActivateDeleted"));
    }
}
