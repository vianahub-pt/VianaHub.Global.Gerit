using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings;

/// <summary>
/// Mapeamento da entidade Tenant
/// Tabela principal de tenants com suporte a Row Level Security
/// </summary>
public class TenantMapping : IEntityTypeConfiguration<TenantEntity>
{
    public void Configure(EntityTypeBuilder<TenantEntity> builder)
    {
        builder.ToTable("Tenants", "dbo");

        // Chave Primária
        builder.HasKey(t => t.Id)
            .HasName("PK_Tenants");

        builder.Property(t => t.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(t => t.LegalName)
            .HasColumnName("LegalName")
            .HasColumnType("NVARCHAR(200)")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.TradeName)
            .HasColumnName("TradeName")
            .HasColumnType("NVARCHAR(200)")
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(t => t.Consent)
            .HasColumnName("Consent")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(t => t.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(t => t.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(t => t.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(t => t.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Relacionamentos
        builder.HasMany(t => t.Contacts)
            .WithOne(tc => tc.Tenant)
            .HasForeignKey(tc => tc.TenantId)
            .HasConstraintName("FK_TenantContacts_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.Addresses)
            .WithOne(ta => ta.Tenant)
            .HasForeignKey(ta => ta.TenantId)
            .HasConstraintName("FK_TenantAddresses_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.FiscalData)
            .WithOne(tfd => tfd.Tenant)
            .HasForeignKey(tfd => tfd.TenantId)
            .HasConstraintName("FK_TenantFiscalData_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.Users)
            .WithOne(u => u.Tenant)
            .HasForeignKey(u => u.TenantId)
            .HasConstraintName("FK_Users_Tenant")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
