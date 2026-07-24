using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientContact;

/// <summary>
/// Validador para desativa��o de ClientContact
/// </summary>
public class DeactivateClientContactPersonValidator : AbstractValidator<ClientContactPersonsEntity>
{
    public DeactivateClientContactPersonValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.ClientContactPerson.IdRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.ClientContactPerson.CannotDeactivateDeleted"));
    }
}
