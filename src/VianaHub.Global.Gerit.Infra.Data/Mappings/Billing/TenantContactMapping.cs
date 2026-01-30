using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Billing;

/// <summary>
/// Mapeamento da entidade TenantContact
/// Contatos do tenant com suporte a Row Level Security
/// </summary>
public class TenantContactMapping : IEntityTypeConfiguration<TenantContact>
{
    public void Configure(EntityTypeBuilder<TenantContact> builder)
    {
        builder.ToTable("TenantContacts", "dbo");

        // Chave Primária
        builder.HasKey(tc => tc.Id)
            .HasName("PK_TenantContacts");

        builder.Property(tc => tc.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(tc => tc.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(tc => tc.Name)
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR(150)")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(tc => tc.Email)
            .HasColumnName("Email")
            .HasColumnType("NVARCHAR(255)")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(tc => tc.Phone)
            .HasColumnName("Phone")
            .HasColumnType("NVARCHAR(30)")
            .HasMaxLength(30)
            .IsRequired(false);

        builder.Property(tc => tc.IsPrimary)
            .HasColumnName("IsPrimary")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(tc => tc.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(tc => tc.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(tc => tc.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(tc => tc.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(tc => tc.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(tc => tc.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Índices
        builder.HasIndex(tc => tc.TenantId)
            .HasDatabaseName("IX_TenantContacts_TenantId")
            .HasFilter("[IsDeleted] = 0");

        // Relacionamentos configurados no TenantMapping
    }
}
