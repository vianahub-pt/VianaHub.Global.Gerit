using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

public class VisitTeamEquipmentMapping : IEntityTypeConfiguration<VisitTeamEquipmentEntity>
{
    public void Configure(EntityTypeBuilder<VisitTeamEquipmentEntity> builder)
    {
        builder.ToTable("VisitTeamEquipment", "dbo");
        builder.HasKey(x => x.Id).HasName("PK_VisitTeamEquipment");
        builder.Property(x => x.Id).UseIdentityColumn(1, 1).IsRequired();

        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.VisitTeamId).IsRequired();
        builder.Property(x => x.EquipmentId).IsRequired();
        builder.Property(x => x.IsActive).HasColumnType("BIT").HasDefaultValue(true);
        builder.Property(x => x.IsDeleted).HasColumnType("BIT").HasDefaultValue(false);

        builder.Property(x => x.CreatedBy).IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnType("DATETIME2(7)").HasDefaultValueSql("SYSUTCDATETIME()");
        builder.Property(x => x.ModifiedBy).IsRequired(false);
        builder.Property(x => x.ModifiedAt).HasColumnType("DATETIME2(7)");

        // Chave alternativa para suportar FKs compostas com TenantId
        builder.HasAlternateKey(x => new { x.Id, x.TenantId })
            .HasName("UQ_VisitTeamEquipment_Id_Tenant");

        // Unico: nao pode duplicar equipamento no mesmo visit team
        builder.HasIndex(x => new { x.TenantId, x.VisitTeamId, x.EquipmentId })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0")
            .HasDatabaseName("UX_VisitTeamEquipment_Unique");

        // Indice nao clusterizado: busca por VisitTeam
        builder.HasIndex(x => new { x.TenantId, x.VisitTeamId })
            .HasDatabaseName("IX_VisitTeamEquipment_VisitTeamId")
            .HasFilter("[IsDeleted] = 0");

        // Relacionamentos
        builder.HasOne(x => x.Equipment)
            .WithMany()
            .HasForeignKey(x => new { x.EquipmentId, x.TenantId })
            .HasPrincipalKey(x => new { x.Id, x.TenantId })
            .HasConstraintName("FK_VisitTeamEquipment_Equipment")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.VisitTeam)
            .WithMany()
            .HasForeignKey(x => new { x.VisitTeamId, x.TenantId })
            .HasPrincipalKey(x => new { x.Id, x.TenantId })
            .HasConstraintName("FK_VisitTeamEquipment_VisitTeam")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
