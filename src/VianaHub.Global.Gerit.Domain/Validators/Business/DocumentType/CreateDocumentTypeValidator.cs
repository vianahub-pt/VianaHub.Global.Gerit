using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.DocumentType;

public class CreateDocumentTypeValidator : AbstractValidator<DocumentTypeEntity>
{
    public CreateDocumentTypeValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.DocumentType.CodeRequired"))
            .MaximumLength(50)
            .WithMessage(localization.GetMessage("Domain.DocumentType.CodeMaxLength", 50));

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.DocumentType.CreatedByRequired"));
    }
}
