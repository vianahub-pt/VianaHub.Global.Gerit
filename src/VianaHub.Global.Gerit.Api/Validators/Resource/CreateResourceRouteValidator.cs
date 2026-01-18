using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Resource;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Resource;

public class CreateResourceRouteValidator : AbstractValidator<CreateResourceRequest>
{
    private readonly ILocalizationService _localization;

    public CreateResourceRouteValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(_localization.GetMessage("Api.Validator.Resource.Create.Name"))
            .MaximumLength(100).WithMessage(_localization.GetMessage("Api.Validator.Resource.Create.Name.MaximumLength", 100));

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage(_localization.GetMessage("Api.Validator.Resource.Create.Description.MaximumLength", 255));
    }
}
