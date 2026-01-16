using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings;

/// <summary>
/// Mapeamento da entidade Role
/// Roles por tenant com suporte a Row Level Security
/// </summary>
public class RoleMapping : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.ToTable("Roles", "dbo");

        // Chave Primária
        builder.HasKey(r => r.Id)
            .HasName("PK_Roles");

        builder.Property(r => r.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(r => r.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(r => r.Name)
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(r => r.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(r => r.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(r => r.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(r => r.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(r => r.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(r => r.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Constraints únicos
        builder.HasIndex(r => new { r.TenantId, r.Name })
            .IsUnique()
            .HasDatabaseName("UQ_Roles_Tenant_Name");

        // Relacionamentos
        builder.HasOne(r => r.Tenant)
            .WithMany()
            .HasForeignKey(r => r.TenantId)
            .HasConstraintName("FK_Roles_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(r => r.Permissions)
            .WithOne(rp => rp.Role)
            .HasForeignKey(rp => rp.RoleId)
            .HasConstraintName("FK_RolePermissions_Role")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(r => r.UserRoles)
            .WithOne(ur => ur.Role)
            .HasForeignKey(ur => ur.RoleId)
            .HasConstraintName("FK_UserRoles_Role")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
