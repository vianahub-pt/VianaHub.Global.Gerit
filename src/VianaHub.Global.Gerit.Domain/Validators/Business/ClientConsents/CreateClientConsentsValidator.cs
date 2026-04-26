using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientConsents;

public class CreateClientConsentsValidator : AbstractValidator<ClientConsentsEntity>
{
    public CreateClientConsentsValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.ClientConsents.TenantIdRequired"));

        RuleFor(x => x.ClientId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.ClientConsents.ClientIdRequired"));

        RuleFor(x => x.ConsentTypeId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.ClientConsents.ConsentTypeIdRequired"));

        RuleFor(x => x.GrantedDate)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.ClientConsents.GrantedDateRequired"));

        RuleFor(x => x.Origin)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.ClientConsents.OriginRequired"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Domain.ClientConsents.OriginMaxLength", 200));

        RuleFor(x => x.IpAddress)
            .MaximumLength(50)
            .WithMessage(localization.GetMessage("Domain.ClientConsents.IpAddressMaxLength", 50));

        RuleFor(x => x.UserAgent)
            .MaximumLength(500)
            .WithMessage(localization.GetMessage("Domain.ClientConsents.UserAgentMaxLength", 500));

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.ClientConsents.CreatedByRequired"));
    }
}
