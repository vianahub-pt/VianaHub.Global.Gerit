using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade Intervention
/// Intervenções do tenant com suporte a Row Level Security
/// </summary>
public class InterventionMapping : IEntityTypeConfiguration<InterventionEntity>
{
    public void Configure(EntityTypeBuilder<InterventionEntity> builder)
    {
        builder.ToTable("Interventions", "dbo");

        // Chave Primária
        builder.HasKey(i => i.Id)
            .HasName("PK_Interventions");

        builder.Property(i => i.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(i => i.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(i => i.ClientId)
            .HasColumnName("ClientId")
            .IsRequired();

        builder.Property(i => i.TeamMemberId)
            .HasColumnName("TeamMemberId")
            .IsRequired();

        builder.Property(i => i.VehicleId)
            .HasColumnName("VehicleId")
            .IsRequired();

        builder.Property(i => i.Title)
            .HasColumnName("Title")
            .HasColumnType("NVARCHAR(200)")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(i => i.Description)
            .HasColumnName("Description")
            .HasColumnType("NVARCHAR(2000)")
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(i => i.StartDateTime)
            .HasColumnName("StartDateTime")
            .HasColumnType("DATETIME2")
            .IsRequired();

        builder.Property(i => i.EndDateTime)
            .HasColumnName("EndDateTime")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        builder.Property(i => i.EstimatedValue)
            .HasColumnName("EstimatedValue")
            .HasColumnType("DECIMAL(10,2)")
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(i => i.RealValue)
            .HasColumnName("RealValue")
            .HasColumnType("DECIMAL(10,2)")
            .HasPrecision(10, 2)
            .IsRequired(false);

        builder.Property(i => i.Status)
            .HasColumnName("Status")
            .HasColumnType("TINYINT")
            .IsRequired();

        builder.Property(i => i.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(i => i.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(i => i.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(i => i.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(i => i.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(i => i.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Constraints de check
        builder.ToTable(t => t.HasCheckConstraint("CK_Interventions_EndDateTime", "[EndDateTime] IS NULL OR [EndDateTime] >= [StartDateTime]"));
        builder.ToTable(t => t.HasCheckConstraint("CK_EstimatedValue", "[EstimatedValue] >= 0"));

        // Índices
        builder.HasIndex(i => new { i.TenantId, i.StartDateTime })
            .HasDatabaseName("IX_Interventions_Tenant_Date")
            .IncludeProperties(i => new { i.ClientId, i.TeamMemberId, i.Status })
            .HasFilter("[IsDeleted] = 0");

        // Relacionamentos
        builder.HasOne(i => i.Tenant)
            .WithMany()
            .HasForeignKey(i => i.TenantId)
            .HasConstraintName("FK_Interventions_Tenants")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Client)
            .WithMany()
            .HasForeignKey(i => i.ClientId)
            .HasConstraintName("FK_Interventions_Clients")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.TeamMember)
            .WithMany()
            .HasForeignKey(i => i.TeamMemberId)
            .HasConstraintName("FK_Interventions_TeamMembers")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(i => i.Contacts)
            .WithOne(ic => ic.Intervention)
            .HasForeignKey(ic => ic.InterventionId)
            .HasConstraintName("FK_InterventionContacts_Intervention")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(i => i.Addresses)
            .WithOne(ia => ia.Intervention)
            .HasForeignKey(ia => ia.InterventionId)
            .HasConstraintName("FK_InterventionAddresses_Intervention")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
