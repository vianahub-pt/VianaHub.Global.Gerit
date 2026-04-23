using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitContact;

/// <summary>
/// Validador para desativação de VisitContact
/// </summary>
public class DeactivateVisitContactValidator : AbstractValidator<VisitContactEntity>
{
    public DeactivateVisitContactValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.VisitContact.IdRequired"));

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.VisitContact.ModifiedByRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.VisitContact.IsDeleted"));
    }
}
