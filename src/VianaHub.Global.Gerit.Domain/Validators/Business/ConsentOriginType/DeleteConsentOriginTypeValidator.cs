using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ConsentOriginType;

public class DeleteConsentOriginTypeValidator : AbstractValidator<ConsentOriginTypeEntity>
{
    public DeleteConsentOriginTypeValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage(localization.GetMessage("Domain.ConsentOriginType.IdRequired"));
        RuleFor(x => x.IsDeleted).Equal(false).WithMessage(localization.GetMessage("Domain.ConsentOriginType.AlreadyDeleted"));
    }
}
