using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.Client;

/// <summary>
/// Validador para atualização de Client
/// </summary>
public class UpdateClientValidator : AbstractValidator<ClientEntity>
{
    public UpdateClientValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Client.IdRequired"));

        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Client.TenantIdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Client.NameRequired"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Domain.Client.NameMaxLength", 150));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Client.EmailRequired"))
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Domain.Client.EmailMaxLength", 255))
            .EmailAddress()
            .WithMessage(localization.GetMessage("Domain.Client.EmailInvalid"));

        RuleFor(x => x.Phone)
            .MaximumLength(30)
            .WithMessage(localization.GetMessage("Domain.Client.PhoneMaxLength", 30));

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Client.ModifiedByRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.Client.CannotUpdateDeleted"));
    }
}
