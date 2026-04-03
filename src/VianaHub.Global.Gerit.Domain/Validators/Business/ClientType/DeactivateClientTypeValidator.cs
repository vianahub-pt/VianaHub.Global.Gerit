using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientType;

public class DeactivateClientTypeValidator : AbstractValidator<ClientTypeEntity>
{
    public DeactivateClientTypeValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.ClientType.IdRequired"));

        RuleFor(x => x.IsActive)
            .Equal(true)
            .WithMessage(localization.GetMessage("Domain.ClientType.AlreadyInactive"));
    }
}
