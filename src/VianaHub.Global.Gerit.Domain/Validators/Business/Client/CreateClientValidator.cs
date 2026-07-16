using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.Client;

/// <summary>
/// Validador para cria��o de Client
/// </summary>
public class CreateClientValidator : AbstractValidator<ClientEntity>
{
    public CreateClientValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Client.TenantIdRequired"));

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Client.CreatedByRequired"));

        RuleFor(x => x.AcquisitionSourceTypeId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Client.AcquisitionSourceTypeIdRequired"));

        // CK_Clients_PartyTypeData: When PartyTypeId=1 (individual), company fields must be null
        RuleFor(x => x.CompanyRegistrationNumber)
            .Null()
            .WithMessage(localization.GetMessage("Domain.Client.PartyTypeIndividualCompanyFieldsMustBeNull"))
            .When(x => x.PartyTypeId == 1);

        RuleFor(x => x.EconomicActivityCode)
            .Null()
            .WithMessage(localization.GetMessage("Domain.Client.PartyTypeIndividualCompanyFieldsMustBeNull"))
            .When(x => x.PartyTypeId == 1);

        RuleFor(x => x.NumberOfEmployees)
            .Null()
            .WithMessage(localization.GetMessage("Domain.Client.PartyTypeIndividualCompanyFieldsMustBeNull"))
            .When(x => x.PartyTypeId == 1);
    }
}
