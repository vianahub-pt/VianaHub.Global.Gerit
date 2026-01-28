using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Identity;

/// <summary>
/// Mapeamento da entidade User
/// Usuários do sistema com suporte a Row Level Security
/// </summary>
public class UserMapping : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users", "dbo");

        // Chave Primária
        builder.HasKey(u => u.Id)
            .HasName("PK_Users");

        builder.Property(u => u.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(u => u.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(u => u.Name)
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR(150)")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasColumnName("Email")
            .HasColumnType("NVARCHAR(256)")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(u => u.NormalizedEmail)
            .HasColumnName("NormalizedEmail")
            .HasColumnType("NVARCHAR(256)")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(u => u.EmailConfirmed)
            .HasColumnName("EmailConfirmed")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(u => u.PhoneNumber)
            .HasColumnName("PhoneNumber")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(u => u.PhoneNumberConfirmed)
            .HasColumnName("PhoneNumberConfirmed")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(u => u.LastAccessAt)
            .HasColumnName("LastAccessAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        builder.Property(u => u.PasswordHash)
            .HasColumnName("PasswordHash")
            .HasColumnType("NVARCHAR(500)")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(u => u.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(u => u.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(u => u.CreatedBy)
            .HasColumnName("CreatedBy")
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(u => u.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .IsRequired(false);

        builder.Property(u => u.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Constraints únicos
        builder.HasIndex(u => new { u.TenantId, u.Email })
            .IsUnique()
            .HasDatabaseName("UQ_Users_Tenant_Email");

        // Índices
        builder.HasIndex(u => new { u.TenantId, u.Email })
            .HasDatabaseName("IX_Users_Login")
            .IncludeProperties(u => new { u.Id, u.IsActive })
            .HasFilter("[IsDeleted] = 0");

        // Relacionamentos
        // NOTA: O relacionamento User -> Tenant já está configurado no TenantMapping.cs
        // através de HasMany(t => t.Users).WithOne(u => u.Tenant).HasForeignKey(u => u.TenantId)
        // Não configurar novamente aqui para evitar propriedades sombra (shadow properties)

        builder.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId)
            .HasConstraintName("FK_UserRoles_User")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
