using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientAddress;

/// <summary>
/// Validador para exclusão de ClientAddress
/// </summary>
public class DeleteClientAddressValidator : AbstractValidator<ClientAddressEntity>
{
    public DeleteClientAddressValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.ClientAddress.IdRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.ClientAddress.AlreadyDeleted"));
    }
}
