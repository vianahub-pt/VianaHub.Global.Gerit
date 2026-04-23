using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientIndividual;

namespace VianaHub.Global.Gerit.Application.Validators.Business.ClientIndividual;

public class CreateClientIndividualValidator : AbstractValidator<CreateClientIndividualRequest>
{
    public CreateClientIndividualValidator()
    {
        RuleFor(x => x.ClientId)
            .GreaterThan(0)
            .WithMessage("client_individual.client_id.required");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("client_individual.first_name.required")
            .MaximumLength(100)
            .WithMessage("client_individual.first_name.max_length");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("client_individual.last_name.required")
            .MaximumLength(100)
            .WithMessage("client_individual.last_name.max_length");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(50)
            .WithMessage("client_individual.phone_number.max_length")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

        RuleFor(x => x.CellPhoneNumber)
            .MaximumLength(50)
            .WithMessage("client_individual.cell_phone_number.max_length")
            .When(x => !string.IsNullOrWhiteSpace(x.CellPhoneNumber));

        RuleFor(x => x.Email)
            .MaximumLength(500)
            .WithMessage("client_individual.email.max_length")
            .EmailAddress()
            .WithMessage("client_individual.email.invalid")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Gender)
            .MaximumLength(20)
            .WithMessage("client_individual.gender.max_length")
            .When(x => !string.IsNullOrWhiteSpace(x.Gender));

        RuleFor(x => x.DocumentType)
            .MaximumLength(50)
            .WithMessage("client_individual.document_type.max_length")
            .When(x => !string.IsNullOrWhiteSpace(x.DocumentType));

        RuleFor(x => x.DocumentNumber)
            .MaximumLength(50)
            .WithMessage("client_individual.document_number.max_length")
            .When(x => !string.IsNullOrWhiteSpace(x.DocumentNumber));

        RuleFor(x => x.Nationality)
            .Length(2)
            .WithMessage("client_individual.nationality.invalid_length")
            .When(x => !string.IsNullOrWhiteSpace(x.Nationality));
    }
}
