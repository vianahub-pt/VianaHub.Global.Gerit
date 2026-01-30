using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade InterventionAddress
/// Endereços da intervenção com suporte a Row Level Security
/// </summary>
public class InterventionAddressMapping : IEntityTypeConfiguration<InterventionAddressEntity>
{
    public void Configure(EntityTypeBuilder<InterventionAddressEntity> builder)
    {
        builder.ToTable("InterventionAddresses", "dbo");

        // Chave Primária
        builder.HasKey(ia => ia.Id)
            .HasName("PK_InterventionAddresses");

        builder.Property(ia => ia.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(ia => ia.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(ia => ia.InterventionId)
            .HasColumnName("InterventionId")
            .IsRequired();

        builder.Property(ia => ia.Street)
            .HasColumnName("Street")
            .HasColumnType("NVARCHAR(200)")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(ia => ia.City)
            .HasColumnName("City")
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(ia => ia.PostalCode)
            .HasColumnName("PostalCode")
            .HasColumnType("NVARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(ia => ia.District)
            .HasColumnName("District")
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(ia => ia.CountryCode)
            .HasColumnName("CountryCode")
            .HasColumnType("CHAR(2)")
            .HasMaxLength(2)
            .HasDefaultValue("PT")
            .IsRequired();

        builder.Property(ia => ia.IsPrimary)
            .HasColumnName("IsPrimary")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(ia => ia.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(ia => ia.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(ia => ia.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(ia => ia.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(ia => ia.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(ia => ia.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Índices
        builder.HasIndex(ia => ia.InterventionId)
            .HasDatabaseName("IX_InterventionAddresses_InterventionId")
            .IncludeProperties(ia => ia.TenantId)
            .HasFilter("[IsDeleted] = 0");

        // Relacionamentos
        builder.HasOne(ia => ia.Tenant)
            .WithMany()
            .HasForeignKey(ia => ia.TenantId)
            .HasConstraintName("FK_InterventionAddresses_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento com Intervention configurado no InterventionMapping
    }
}
