using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.Client;

/// <summary>
/// Validador para exclus�o de Client
/// </summary>
public class DeleteClientValidator : AbstractValidator<ClientEntity>
{
    public DeleteClientValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Client.IdRequired"));
    }
}
