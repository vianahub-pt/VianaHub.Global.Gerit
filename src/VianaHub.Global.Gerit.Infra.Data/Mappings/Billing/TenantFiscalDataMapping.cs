using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Billing;

/// <summary>
/// Mapeamento da entidade TenantFiscalData
/// Dados fiscais do tenant com suporte a Row Level Security
/// </summary>
public class TenantFiscalDataMapping : IEntityTypeConfiguration<TenantFiscalDataEntity>
{
    public void Configure(EntityTypeBuilder<TenantFiscalDataEntity> builder)
    {
        builder.ToTable("TenantFiscalData", "dbo");

        // Chave Primaria
        builder.HasKey(x => x.Id)
            .HasName("PK_TenantFiscalData");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.TenantId)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(x => x.TaxNumber)
            .HasColumnType("NVARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.VatNumber)
            .HasColumnType("NVARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(x => x.IBAN)
            .HasColumnType("NVARCHAR(34)")
            .HasMaxLength(34)
            .IsRequired(false);

        builder.Property(x => x.FiscalEmail)
            .HasColumnType("NVARCHAR(255)")
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(x => x.FiscalCountry)
            .HasColumnType("CHAR(2)")
            .HasMaxLength(2)
            .HasDefaultValue("PT")
            .IsRequired();

        builder.Property(x => x.IsVatRegistered)
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
              .HasColumnType("INT")
              .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnType("DATETIME2(7)")
            .HasDefaultValueSql("SYSUTCDATETIME()")
            .IsRequired();

        builder.Property(x => x.ModifiedBy)
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(x => x.ModifiedAt)
            .HasColumnType("DATETIME2(7)")
            .IsRequired(false);

        // Relacionamento
        builder.HasOne(x => x.Tenant)
            .WithMany(t => t.FiscalData)
            .HasForeignKey(x => x.TenantId)
            .HasConstraintName("FK_TenantFiscalData_Tenant")
            .OnDelete(DeleteBehavior.NoAction);

        // Índice único filtrado: TaxNumber único por Tenant+FiscalCountry
        builder.HasIndex(x => new { x.TenantId, x.FiscalCountry, x.TaxNumber })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0")
            .HasDatabaseName("UX_TenantFiscalData_TaxNumber");

        // Índice único filtrado: apenas um registro fiscal ativo por tenant
        builder.HasIndex(x => new { x.TenantId })
            .IsUnique()
            .HasFilter("[IsActive] = 1 AND [IsDeleted] = 0")
            .HasDatabaseName("UX_TenantFiscalData_Active");

        // Índice filtrado para consultas por tenant
        builder.HasIndex(x => x.TenantId)
            .HasFilter("[IsDeleted] = 0")
            .HasDatabaseName("IX_TenantFiscalData_TenantId");
    }
}
