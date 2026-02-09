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
        builder.HasKey(x => x.Id)
            .HasName("PK_TenantFiscalData");

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.TenantId)
            .HasColumnName("TenantId")
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(x => x.NIF)
            .HasColumnName("NIF")
            .HasColumnType("CHAR(9)")
            .HasMaxLength(9)
            .IsRequired();

        builder.Property(x => x.VATNumber)
            .HasColumnName("VATNumber")
            .HasColumnType("NVARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.CAE)
            .HasColumnName("CAE")
            .HasColumnType("NVARCHAR(10)")
            .HasMaxLength(10)
            .IsRequired(false);

        builder.Property(x => x.FiscalCountry)
            .HasColumnName("FiscalCountry")
            .HasColumnType("CHAR(2)")
            .HasMaxLength(2)
            .HasDefaultValue("PT")
            .IsRequired();

        builder.Property(x => x.IsVATRegistered)
            .HasColumnName("IsVATRegistered")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
              .HasColumnName("CreatedBy")
              .HasColumnType("INT")
              .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(x => x.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(x => x.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Relacionamento
        builder.HasOne(x => x.Tenant)
            .WithMany(t => t.FiscalData)
            .HasForeignKey(x => x.TenantId)
            .HasConstraintName("FK_TenantFiscalData_Tenant")
            .OnDelete(DeleteBehavior.NoAction);

        // Constraint Única: NIF único
        builder.HasIndex(x => x.NIF)
            .IsUnique()
            .HasDatabaseName("UQ_TenantFiscalData_NIF");

        // Constraint Única: Garantir que só pode haver um registro ativo por tenant (soft delete)
        builder.HasIndex(x => new { x.TenantId, x.IsActive })
            .IsUnique()
            .HasDatabaseName("UQ_TenantFiscalData_Tenant_Active");
    }
}
