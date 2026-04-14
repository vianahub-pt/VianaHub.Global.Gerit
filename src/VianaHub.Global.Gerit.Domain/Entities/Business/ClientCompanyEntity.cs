using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

public class ClientCompanyEntity : Entity
{
    public int TenantId { get; private set; }
    public int ClientId { get; private set; }

    public string LegalName { get; private set; } = null!;
    public string TradeName { get; private set; }
    public string Site { get; private set; }
    public string CompanyRegistration { get; private set; }
    public string CAE { get; private set; }
    public int? NumberOfEmployee { get; private set; }
    public string LegalRepresentative { get; private set; }

    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public ClientEntity Client { get; private set; } = null!;
    public ClientCompanyFiscalDataEntity? FiscalData { get; private set; }

    // Construtor protegido para EF Core
    protected ClientCompanyEntity() { }

    public ClientCompanyEntity(int tenantId, int clientId, string legalName, string tradeName, string site, string companyRegistration, string cae, int numberOfEmployee, string legalRepresentative, int createdBy)
    {
        TenantId = tenantId;
        ClientId = clientId;

        LegalName = legalName;
        TradeName = tradeName;
        Site = site;
        CompanyRegistration = companyRegistration;
        CAE = cae;
        NumberOfEmployee = numberOfEmployee;
        LegalRepresentative = legalRepresentative;

        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public string DisplayName => TradeName ?? LegalName;

    public void Update(string legalName, string tradeName, string site, string companyRegistration, string cae, int numberOfEmployee, string legalRepresentative, int modifiedBy)
    {
        LegalName = legalName;
        TradeName = tradeName;
        Site = site;
        CompanyRegistration = companyRegistration;
        CAE = cae;
        NumberOfEmployee = numberOfEmployee;
        LegalRepresentative = legalRepresentative;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void SetFiscalData(ClientCompanyFiscalDataEntity fiscalData)
    {
        FiscalData = fiscalData;
    }

    public void RemoveFiscalData(int modifiedBy)
    {
        if (FiscalData is null)
            return;

        FiscalData.Delete(modifiedBy);
        FiscalData = null;
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
        FiscalData?.Delete(modifiedBy);

        IsActive = false;
        IsDeleted = true;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
}