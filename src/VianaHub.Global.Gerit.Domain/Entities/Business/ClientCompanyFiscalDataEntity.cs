using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities.Business
{
    public class ClientCompanyFiscalDataEntity : Entity
    {
        public int TenantId { get; private set; }
        public int ClientCompanyId { get; private set; }

        public string TaxNumber { get; private set; } = null!;
        public string VatNumber { get; private set; }
        public string FiscalCountry { get; private set; } = null!;
        public bool IsVatRegistered { get; private set; }
        public string IBAN { get; private set; }
        public string FiscalEmail { get; private set; }

        public bool IsActive { get; private set; }
        public bool IsDeleted { get; private set; }

        // Navigation Properties
        public ClientCompanyEntity ClientCompany { get; private set; } = null!;

        // Construtor protegido para EF Core
        protected ClientCompanyFiscalDataEntity() { }

        public ClientCompanyFiscalDataEntity(int tenantId, int clientCompanyId, string taxNumber, string vatNumber, string fiscalCountry, bool isVatRegistered, string iban, string fiscalEmail, int createdBy)
        {
            if (tenantId <= 0)
                throw new ArgumentException("TenantId inválido.", nameof(tenantId));

            if (clientCompanyId <= 0)
                throw new ArgumentException("ClientCompanyId inválido.", nameof(clientCompanyId));

            TenantId = tenantId;
            ClientCompanyId = clientCompanyId;
            TaxNumber = Require(taxNumber, nameof(taxNumber), 20);
            VatNumber = NormalizeNullable(vatNumber, 20);
            FiscalCountry = NormalizeCountryCode(fiscalCountry);
            IsVatRegistered = isVatRegistered;
            IBAN = NormalizeNullable(iban, 34);
            FiscalEmail = NormalizeEmailNullable(fiscalEmail, 255);

            IsActive = true;
            IsDeleted = false;
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string taxNumber, string vatNumber, string fiscalCountry, bool isVatRegistered, string iban, string fiscalEmail, int modifiedBy)
        {
            EnsureNotDeleted();

            TaxNumber = Require(taxNumber, nameof(taxNumber), 20);
            VatNumber = NormalizeNullable(vatNumber, 20);
            FiscalCountry = NormalizeCountryCode(fiscalCountry);
            IsVatRegistered = isVatRegistered;
            IBAN = NormalizeNullable(iban, 34);
            FiscalEmail = NormalizeEmailNullable(fiscalEmail, 255);
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.UtcNow;
        }

        public void Activate(int modifiedBy)
        {
            EnsureNotDeleted();
            IsActive = true;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.UtcNow;
        }

        public void Deactivate(int modifiedBy)
        {
            EnsureNotDeleted();
            IsActive = false;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.UtcNow;
        }

        public void Delete(int modifiedBy)
        {
            if (IsDeleted)
                return;

            IsActive = false;
            IsDeleted = true;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.UtcNow;
        }

        private void EnsureNotDeleted()
        {
            if (IsDeleted)
                throw new InvalidOperationException("ClientCompanyFiscalData removido.");
        }

        private static string Require(string? value, string paramName, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{paramName} é obrigatório.", paramName);

            var normalized = value.Trim();

            if (normalized.Length > maxLength)
                throw new ArgumentException($"{paramName} excede {maxLength} caracteres.", paramName);

            return normalized;
        }

        private static string? NormalizeNullable(string? value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var normalized = value.Trim();

            if (normalized.Length > maxLength)
                throw new ArgumentException($"Valor excede {maxLength} caracteres.", nameof(value));

            return normalized;
        }

        private static string NormalizeCountryCode(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("FiscalCountry é obrigatório.", nameof(value));

            var normalized = value.Trim().ToUpperInvariant();

            if (normalized.Length != 2)
                throw new ArgumentException("FiscalCountry deve ser ISO-2.", nameof(value));

            return normalized;
        }

        private static string? NormalizeEmailNullable(string? value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var normalized = value.Trim();

            if (normalized.Length > maxLength)
                throw new ArgumentException($"FiscalEmail excede {maxLength} caracteres.", nameof(value));

            if (!normalized.Contains("@"))
                throw new ArgumentException("FiscalEmail inválido.", nameof(value));

            return normalized;
        }
    }
}