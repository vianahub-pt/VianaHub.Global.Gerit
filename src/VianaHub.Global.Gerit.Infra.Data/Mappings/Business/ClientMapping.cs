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

        // Chave Prim�ria
        builder.HasKey(x => x.Id)
            .HasName("PK_Clients");

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Chave alternativa para suportar FKs compostas com TenantId
        builder.HasAlternateKey(x => new { x.Id, x.TenantId })
            .HasName("UQ_Clients_Id_Tenant");

        // Propriedades
        builder.Property(x => x.TenantId)
            .HasColumnName("TenantId")
            .HasColumnType("INT")
            .IsRequired(true);

        builder.Property(x => x.ClientType)
            .HasColumnName("ClientType")
            .HasColumnType("INT")
            .IsRequired(true);

        builder.Property(x => x.Origin)
            .HasColumnName("Origin")
            .HasColumnType("INT")
            .IsRequired(true);

        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR(150)")
            .HasMaxLength(150)
            .IsRequired(true);

        builder.Property(x => x.Phone)
            .HasColumnName("Phone")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired(true);

        builder.Property(x => x.Email)
            .HasColumnName("Email")
            .HasColumnType("NVARCHAR(255)")
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(x => x.Website)
            .HasColumnName("Website")
            .HasColumnType("NVARCHAR(255)")
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(x => x.UrlImage)
            .HasColumnName("UrlImage")
            .HasColumnType("NVARCHAR(255)")
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(x => x.Score)
            .HasColumnName("Score")
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(x => x.Consent)
            .HasColumnName("Consent")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired(true);

        builder.Property(x => x.ConsentDate)
            .HasColumnName("ConsentDate")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired(true);

        builder.Property(x => x.RevokedConsentDate)
            .HasColumnName("RevokedConsentDate")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired(false);

        builder.Property(x => x.Remarks)
            .HasColumnName("Remarks")
            .HasColumnType("NVARCHAR(500)")
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(x => x.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired(true);

        builder.Property(x => x.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired(true);

        builder.Property(x => x.CreatedBy)
              .HasColumnName("CreatedBy")
              .HasColumnType("INT")
              .IsRequired(true);

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired(true);

        builder.Property(x => x.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(x => x.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Constraints �nicos
        builder.HasIndex(v => new { v.TenantId, v.Name })
            .IsUnique()
            .HasDatabaseName("UQ_Clients_Tenant_Name");

        // �ndices �nicos com filtro
        builder.HasIndex(v => new { v.TenantId, v.Name })
            .IsUnique()
            .HasDatabaseName("UX_Clients_Tenant_Name_Active")
            .HasFilter("[IsDeleted] = 0");

        // �ndices n�o clusterizados
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

        builder.HasOne(x => x.Individual)
            .WithOne(ci => ci.Client)
            .HasForeignKey<ClientIndividualEntity>(ci => new { ci.ClientId, ci.TenantId })
            .HasPrincipalKey<ClientEntity>(c => new { c.Id, c.TenantId })
            .HasConstraintName("FK_ClientIndividuals_Client")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Company)
            .WithOne(cc => cc.Client)
            .HasForeignKey<ClientCompanyEntity>(cc => new { cc.ClientId, cc.TenantId })
            .HasPrincipalKey<ClientEntity>(c => new { c.Id, c.TenantId })
            .HasConstraintName("FK_ClientCompanies_Client")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Consents)
            .WithOne(cc => cc.Client)
            .HasForeignKey(cc => new { cc.ClientId, cc.TenantId })
            .HasPrincipalKey(c => new { c.Id, c.TenantId })
            .HasConstraintName("FK_ClientConsents_Client")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.ChildHierarchies)
            .WithOne(h => h.ParentClient)
            .HasForeignKey(h => new { h.ParentClientId, h.TenantId })
            .HasPrincipalKey(c => new { c.Id, c.TenantId })
            .HasConstraintName("FK_ClientHierarchies_ParentClient")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.ParentHierarchies)
            .WithOne(h => h.ChildClient)
            .HasForeignKey(h => new { h.ChildClientId, h.TenantId })
            .HasPrincipalKey(c => new { c.Id, c.TenantId })
            .HasConstraintName("FK_ClientHierarchies_ChildClient")
            .OnDelete(DeleteBehavior.Restrict);
    }
}

