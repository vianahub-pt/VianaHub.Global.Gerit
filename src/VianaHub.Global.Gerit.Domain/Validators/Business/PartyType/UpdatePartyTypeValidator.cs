using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.PartyType;

public class UpdatePartyTypeValidator : AbstractValidator<PartyTypeEntity>
{
    public UpdatePartyTypeValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .NotEqual((byte)0)
            .WithMessage(localization.GetMessage("Domain.PartyType.IdRequired"));

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.PartyType.CodeRequired"))
            .MaximumLength(50)
            .WithMessage(localization.GetMessage("Domain.PartyType.CodeMaxLength", 50));

        RuleFor(x => x.ModifiedBy)
            .NotNull()
            .WithMessage(localization.GetMessage("Domain.PartyType.ModifiedByRequired"))
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.PartyType.ModifiedByRequired"));
    }
}
