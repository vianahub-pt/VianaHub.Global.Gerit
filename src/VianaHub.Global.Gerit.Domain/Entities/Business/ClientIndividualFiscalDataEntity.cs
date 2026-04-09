using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa dados fiscais de um Cliente Individual
/// </summary>
public class ClientIndividualFiscalDataEntity : Entity
{
    public int TenantId { get; private set; }
    public int ClientIndividualId { get; private set; }
    public string TaxNumber { get; private set; }
    public string VatNumber { get; private set; }
    public string FiscalCountry { get; private set; }
    public bool IsVatRegistered { get; private set; }
    public string IBAN { get; private set; }
    public string FiscalEmail { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }
    public ClientIndividualEntity ClientIndividual { get; private set; }

    protected ClientIndividualFiscalDataEntity() { }

    public ClientIndividualFiscalDataEntity(int tenantId, int clientIndividualId, string taxNumber,
        string vatNumber, string fiscalCountry, bool isVatRegistered, string iban, string fiscalEmail, int createdBy)
    {
        TenantId = tenantId;
        ClientIndividualId = clientIndividualId;
        TaxNumber = taxNumber;
        VatNumber = vatNumber;
        FiscalCountry = fiscalCountry;
        IsVatRegistered = isVatRegistered;
        IBAN = iban;
        FiscalEmail = fiscalEmail;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string taxNumber, string vatNumber, string fiscalCountry, bool isVatRegistered,
        string iban, string fiscalEmail, int modifiedBy)
    {
        TaxNumber = taxNumber;
        VatNumber = vatNumber;
        FiscalCountry = fiscalCountry;
        IsVatRegistered = isVatRegistered;
        IBAN = iban;
        FiscalEmail = fiscalEmail;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Activate(int modifiedBy)
    {
        IsActive = true;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Deactivate(int modifiedBy)
    {
        IsActive = false;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Delete(int modifiedBy)
    {
        IsDeleted = true;
        IsActive = false;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
}
