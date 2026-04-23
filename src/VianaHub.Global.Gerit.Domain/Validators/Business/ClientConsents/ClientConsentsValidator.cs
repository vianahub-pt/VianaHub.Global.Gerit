using FluentValidation;
using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientConsents;

public class ClientConsentsValidator : AbstractValidator<ClientConsentsEntity>, IEntityDomainValidator<ClientConsentsEntity>
{
    private readonly ILocalizationService _localization;

    public ClientConsentsValidator(ILocalizationService localization)
    {
        _localization = localization;
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
            .MaximumLength(50)
            .WithMessage(localization.GetMessage("Domain.ClientConsents.OriginMaxLength", 50))
            .Must(origin => new[] { "Web", "Mobile", "Paper", "API" }.Contains(origin))
            .WithMessage(localization.GetMessage("Domain.ClientConsents.OriginInvalid"));

        RuleFor(x => x.IpAddress)
            .MaximumLength(50)
            .WithMessage(localization.GetMessage("Domain.ClientConsents.IpAddressMaxLength", 50));

        RuleFor(x => x.UserAgent)
            .MaximumLength(500)
            .WithMessage(localization.GetMessage("Domain.ClientConsents.UserAgentMaxLength", 500));
    }

    public async Task<ValidationResult> ValidateForCreateAsync(ClientConsentsEntity entity)
        => await new CreateClientConsentsValidator(_localization).ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForUpdateAsync(ClientConsentsEntity entity)
        => await new UpdateClientConsentsValidator(_localization).ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForActivateAsync(ClientConsentsEntity entity)
        => await new ActivateClientConsentsValidator(_localization).ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForDeactivateAsync(ClientConsentsEntity entity)
        => await new DeactivateClientConsentsValidator(_localization).ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForDeleteAsync(ClientConsentsEntity entity)
        => await new DeleteClientConsentsValidator(_localization).ValidateAsync(entity);

    public Task<ValidationResult> ValidateForRevokeAsync(ClientConsentsEntity entity)
        => ValidateForDeleteAsync(entity);
}
