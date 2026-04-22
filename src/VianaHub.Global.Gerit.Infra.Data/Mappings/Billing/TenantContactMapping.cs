using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Billing;

/// <summary>
/// Mapeamento da entidade TenantContact
/// Contatos do tenant com suporte a Row Level Security
/// </summary>
public class TenantContactMapping : IEntityTypeConfiguration<TenantContactEntity>
{
    public void Configure(EntityTypeBuilder<TenantContactEntity> builder)
    {
        builder.ToTable("TenantContacts", "dbo");

        // Chave Primária
        builder.HasKey(x => x.Id)
            .HasName("PK_TenantContacts");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.TenantId)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnType("NVARCHAR(150)")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnType("NVARCHAR(255)")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Phone)
            .HasColumnType("NVARCHAR(30)")
            .HasMaxLength(30)
            .IsRequired(false);

        builder.Property(x => x.IsPrimary)
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();
        builder.Property(x => x.CreatedBy)
              .HasColumnType("INT")
              .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(x => x.ModifiedBy)
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(x => x.ModifiedAt)
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Índices
        builder.HasIndex(x => x.TenantId)
            .HasDatabaseName("IX_TenantContacts_TenantId")
            .HasFilter("[IsDeleted] = 0");

        // Relacionamentos configurados no TenantMapping
    }
}
