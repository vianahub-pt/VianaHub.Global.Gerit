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
        builder.HasKey(e => e.Id)
            .HasName("PK_Equipments");

        builder.Property(e => e.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(e => e.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(e => e.Name)
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR(150)")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(e => e.SerialNumber)
            .HasColumnName("SerialNumber")
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired(false);

        // Use INT to match the existing DB schema (columns are INT) so EF can materialize into byte-backed enums
        builder.Property(e => e.TypeEquipament)
            .HasColumnName("TypeEquipament")
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(e => e.Status)
            .HasColumnName("Status")
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(e => e.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(e => e.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        // Audit fields: use INT for CreatedBy/ModifiedBy to match Domain.Entity and DB schema
        builder.Property(e => e.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(e => e.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(e => e.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Relacionamentos
        builder.HasOne(e => e.Tenant)
            .WithMany()
            .HasForeignKey(e => e.TenantId)
            .HasConstraintName("FK_Equipments_Tenant")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
