using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.StatusType;

public class DeactivateStatusTypeValidator : AbstractValidator<StatusTypeEntity>
{
    private readonly ILocalizationService _localization;

    public DeactivateStatusTypeValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.StatusType.IdRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(_localization.GetMessage("Domain.StatusType.CannotDeactivateDeleted"));
    }
}
