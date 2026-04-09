using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.FileType;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.FileType;

public class UpdateFileTypeRouteValidator : AbstractValidator<UpdateFileTypeRequest>
{
    public UpdateFileTypeRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.MimeType)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.FileType.Update.MimeType"))
            .MaximumLength(100).WithMessage(localization.GetMessage("Api.Validator.FileType.Update.MimeType.MaximumLength", 100));

        RuleFor(x => x.Extension)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.FileType.Update.Extension"))
            .MaximumLength(20).WithMessage(localization.GetMessage("Api.Validator.FileType.Update.Extension.MaximumLength", 20));
    }
}
