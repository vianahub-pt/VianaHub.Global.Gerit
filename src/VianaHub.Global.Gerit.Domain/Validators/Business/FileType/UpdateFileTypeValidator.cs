using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.FileType;

public class UpdateFileTypeValidator : AbstractValidator<FileTypeEntity>
{
    public UpdateFileTypeValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.FileType.IdRequired"));

        RuleFor(x => x.MimeType)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.FileType.MimeTypeRequired"))
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.FileType.MimeTypeMaxLength", 100));

        RuleFor(x => x.Extension)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.FileType.ExtensionRequired"))
            .MaximumLength(20)
            .WithMessage(localization.GetMessage("Domain.FileType.ExtensionMaxLength", 20));

        RuleFor(x => x.ModifiedBy)
            .NotNull()
            .WithMessage(localization.GetMessage("Domain.FileType.ModifiedByRequired"))
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.FileType.ModifiedByRequired"));
    }
}
