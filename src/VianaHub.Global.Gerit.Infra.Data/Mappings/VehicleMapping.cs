using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings;

/// <summary>
/// Mapeamento da entidade Vehicle
/// Veículos do tenant com suporte a Row Level Security
/// </summary>
public class VehicleMapping : IEntityTypeConfiguration<VehicleEntity>
{
    public void Configure(EntityTypeBuilder<VehicleEntity> builder)
    {
        builder.ToTable("Vehicles", "dbo");

        // Chave Primária
        builder.HasKey(v => v.Id)
            .HasName("PK_Vehicles");

        builder.Property(v => v.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(v => v.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(v => v.Plate)
            .HasColumnName("Plate")
            .HasColumnType("NVARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(v => v.Model)
            .HasColumnName("Model")
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(v => v.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(v => v.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(v => v.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(v => v.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(v => v.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(v => v.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Constraints únicos
        builder.HasIndex(v => new { v.TenantId, v.Plate })
            .IsUnique()
            .HasDatabaseName("UQ_Vehicles_Tenant_Plate");

        // Relacionamentos
        builder.HasOne(v => v.Tenant)
            .WithMany()
            .HasForeignKey(v => v.TenantId)
            .HasConstraintName("FK_Vehicles_Tenants")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
