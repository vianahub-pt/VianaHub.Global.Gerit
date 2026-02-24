using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientContact;

/// <summary>
/// Validador para desativação de ClientContact
/// </summary>
public class DeactivateClientContactValidator : AbstractValidator<ClientContactEntity>
{
    public DeactivateClientContactValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.ClientContact.IdRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.ClientContact.CannotDeactivateDeleted"));
    }
}
