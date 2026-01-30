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

        // Chave Primária
        builder.HasKey(tfd => tfd.Id)
            .HasName("PK_TenantFiscalData");

        builder.Property(tfd => tfd.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(tfd => tfd.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(tfd => tfd.NIF)
            .HasColumnName("NIF")
            .HasColumnType("CHAR(9)")
            .HasMaxLength(9)
            .IsRequired();

        builder.Property(tfd => tfd.VATNumber)
            .HasColumnName("VATNumber")
            .HasColumnType("NVARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(tfd => tfd.CAE)
            .HasColumnName("CAE")
            .HasColumnType("NVARCHAR(10)")
            .HasMaxLength(10)
            .IsRequired(false);

        builder.Property(tfd => tfd.FiscalCountry)
            .HasColumnName("FiscalCountry")
            .HasColumnType("CHAR(2)")
            .HasMaxLength(2)
            .HasDefaultValue("PT")
            .IsRequired();

        builder.Property(tfd => tfd.IsVATRegistered)
            .HasColumnName("IsVATRegistered")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(tfd => tfd.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(tfd => tfd.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(tfd => tfd.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(tfd => tfd.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(tfd => tfd.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(tfd => tfd.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Constraints únicos
        builder.HasIndex(tfd => tfd.NIF)
            .IsUnique()
            .HasDatabaseName("UQ_TenantFiscalData_NIF");

        // Relacionamentos configurados no TenantMapping
    }
}
