using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Employee;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.Employee;

public class CreateEmployeeRouteValidator : AbstractValidator<CreateEmployeeRequest>
{
    public CreateEmployeeRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.Employee.Create.Name"))
            .MaximumLength(150).WithMessage(localization.GetMessage("Api.Validator.Employee.Create.Name.MaximumLength", 150));

        RuleFor(x => x.StatusDefinitionId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.Employee.Create.StatusDefinitionId"));

        RuleFor(x => x.StatusDomainId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.Employee.Create.StatusDomainId"));

        RuleFor(x => x.Email)
            .MaximumLength(250).WithMessage(localization.GetMessage("Api.Validator.Employee.Create.Email.MaximumLength", 250))
            .EmailAddress().WithMessage(localization.GetMessage("Api.Validator.Employee.Create.Email.Invalid"));

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(30).WithMessage(localization.GetMessage("Api.Validator.Employee.Create.PhoneNumber.MaximumLength", 30));

        RuleFor(x => x.CellPhoneNumber)
            .MaximumLength(30).WithMessage(localization.GetMessage("Api.Validator.Employee.Create.CellPhoneNumber.MaximumLength", 30));

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500).WithMessage(localization.GetMessage("Api.Validator.Employee.Create.ImageUrl.MaximumLength", 500));
    }
}
