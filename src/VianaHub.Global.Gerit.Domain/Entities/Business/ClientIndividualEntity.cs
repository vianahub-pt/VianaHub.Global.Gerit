using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities.Business
{
    public class ClientIndividualEntity : Entity
    {
        public int TenantId { get; private set; }
        public int ClientId { get; private set; }

        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public DateTime BirthDate { get; private set; }
        public string Gender { get; private set; }
        public string DocumentType { get; private set; }
        public string DocumentNumber { get; private set; }
        public string Nationality { get; private set; }

        public bool IsActive { get; private set; }
        public bool IsDeleted { get; private set; }

        // Navigation
        public ClientEntity Client { get; private set; } = null!;
        public ClientIndividualFiscalDataEntity? FiscalData { get; private set; }

        // EF
        protected ClientIndividualEntity() { }

        public ClientIndividualEntity(int tenantId, int clientId, string firstName, string lastName, DateTime birthDate, string gender, string documentType, string documentNumber, string nationality, int createdBy)
        {
            if (tenantId <= 0)
                throw new ArgumentException("TenantId inválido.", nameof(tenantId));

            if (clientId <= 0)
                throw new ArgumentException("ClientId inválido.", nameof(clientId));

            ValidateBirthDate(birthDate);

            TenantId = tenantId;
            ClientId = clientId;

            FirstName = Require(firstName, nameof(firstName), 100);
            LastName = Require(lastName, nameof(lastName), 100);
            BirthDate = birthDate;
            Gender = NormalizeNullable(gender, 20);
            DocumentType = NormalizeNullable(documentType, 50);
            DocumentNumber = NormalizeNullable(documentNumber, 50);
            Nationality = NormalizeCountryCodeNullable(nationality);

            IsActive = true;
            IsDeleted = false;
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
        }

        public string FullName => $"{FirstName} {LastName}".Trim();

        public void Update(
            string firstName,
            string lastName,
            DateTime birthDate,
            string gender,
            string documentType,
            string documentNumber,
            string nationality,
            int modifiedBy)
        {
            EnsureNotDeleted();
            ValidateBirthDate(birthDate);

            FirstName = Require(firstName, nameof(firstName), 100);
            LastName = Require(lastName, nameof(lastName), 100);
            BirthDate = birthDate;
            Gender = NormalizeNullable(gender, 20);
            DocumentType = NormalizeNullable(documentType, 50);
            DocumentNumber = NormalizeNullable(documentNumber, 50);
            Nationality = NormalizeCountryCodeNullable(nationality);
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.UtcNow;
        }

        public void SetFiscalData(ClientIndividualFiscalDataEntity fiscalData)
        {
            EnsureNotDeleted();

            if (fiscalData is null)
                throw new ArgumentNullException(nameof(fiscalData));

            if (fiscalData.TenantId != TenantId)
                throw new InvalidOperationException("Tenant inconsistente no FiscalData.");

            FiscalData = fiscalData;
        }

        public void RemoveFiscalData(int modifiedBy)
        {
            EnsureNotDeleted();

            if (FiscalData is null)
                return;

            FiscalData.Delete(modifiedBy);
            FiscalData = null;
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

            FiscalData?.Delete(modifiedBy);

            IsActive = false;
            IsDeleted = true;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.UtcNow;
        }

        private void EnsureNotDeleted()
        {
            if (IsDeleted)
                throw new InvalidOperationException("ClientIndividual removido.");
        }

        private static void ValidateBirthDate(DateTime? birthDate)
        {
            if (birthDate.HasValue && birthDate.Value.Date > DateTime.UtcNow.Date)
                throw new ArgumentException("BirthDate não pode ser futura.", nameof(birthDate));
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

        private static string? NormalizeCountryCodeNullable(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var normalized = value.Trim().ToUpperInvariant();

            if (normalized.Length != 2)
                throw new ArgumentException("Nationality deve ser ISO-2.", nameof(value));

            return normalized;
        }
    }
}