using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities.Billing;

/// <summary>
/// Entidade que representa os dados fiscais do Tenant
/// </summary>
public class TenantFiscalDataEntity : Entity
{
    public int TenantId { get; private set; }
    public string NIF { get; private set; }
    public string VATNumber { get; private set; }
    public string CAE { get; private set; }
    public string FiscalCountry { get; private set; }
    public bool IsVATRegistered { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Property
    public TenantEntity Tenant { get; private set; }

    // Construtor protegido para o EF Core
    protected TenantFiscalDataEntity() { }

    /// <summary>
    /// Construtor para criação de novos dados fiscais do Tenant
    /// </summary>
    public TenantFiscalDataEntity(int tenantId, string nif, string vatNumber, string cae, string fiscalCountry, bool isVATRegistered, int createdBy)
    {
        TenantId = tenantId;
        NIF = nif;
        VATNumber = vatNumber;
        CAE = cae;
        FiscalCountry = fiscalCountry;
        IsVATRegistered = isVATRegistered;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateFiscalData(string nif, string vatNumber, string cae, string fiscalCountry, bool isVATRegistered, int modifiedBy)
    {
        NIF = nif;
        VATNumber = vatNumber;
        CAE = cae;
        FiscalCountry = fiscalCountry;
        IsVATRegistered = isVATRegistered;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void RegisterForVAT(int modifiedBy)
    {
        IsVATRegistered = true;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UnregisterFromVAT(int modifiedBy)
    {
        IsVATRegistered = false;
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
