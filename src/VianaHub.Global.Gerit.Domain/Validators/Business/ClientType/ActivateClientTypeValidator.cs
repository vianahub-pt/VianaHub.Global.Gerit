using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientType;

public class ActivateClientTypeValidator : AbstractValidator<ClientTypeEntity>
{
    public ActivateClientTypeValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.ClientType.IdRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.ClientType.CannotActivateDeleted"));
    }
}
