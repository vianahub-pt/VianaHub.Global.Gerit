using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade Visit
/// Intervenções do tenant com suporte a Row Level Security
/// </summary>
public class VisitMapping : IEntityTypeConfiguration<VisitEntity>
{
    public void Configure(EntityTypeBuilder<VisitEntity> builder)
    {
        builder.ToTable("Visits", "dbo");

        // Chave Primária
        builder.HasKey(x => x.Id)
            .HasName("PK_Visits");

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(x => x.ClientId)
            .HasColumnName("ClientId")
            .IsRequired();

        builder.Property(x => x.StatusId)
            .HasColumnName("StatusId")
            .IsRequired();

        builder.Property(x => x.Title)
            .HasColumnName("Title")
            .HasColumnType("NVARCHAR(200)")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("Description")
            .HasColumnType("NVARCHAR(2000)")
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(x => x.StartDateTime)
            .HasColumnName("StartDateTime")
            .HasColumnType("DATETIME2")
            .IsRequired();

        builder.Property(x => x.EndDateTime)
            .HasColumnName("EndDateTime")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        builder.Property(x => x.EstimatedValue)
            .HasColumnName("EstimatedValue")
            .HasColumnType("DECIMAL(10,2)")
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(x => x.RealValue)
            .HasColumnName("RealValue")
            .HasColumnType("DECIMAL(10,2)")
            .HasPrecision(10, 2)
            .IsRequired(false);

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

        // Constraints de check
        builder.ToTable(t => t.HasCheckConstraint("CK_Visits_EndDateTime", "[EndDateTime] IS NULL OR [EndDateTime] >= [StartDateTime]"));


        builder.HasIndex(v => new { v.Id, v.TenantId })
            .HasDatabaseName("UQ_Visits_Id_Tenant")
            .IsUnique();

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

        builder.HasOne(x => x.Status)
            .WithMany()
            .HasForeignKey(x => new { x.StatusId, x.TenantId })
            .HasPrincipalKey(s => new { s.Id, s.TenantId })
            .HasConstraintName("FK_Visits_Status")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Contacts)
            .WithOne(ic => ic.Visit)
            .HasForeignKey(ic => ic.VisitId)
            .HasConstraintName("FK_VisitContacts_Visit")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Addresses)
            .WithOne(ia => ia.Visit)
            .HasForeignKey(ia => ia.VisitId)
            .HasConstraintName("FK_VisitAddresses_Visit")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
