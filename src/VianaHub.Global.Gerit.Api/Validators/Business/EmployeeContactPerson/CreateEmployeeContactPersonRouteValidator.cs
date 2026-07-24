using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.EmployeeContact;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.EmployeeContact;

public class CreateEmployeeContactPersonRouteValidator : AbstractValidator<CreateEmployeeContactPersonRequest>
{
    public CreateEmployeeContactPersonRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.EmployeeContactPerson.Create.EmployeeId"));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.EmployeeContactPerson.Create.Name"))
            .MaximumLength(150).WithMessage(localization.GetMessage("Api.Validator.EmployeeContactPerson.Create.Name.MaximumLength", 150));

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.EmployeeContactPerson.Create.Email"))
            .MaximumLength(255).WithMessage(localization.GetMessage("Api.Validator.EmployeeContactPerson.Create.Email.MaximumLength", 255))
            .EmailAddress().WithMessage(localization.GetMessage("Api.Validator.EmployeeContactPerson.Create.Email.Invalid"));

        RuleFor(x => x.Phone)
            .MaximumLength(30).When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage(localization.GetMessage("Api.Validator.EmployeeContactPerson.Create.Phone.MaximumLength", 30));
    }
}
