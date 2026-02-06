using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.TeamMemberAddress;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Business.TeamMemberAddress;

public class UpdateTeamMemberAddressRequestValidator : AbstractValidator<UpdateTeamMemberAddressRequest>
{
    private readonly ILocalizationService _localization;

    public UpdateTeamMemberAddressRequestValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Api.Validator.TeamMemberAddress.Update.Street"))
            .MaximumLength(200)
            .WithMessage(_localization.GetMessage("Api.Validator.TeamMemberAddress.Update.Street.MaximumLength", 200));

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Api.Validator.TeamMemberAddress.Update.City"))
            .MaximumLength(100)
            .WithMessage(_localization.GetMessage("Api.Validator.TeamMemberAddress.Update.City.MaximumLength", 100));

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Api.Validator.TeamMemberAddress.Update.PostalCode"))
            .MaximumLength(20)
            .WithMessage(_localization.GetMessage("Api.Validator.TeamMemberAddress.Update.PostalCode.MaximumLength", 20));

        RuleFor(x => x.District)
            .MaximumLength(100)
            .WithMessage(_localization.GetMessage("Api.Validator.TeamMemberAddress.Update.District.MaximumLength", 100))
            .When(x => !string.IsNullOrEmpty(x.District));

        RuleFor(x => x.CountryCode)
            .NotEmpty()
            .WithMessage(_localization.GetMessage("Api.Validator.TeamMemberAddress.Update.CountryCode"))
            .Length(2)
            .WithMessage(_localization.GetMessage("Api.Validator.TeamMemberAddress.Update.CountryCode.Length"));
    }
}
