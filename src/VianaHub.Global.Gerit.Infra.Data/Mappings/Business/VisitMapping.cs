using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade Visit
/// Interven��es do tenant com suporte a Row Level Security
/// </summary>
public class VisitMapping : IEntityTypeConfiguration<VisitEntity>
{
    public void Configure(EntityTypeBuilder<VisitEntity> builder)
    {
        builder.ToTable("Visits", "dbo");

        // Chave Prim�ria
        builder.HasKey(x => x.Id)
            .HasName("PK_Visits");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.TenantId)
            .IsRequired();

        builder.Property(x => x.ClientId)
            .IsRequired();

        builder.Property(x => x.StatusDefinitionId)
            .IsRequired();

        builder.Property(x => x.StatusDomainId)
            .IsRequired();

        builder.Property(x => x.CurrencyCode)
            .HasColumnType("CHAR(3)")
            .HasMaxLength(3)
            .HasDefaultValue("EUR")
            .IsRequired();

        builder.Property(x => x.Title)
            .HasColumnType("NVARCHAR(200)")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnType("NVARCHAR(2000)")
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(x => x.StartDateTime)
            .HasColumnType("DATETIME2")
            .IsRequired();

        builder.Property(x => x.EndDateTime)
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        builder.Property(x => x.EstimatedValue)
            .HasColumnType("DECIMAL(10,2)")
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(x => x.RealValue)
            .HasColumnType("DECIMAL(10,2)")
            .HasPrecision(10, 2)
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

        // Constraints de check
        builder.ToTable(t => t.HasCheckConstraint("CK_Visits_EndDateTime", "[EndDateTime] IS NULL OR [EndDateTime] >= [StartDateTime]"));


        // Chave alternativa para suportar FKs compostas com TenantId
        builder.HasAlternateKey(v => new { v.Id, v.TenantId })
            .HasName("UQ_Visits_Id_Tenant");

        // Relacionamentos
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .HasConstraintName("FK_Visits_Tenants")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Client)
            .WithMany()
            .HasForeignKey(x => new { x.ClientId, x.TenantId })
            .HasPrincipalKey(c => new { c.Id, c.TenantId })
            .HasConstraintName("FK_Visits_Clients")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.StatusDefinition)
            .WithMany()
            .HasForeignKey(x => new { x.StatusDefinitionId, x.TenantId, x.StatusDomainId })
            .HasPrincipalKey(s => new { s.Id, s.TenantId, s.StatusDomainId })
            .HasConstraintName("FK_Visits_StatusDefinitions")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Contacts)
            .WithOne(ic => ic.Visit)
            .HasForeignKey(ic => new { ic.VisitId, ic.TenantId })
            .HasPrincipalKey(x => new { x.Id, x.TenantId })
            .HasConstraintName("FK_VisitContacts_Visit")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Addresses)
            .WithOne(ia => ia.Visit)
            .HasForeignKey(ia => new { ia.VisitId, ia.TenantId })
            .HasPrincipalKey(x => new { x.Id, x.TenantId })
            .HasConstraintName("FK_VisitAddresses_Visit")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
