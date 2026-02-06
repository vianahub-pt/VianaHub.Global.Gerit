using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

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
        builder.HasKey(x => x.Id)
            .HasName("PK_Clients");

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
            .HasColumnType("NVARCHAR(255)")
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(x => x.Phone)
            .HasColumnName("Phone")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Consent)
            .HasColumnName("Consent")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
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
        builder.HasIndex(v => new { v.TenantId, v.Name })
            .IsUnique()
            .HasDatabaseName("UQ_Clients_Tenant_Name");

        // Índices únicos com filtro
        builder.HasIndex(v => new { v.TenantId, v.Name })
            .IsUnique()
            .HasDatabaseName("UX_Clients_Tenant_Name_Active")
            .HasFilter("[IsDeleted] = 0");

        // Índices não clusterizados
        builder.HasIndex(v => new { v.TenantId, v.Name })
            .HasDatabaseName("IX_Clients_Tenant_Active")
            .IncludeProperties(v => new { v.Email, v.Phone })
            .HasFilter("[IsDeleted] = 0");

        // Relacionamentos
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .HasConstraintName("FK_Clients_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Contacts)
            .WithOne(cc => cc.Client)
            .HasForeignKey(cc => cc.ClientId)
            .HasConstraintName("FK_ClientContacts_Client")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Addresses)
            .WithOne(ca => ca.Client)
            .HasForeignKey(ca => ca.ClientId)
            .HasConstraintName("FK_ClientAddresses_Client")
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento FiscalData configurado no ClientFiscalDataMapping
    }
}
