using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities.Business
{
    public class ClientIndividualFiscalDataEntity : Entity
    {
        public int TenantId { get; private set; }
        public int ClientIndividualId { get; private set; }

        public string TaxNumber { get; private set; } = null!;
        public string VatNumber { get; private set; }
        public string FiscalCountry { get; private set; } = "PT";
        public bool IsVatRegistered { get; private set; }
        public string IBAN { get; private set; }
        public string FiscalEmail { get; private set; }

        public bool IsActive { get; private set; }
        public bool IsDeleted { get; private set; }

        // Navigation
        public ClientIndividualEntity ClientIndividual { get; private set; } = null!;

        // EF
        protected ClientIndividualFiscalDataEntity() { }

        public ClientIndividualFiscalDataEntity(int tenantId, int clientIndividualId, string taxNumber, string vatNumber, string fiscalCountry, bool isVatRegistered, string iban, string fiscalEmail, int createdBy)
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

        public void Update(string taxNumber, string vatNumber, string fiscalCountry, bool isVatRegistered, string iban, string fiscalEmail, int modifiedBy)
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
            IsActive = false;
            IsDeleted = true;

            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.UtcNow;
        }
    }
}