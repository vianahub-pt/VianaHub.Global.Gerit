using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.FileType;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.FileType;

public class CreateFileTypeRouteValidator : AbstractValidator<CreateFileTypeRequest>
{
    public CreateFileTypeRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.MimeType)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.FileType.Create.MimeType"))
            .MaximumLength(100).WithMessage(localization.GetMessage("Api.Validator.FileType.Create.MimeType.MaximumLength", 100));

        RuleFor(x => x.Extension)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.FileType.Create.Extension"))
            .MaximumLength(20).WithMessage(localization.GetMessage("Api.Validator.FileType.Create.Extension.MaximumLength", 20));
    }
}
