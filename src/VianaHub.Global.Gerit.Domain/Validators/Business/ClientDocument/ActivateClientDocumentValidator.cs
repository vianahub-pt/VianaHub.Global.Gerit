using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientDocument;

/// <summary>
/// Validador para ativação de ClientDocument
/// </summary>
public class ActivateClientDocumentValidator : AbstractValidator<ClientDocumentsEntity>
{
    public ActivateClientDocumentValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.ClientDocument.IdRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.ClientDocument.CannotActivateDeleted"));
    }
}
