using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

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
        builder.HasKey(x => x.Id)
            .HasName("PK_Vehicles");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Chave alternativa para suportar FKs compostas com TenantId
        builder.HasAlternateKey(x => new { x.Id, x.TenantId })
            .HasName("UQ_Vehicles_Id_Tenant");

        // Propriedades
        builder.Property(x => x.TenantId)
            .IsRequired();

        builder.Property(x => x.StatusId)
            .IsRequired();

        builder.Property(x => x.Plate)
            .HasColumnType("NVARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Brand)
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Model)
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Year)
            .IsRequired();

        builder.Property(x => x.Color)
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(x => x.FuelType)
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired(false);

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

        // Constraints únicos
        builder.HasIndex(v => new { v.TenantId, v.Plate })
            .IsUnique()
            .HasDatabaseName("UQ_Vehicles_Tenant_Plate");

        // Relacionamentos
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .HasConstraintName("FK_Vehicles_Tenants")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Status)
            .WithMany()
            .HasForeignKey(x => x.StatusId)
            .HasConstraintName("FK_Vehicles_Status")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
