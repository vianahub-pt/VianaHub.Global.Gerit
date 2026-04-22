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
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Chave alternativa para suportar FKs compostas com TenantId
        builder.HasAlternateKey(x => new { x.Id, x.TenantId })
            .HasName("UQ_Clients_Id_Tenant");

        // Propriedades
        builder.Property(x => x.TenantId)
            .HasColumnType("INT")
            .IsRequired(true);

        builder.Property(x => x.ClientType)
            .HasColumnType("INT")
            .IsRequired(true);

        builder.Property(x => x.OriginType)
            .HasColumnType("INT")
            .IsRequired(true);

        builder.Property(x => x.UrlImage)
            .HasColumnType("NVARCHAR(500)")
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(x => x.Note)
            .HasColumnType("NVARCHAR(500)")
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnType("DATETIME2(7)")
            .IsRequired();

        builder.Property(x => x.ModifiedBy)
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(x => x.ModifiedAt)
            .HasColumnType("DATETIME2(7)")
            .IsRequired(false);

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
            .HasForeignKey(h => new { h.ParentId, h.TenantId })
            .HasPrincipalKey(c => new { c.Id, c.TenantId })
            .HasConstraintName("FK_ClientHierarchies_ParentClient")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.ParentHierarchies)
            .WithOne(h => h.ChildClient)
            .HasForeignKey(h => new { h.ChildId, h.TenantId })
            .HasPrincipalKey(c => new { c.Id, c.TenantId })
            .HasConstraintName("FK_ClientHierarchies_ChildClient")
            .OnDelete(DeleteBehavior.Restrict);
    }
}

