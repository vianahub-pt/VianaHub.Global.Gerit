using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Employee;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.Employee;

public class UpdateEmployeeRouteValidator : AbstractValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.Employee.Update.Name"))
            .MaximumLength(150).WithMessage(localization.GetMessage("Api.Validator.Employee.Update.Name.MaximumLength", 150));

        RuleFor(x => x.StatusDefinitionId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.Employee.Update.StatusDefinitionId"));

        RuleFor(x => x.StatusDomainId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.Employee.Update.StatusDomainId"));

        RuleFor(x => x.Email)
            .MaximumLength(250).WithMessage(localization.GetMessage("Api.Validator.Employee.Update.Email.MaximumLength", 250))
            .EmailAddress().WithMessage(localization.GetMessage("Api.Validator.Employee.Update.Email.Invalid"));

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(30).WithMessage(localization.GetMessage("Api.Validator.Employee.Update.PhoneNumber.MaximumLength", 30));

        RuleFor(x => x.CellPhoneNumber)
            .MaximumLength(30).WithMessage(localization.GetMessage("Api.Validator.Employee.Update.CellPhoneNumber.MaximumLength", 30));

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500).WithMessage(localization.GetMessage("Api.Validator.Employee.Update.ImageUrl.MaximumLength", 500));
    }
}
