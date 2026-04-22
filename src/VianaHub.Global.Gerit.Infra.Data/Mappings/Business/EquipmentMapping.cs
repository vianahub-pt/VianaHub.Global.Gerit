using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade Equipment
/// Equipamentos do tenant com suporte a Row Level Security
/// </summary>
public class EquipmentMapping : IEntityTypeConfiguration<EquipmentEntity>
{
    public void Configure(EntityTypeBuilder<EquipmentEntity> builder)
    {
        builder.ToTable("Equipments", "dbo");

        // Chave Primária
        builder.HasKey(x => x.Id)
            .HasName("PK_Equipments");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.TenantId)
            .IsRequired();

        builder.Property(x => x.EquipmentTypeId)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(x => x.StatusId)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnType("NVARCHAR(150)")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.SerialNumber)
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
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

        // Relacionamentos
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .HasConstraintName("FK_Equipments_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.EquipmentType)
            .WithMany()
            .HasForeignKey(x => new { x.EquipmentTypeId, x.TenantId })
            .HasPrincipalKey(x => new { x.Id, x.TenantId })
            .HasConstraintName("FK_Equipments_EquipamentType")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Status)
            .WithMany()
            .HasForeignKey(x => new { x.StatusId, x.TenantId })
            .HasPrincipalKey(x => new { x.Id, x.TenantId })
            .HasConstraintName("FK_Equipments_Status")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
