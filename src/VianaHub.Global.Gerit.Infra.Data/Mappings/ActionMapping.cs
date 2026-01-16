using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings;

/// <summary>
/// Mapeamento da entidade ActionEntity
/// Ações possíveis no sistema (não possui TenantId pois são globais)
/// </summary>
public class ActionMapping : IEntityTypeConfiguration<Domain.Entities.ActionEntity>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.ActionEntity> builder)
    {
        builder.ToTable("Actions", "dbo");

        // Chave Primária
        builder.HasKey(a => a.Id)
            .HasName("PK_Actions");

        builder.Property(a => a.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(a => a.Name)
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(a => a.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(a => a.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(a => a.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(a => a.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(a => a.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Constraints únicos
        builder.HasIndex(a => a.Name)
            .IsUnique()
            .HasDatabaseName("UQ_Actions_Name");

        // Relacionamentos
        builder.HasMany(a => a.RolePermissions)
            .WithOne(rp => rp.Action)
            .HasForeignKey(rp => rp.ActionId)
            .HasConstraintName("FK_RolePermissions_Action")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
