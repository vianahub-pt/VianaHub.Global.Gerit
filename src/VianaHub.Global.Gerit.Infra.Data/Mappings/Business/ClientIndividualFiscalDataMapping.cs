using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

public class ClientIndividualFiscalDataMapping : IEntityTypeConfiguration<ClientIndividualFiscalDataEntity>
{
    public void Configure(EntityTypeBuilder<ClientIndividualFiscalDataEntity> builder)
    {
        builder.ToTable("ClientIndividualFiscalData");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("Id").IsRequired();

        builder.Property(x => x.TenantId).HasColumnName("TenantId").IsRequired();
        builder.Property(x => x.ClientIndividualId).HasColumnName("ClientIndividualId").IsRequired();

        builder.Property(x => x.TaxNumber).HasColumnName("TaxNumber").HasColumnType("NVARCHAR(20)").HasMaxLength(20).IsRequired();
        builder.Property(x => x.VatNumber).HasColumnName("VatNumber").HasColumnType("NVARCHAR(20)").HasMaxLength(20);
        builder.Property(x => x.FiscalCountry).HasColumnName("FiscalCountry").HasColumnType("CHAR(2)").HasMaxLength(2).IsRequired().HasDefaultValue("PT");
        builder.Property(x => x.IsVatRegistered).HasColumnName("IsVatRegistered").HasDefaultValue(false);
        builder.Property(x => x.IBAN).HasColumnName("IBAN").HasColumnType("NVARCHAR(34)").HasMaxLength(34);
        builder.Property(x => x.FiscalEmail).HasColumnName("FiscalEmail").HasColumnType("NVARCHAR(255)").HasMaxLength(255);

        builder.Property(x => x.IsActive).HasColumnName("IsActive").HasDefaultValue(true);
        builder.Property(x => x.IsDeleted).HasColumnName("IsDeleted").HasDefaultValue(false);
        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("CreatedAt").HasColumnType("DATETIME2(7)").IsRequired();
        builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
        builder.Property(x => x.ModifiedAt).HasColumnName("ModifiedAt").HasColumnType("DATETIME2(7)");

        // Relacionamentos
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ClientIndividual)
            .WithMany()
            .HasForeignKey(x => new { x.ClientIndividualId, x.TenantId })
            .HasPrincipalKey(x => new { x.Id, x.TenantId })
            .OnDelete(DeleteBehavior.Restrict);

        // Índices
        builder.HasIndex(x => new { x.TenantId, x.ClientIndividualId })
            .HasFilter("[IsActive] = 1 AND [IsDeleted] = 0")
            .IsUnique()
            .HasDatabaseName("UX_ClientIndividualFiscalData_Active");

        builder.HasIndex(x => new { x.TenantId, x.TaxNumber })
            .HasFilter("[TaxNumber] IS NOT NULL AND [IsDeleted] = 0")
            .IsUnique()
            .HasDatabaseName("UX_ClientIndividualFiscalData_TaxNumber");

        // Constraints
        builder.HasCheckConstraint("CK_ClientIndividualFiscalData_Active_Deleted",
            "NOT ([IsActive] = 1 AND [IsDeleted] = 1)");
    }
}
