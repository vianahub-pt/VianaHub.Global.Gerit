using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Billing;

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
        builder.HasKey(x => x.Id)
            .HasName("PK_Tenants");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.Name)
            .HasColumnType("NVARCHAR(200)")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnType("NVARCHAR(255)")
            .IsRequired();

        builder.Property(x => x.Website)
            .HasColumnType("NVARCHAR(255)");

        builder.Property(x => x.UrlImage)
            .HasColumnType("NVARCHAR(500)");

        builder.Property(x => x.Note)
            .HasColumnType("NVARCHAR(1000)");

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
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(x => x.ModifiedBy)
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(x => x.ModifiedAt)
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Relacionamentos
        builder.HasMany(x => x.Contacts)
            .WithOne(tc => tc.Tenant)
            .HasForeignKey(tc => tc.TenantId)
            .HasConstraintName("FK_TenantContacts_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Addresses)
            .WithOne(ta => ta.Tenant)
            .HasForeignKey(ta => ta.TenantId)
            .HasConstraintName("FK_TenantAddresses_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.FiscalData)
            .WithOne(tfd => tfd.Tenant)
            .HasForeignKey(tfd => tfd.TenantId)
            .HasConstraintName("FK_TenantFiscalData_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Users)
            .WithOne(u => u.Tenant)
            .HasForeignKey(u => u.TenantId)
            .HasConstraintName("FK_Users_Tenant")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
