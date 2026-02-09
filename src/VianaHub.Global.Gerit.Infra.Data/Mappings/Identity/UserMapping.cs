using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Identity;

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
        builder.HasKey(x => x.Id)
            .HasName("PK_Users");

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR(150)")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnName("Email")
            .HasColumnType("NVARCHAR(256)")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.NormalizedEmail)
            .HasColumnName("NormalizedEmail")
            .HasColumnType("NVARCHAR(256)")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.EmailConfirmed)
            .HasColumnName("EmailConfirmed")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasColumnName("PhoneNumber")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(x => x.PhoneNumberConfirmed)
            .HasColumnName("PhoneNumberConfirmed")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.LastAccessAt)
            .HasColumnName("LastAccessAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        builder.Property(x => x.PasswordHash)
            .HasColumnName("PasswordHash")
            .HasColumnType("NVARCHAR(500)")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
             .HasColumnName("CreatedBy")
             .HasColumnType("INT")
             .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(x => x.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(x => x.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Constraints únicos
        builder.HasIndex(x => new { x.TenantId, x.Email })
            .IsUnique()
            .HasDatabaseName("UQ_Users_Tenant_Email");

        builder.HasIndex(x => new { x.TenantId, x.NormalizedEmail })
            .IsUnique()
            .HasDatabaseName("UQ_Users_Tenant_NormalizedEmail");

        // Índices
        builder.HasIndex(x => new { x.TenantId, x.Email })
            .HasDatabaseName("IX_Users_Login")
            .IncludeProperties(x => new { x.Id, x.IsActive })
            .HasFilter("[IsDeleted] = 0");

        // Relacionamentos
        // NOTA: O relacionamento User -> Tenant já está configurado no TenantMapping.cs
        // através de HasMany(t => t.Users).WithOne(x => x.Tenant).HasForeignKey(x => x.TenantId)
        // Não configurar novamente aqui para evitar propriedades sombra (shadow properties)

        builder.HasMany(x => x.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId)
            .HasConstraintName("FK_UserRoles_User")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
