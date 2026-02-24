using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.StatusType;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.StatusType;

public class CreateStatusTypeRouteValidator : AbstractValidator<CreateStatusTypeRequest>
{
    public CreateStatusTypeRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.StatusType.Create.Name"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Api.Validator.StatusType.Create.Name.MaximumLength", 150));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.StatusType.Create.Description"))
            .MaximumLength(500)
            .WithMessage(localization.GetMessage("Api.Validator.StatusType.Create.Description.MaximumLength", 500));
    }
}
