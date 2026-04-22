using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

public class ClientCompanyFiscalDataMapping : IEntityTypeConfiguration<ClientCompanyFiscalDataEntity>
{
    public void Configure(EntityTypeBuilder<ClientCompanyFiscalDataEntity> builder)
    {
        builder.ToTable("ClientCompanyFiscalData");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.TenantId)
            .IsRequired();

        builder.Property(x => x.ClientCompanyId)
            .IsRequired();

        builder.Property(x => x.TaxNumber)
            .HasColumnType("NVARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.VatNumber)
            .HasColumnType("NVARCHAR(20)")
            .HasMaxLength(20);

        builder.Property(x => x.FiscalCountry)
            .HasColumnType("CHAR(2)")
            .HasMaxLength(2)
            .HasDefaultValue("PT")
            .IsRequired();

        builder.Property(x => x.IsVatRegistered)
            .HasDefaultValue(false);

        builder.Property(x => x.IBAN)
            .HasColumnType("NVARCHAR(34)")
            .HasMaxLength(34);

        builder.Property(x => x.FiscalEmail)
            .HasColumnType("NVARCHAR(255)")
            .HasMaxLength(255);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnType("DATETIME2(7)")
            .IsRequired();

        builder.Property(x => x.ModifiedBy)
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(x => x.ModifiedAt)
            .HasColumnType("DATETIME2(7)")
            .IsRequired(false);

        // Índices
        builder.HasIndex(x => new { x.TenantId, x.ClientCompanyId })
            .HasFilter("[IsActive] = 1 AND [IsDeleted] = 0")
            .IsUnique()
            .HasDatabaseName("UX_ClientCompanyFiscalData_Active");

        builder.HasIndex(x => new { x.TenantId, x.TaxNumber })
            .HasFilter("[TaxNumber] IS NOT NULL AND [IsDeleted] = 0")
            .IsUnique()
            .HasDatabaseName("UX_ClientCompanieFiscalData_TaxNumber");

        // Constraints
        builder.HasCheckConstraint("CK_ClientCompanyFiscalData_Active_Deleted",
            "NOT ([IsActive] = 1 AND [IsDeleted] = 1)");
    }
}

