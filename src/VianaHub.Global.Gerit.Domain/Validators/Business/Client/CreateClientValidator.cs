using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.Client;

/// <summary>
/// Validador para criação de Client
/// </summary>
public class CreateClientValidator : AbstractValidator<ClientEntity>
{
    public CreateClientValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Client.TenantIdRequired"));

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Client.CreatedByRequired"));
    }
}
