using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings;

/// <summary>
/// Mapeamento da entidade UserRole
/// Relação usuário x role com suporte a Row Level Security
/// </summary>
public class UserRoleMapping : IEntityTypeConfiguration<UserRoleEntity>
{
    public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
    {
        builder.ToTable("UserRoles", "dbo");

        // Chave Primária
        builder.HasKey(ur => ur.Id)
            .HasName("PK_UserRoles");

        builder.Property(ur => ur.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(ur => ur.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(ur => ur.UserId)
            .HasColumnName("UserId")
            .IsRequired();

        builder.Property(ur => ur.RoleId)
            .HasColumnName("RoleId")
            .IsRequired();

        // Constraints únicos
        builder.HasIndex(ur => new { ur.TenantId, ur.UserId, ur.RoleId })
            .IsUnique()
            .HasDatabaseName("UQ_UserRoles");

        // Índices
        builder.HasIndex(ur => ur.UserId)
            .HasDatabaseName("IX_UserRoles_UserId")
            .IncludeProperties(ur => new { ur.TenantId, ur.RoleId });

        builder.HasIndex(ur => ur.RoleId)
            .HasDatabaseName("IX_UserRoles_RoleId")
            .IncludeProperties(ur => new { ur.TenantId, ur.UserId });

        // Relacionamentos
        builder.HasOne(ur => ur.Tenant)
            .WithMany()
            .HasForeignKey(ur => ur.TenantId)
            .HasConstraintName("FK_UserRoles_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ur => ur.Role)
            .WithMany()
            .HasForeignKey(ur => ur.RoleId)
            .HasConstraintName("FK_UserRoles_Role")
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento com User configurado no UserMapping
    }
}
