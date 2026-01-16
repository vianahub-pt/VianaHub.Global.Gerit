using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings;

/// <summary>
/// Mapeamento da entidade ClientContact
/// Contatos do cliente com suporte a Row Level Security
/// </summary>
public class ClientContactMapping : IEntityTypeConfiguration<ClientContactEntity>
{
    public void Configure(EntityTypeBuilder<ClientContactEntity> builder)
    {
        builder.ToTable("ClientContacts", "dbo");

        // Chave Primária
        builder.HasKey(cc => cc.Id)
            .HasName("PK_ClientContacts");

        builder.Property(cc => cc.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(cc => cc.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(cc => cc.ClientId)
            .HasColumnName("ClientId")
            .IsRequired();

        builder.Property(cc => cc.Name)
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR(150)")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(cc => cc.Email)
            .HasColumnName("Email")
            .HasColumnType("NVARCHAR(255)")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(cc => cc.Phone)
            .HasColumnName("Phone")
            .HasColumnType("NVARCHAR(30)")
            .HasMaxLength(30)
            .IsRequired(false);

        builder.Property(cc => cc.IsPrimary)
            .HasColumnName("IsPrimary")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(cc => cc.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(cc => cc.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(cc => cc.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(cc => cc.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(cc => cc.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(cc => cc.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Índices
        builder.HasIndex(cc => cc.ClientId)
            .HasDatabaseName("IX_ClientContacts_ClientId")
            .IncludeProperties(cc => cc.TenantId)
            .HasFilter("[IsDeleted] = 0");

        // Relacionamentos
        builder.HasOne(cc => cc.Tenant)
            .WithMany()
            .HasForeignKey(cc => cc.TenantId)
            .HasConstraintName("FK_ClientContacts_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento com Client configurado no ClientMapping
    }
}
