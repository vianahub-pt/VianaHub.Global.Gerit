using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ConsentOriginType;

public class ActivateConsentOriginTypeValidator : AbstractValidator<ConsentOriginTypeEntity>
{
    public ActivateConsentOriginTypeValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage(localization.GetMessage("Domain.ConsentOriginType.IdRequired"));
        RuleFor(x => x.IsDeleted).Equal(false).WithMessage(localization.GetMessage("Domain.ConsentOriginType.CannotActivateDeleted"));
    }
}
