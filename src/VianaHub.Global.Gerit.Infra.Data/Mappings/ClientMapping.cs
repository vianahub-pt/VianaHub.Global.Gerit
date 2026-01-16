using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings;

/// <summary>
/// Mapeamento da entidade Client
/// Clientes do tenant com suporte a Row Level Security
/// </summary>
public class ClientMapping : IEntityTypeConfiguration<ClientEntity>
{
    public void Configure(EntityTypeBuilder<ClientEntity> builder)
    {
        builder.ToTable("Clients", "dbo");

        // Chave Primária
        builder.HasKey(c => c.Id)
            .HasName("PK_Clients");

        builder.Property(c => c.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(c => c.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(c => c.Name)
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR(150)")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(c => c.Email)
            .HasColumnName("Email")
            .HasColumnType("NVARCHAR(255)")
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(c => c.Phone)
            .HasColumnName("Phone")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Consent)
            .HasColumnName("Consent")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(c => c.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(c => c.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(c => c.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(c => c.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(c => c.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Constraints únicos
        builder.HasIndex(c => new { c.TenantId, c.Name })
            .IsUnique()
            .HasDatabaseName("UQ_Clients_Tenant_Name");

        // Índices únicos com filtro
        builder.HasIndex(c => new { c.TenantId, c.Name })
            .IsUnique()
            .HasDatabaseName("UX_Clients_Tenant_Name_Active")
            .HasFilter("[IsDeleted] = 0");

        // Índices não clusterizados
        builder.HasIndex(c => new { c.TenantId, c.Name })
            .HasDatabaseName("IX_Clients_Tenant_Active")
            .IncludeProperties(c => new { c.Email, c.Phone })
            .HasFilter("[IsDeleted] = 0");

        // Relacionamentos
        builder.HasOne(c => c.Tenant)
            .WithMany()
            .HasForeignKey(c => c.TenantId)
            .HasConstraintName("FK_Clients_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Contacts)
            .WithOne(cc => cc.Client)
            .HasForeignKey(cc => cc.ClientId)
            .HasConstraintName("FK_ClientContacts_Client")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Addresses)
            .WithOne(ca => ca.Client)
            .HasForeignKey(ca => ca.ClientId)
            .HasConstraintName("FK_ClientAddresses_Client")
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento FiscalData configurado no ClientFiscalDataMapping
    }
}
