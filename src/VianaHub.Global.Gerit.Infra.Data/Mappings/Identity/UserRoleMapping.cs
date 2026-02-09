using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Identity;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Identity;

/// <summary>
/// Mapeamento da entidade UserRole
/// Relação usuário x role com suporte a Row Level Security
/// </summary>
public class UserRoleMapping : IEntityTypeConfiguration<UserRoleEntity>
{
    public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
    {
        builder.ToTable("UserRoles", "dbo");

        // Chave primária composta
        builder.HasKey(x => new { x.TenantId, x.UserId, x.RoleId })
            .HasName("PK_UserRoles");

        // Propriedades
        builder.Property(x => x.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnName("UserId")
            .IsRequired();

        builder.Property(x => x.RoleId)
            .HasColumnName("RoleId")
            .IsRequired();

        // Índices
        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("IX_UserRoles_UserId")
            .IncludeProperties(x => new { x.TenantId, x.RoleId });

        builder.HasIndex(x => x.RoleId)
            .HasDatabaseName("IX_UserRoles_RoleId")
            .IncludeProperties(x => new { x.TenantId, x.UserId });

        // Relacionamentos
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .HasConstraintName("FK_UserRoles_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

    }
}
