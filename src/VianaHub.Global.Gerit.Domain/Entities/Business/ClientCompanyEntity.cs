using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

public class ClientCompanyEntity : Entity
{
    private readonly List<ClientCompanyFiscalDataEntity> _fiscalData = [];

    public int TenantId { get; private set; }
    public int ClientId { get; private set; }

    public string LegalName { get; private set; }
    public string TradeName { get; private set; }
    public string PhoneNumber { get; private set; }
    public string CellPhoneNumber { get; private set; }
    public bool IsWhatsapp { get; private set; } = false;
    public string Email { get; private set; }
    public string Site { get; private set; }
    public string CompanyRegistration { get; private set; }
    public string CAE { get; private set; }
    public int? NumberOfEmployee { get; private set; }
    public string LegalRepresentative { get; private set; }

    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public ClientEntity Client { get; private set; } = null!;

    public IReadOnlyCollection<ClientCompanyFiscalDataEntity> FiscalData => _fiscalData.AsReadOnly();

    // Construtor protegido para EF Core
    protected ClientCompanyEntity() { }

    public ClientCompanyEntity(int tenantId, string legalName, string tradeName, string phoneMumber, string cellPhoneNumber, bool isWhatsapp, string email, string site, string companyRegistration, string cae, int? numberOfEmployee, string legalRepresentative, int createdBy)
    {
        TenantId = tenantId;
        ClientId = Id;

        LegalName = legalName;
        TradeName = tradeName;
        PhoneNumber = phoneMumber;
        CellPhoneNumber = cellPhoneNumber;
        IsWhatsapp = isWhatsapp;
        Email = email;
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

    public void Update(string legalName, string tradeName, string phoneMumber, string cellPhoneNumber, bool isWhatsapp, string email, string site, string companyRegistration, string cae, int? numberOfEmployee, string legalRepresentative, int modifiedBy)
    {
        LegalName = legalName;
        TradeName = tradeName;
        PhoneNumber = phoneMumber;
        CellPhoneNumber = cellPhoneNumber;
        IsWhatsapp = isWhatsapp;
        Email = email;
        Site = site;
        CompanyRegistration = companyRegistration;
        CAE = cae;
        NumberOfEmployee = numberOfEmployee;
        LegalRepresentative = legalRepresentative;
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
        IsActive = false;
        IsDeleted = true;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
}