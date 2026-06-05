using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Client;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.Client;

/// <summary>
/// Validador para CreateClientRequest
/// </summary>
public class CreateClientRouteValidator : AbstractValidator<CreateClientRequest>
{
    public CreateClientRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.ClientType)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.ClientType"));

        RuleFor(x => x.OriginType)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.OriginType"));

        RuleFor(x => x.UrlImage)
            .MaximumLength(500)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.UrlImage.MaximumLength", 500))
            .When(x => !string.IsNullOrWhiteSpace(x.UrlImage));

        RuleFor(x => x.Note)
            .MaximumLength(1000)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Note.MaximumLength", 1000))
            .When(x => !string.IsNullOrWhiteSpace(x.Note));

        RuleFor(x => x.Individual!.FirstName)
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Individual.FirstName.MaximumLength", 100))
            .When(x => x.Individual != null && !string.IsNullOrWhiteSpace(x.Individual.FirstName));

        RuleFor(x => x.Individual!.LastName)
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Individual.LastName.MaximumLength", 100))
            .When(x => x.Individual != null && !string.IsNullOrWhiteSpace(x.Individual.LastName));

        RuleFor(x => x.Individual!.PhoneNumber)
            .MaximumLength(50)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Individual.PhoneNumber.MaximumLength", 50))
            .When(x => x.Individual != null && !string.IsNullOrWhiteSpace(x.Individual.PhoneNumber));

        RuleFor(x => x.Individual!.CellPhoneNumber)
            .MaximumLength(50)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Individual.CellPhoneNumber.MaximumLength", 50))
            .When(x => x.Individual != null && !string.IsNullOrWhiteSpace(x.Individual.CellPhoneNumber));

        RuleFor(x => x.Individual!.Email)
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Individual.Email.MaximumLength", 100))
            .When(x => x.Individual != null && !string.IsNullOrWhiteSpace(x.Individual.Email));

        RuleFor(x => x.Individual!.Gender)
            .MaximumLength(20)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Individual.Gender.MaximumLength", 20))
            .When(x => x.Individual != null && !string.IsNullOrWhiteSpace(x.Individual.Gender));

        RuleFor(x => x.Individual!.DocumentType)
            .MaximumLength(50)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Individual.DocumentType.MaximumLength", 50))
            .When(x => x.Individual != null && !string.IsNullOrWhiteSpace(x.Individual.DocumentType));

        RuleFor(x => x.Individual!.DocumentNumber)
            .MaximumLength(50)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Individual.DocumentNumber.MaximumLength", 50))
            .When(x => x.Individual != null && !string.IsNullOrWhiteSpace(x.Individual.DocumentNumber));

        RuleFor(x => x.Individual!.Nationality)
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Individual.Nationality.MaximumLength", 100))
            .When(x => x.Individual != null && !string.IsNullOrWhiteSpace(x.Individual.Nationality));

        RuleFor(x => x.Company!.LegalName)
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Company.LegalName.MaximumLength", 200))
            .When(x => x.Company != null && !string.IsNullOrWhiteSpace(x.Company.LegalName));

        RuleFor(x => x.Company!.TradeName)
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Company.TradeName.MaximumLength", 200))
            .When(x => x.Company != null && !string.IsNullOrWhiteSpace(x.Company.TradeName));

        RuleFor(x => x.Company!.Site)
            .MaximumLength(500)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Company.Site.MaximumLength", 500))
            .When(x => x.Company != null && !string.IsNullOrWhiteSpace(x.Company.Site));

        RuleFor(x => x.Company!.CompanyRegistration)
            .MaximumLength(50)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Company.CompanyRegistration.MaximumLength", 50))
            .When(x => x.Company != null && !string.IsNullOrWhiteSpace(x.Company.CompanyRegistration));

        RuleFor(x => x.Company!.CAE)
            .MaximumLength(10)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Company.CAE.MaximumLength", 10))
            .When(x => x.Company != null && !string.IsNullOrWhiteSpace(x.Company.CAE));

        RuleFor(x => x.Company!.LegalRepresentative)
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Company.LegalRepresentative.MaximumLength", 150))
            .When(x => x.Company != null && !string.IsNullOrWhiteSpace(x.Company.LegalRepresentative));
    }
}
