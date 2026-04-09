using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

public class VisitTeamEquipmentMapping : IEntityTypeConfiguration<VisitTeamEquipmentEntity>
{
    public void Configure(EntityTypeBuilder<VisitTeamEquipmentEntity> builder)
    {
        builder.ToTable("VisitTeamEquipments", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.VisitTeamId).IsRequired();
        builder.Property(x => x.EquipmentId).IsRequired();
        builder.Property(x => x.IsActive).HasColumnType("BIT").HasDefaultValue(true);
        builder.Property(x => x.IsDeleted).HasColumnType("BIT").HasDefaultValue(false);

        builder.Property(x => x.CreatedAt).HasColumnType("DATETIME2").HasDefaultValueSql("SYSDATETIME()");
        builder.Property(x => x.ModifiedAt).HasColumnType("DATETIME2");

        builder.HasIndex(x => new { x.TenantId, x.VisitTeamId, x.EquipmentId }).IsUnique();

        builder.HasOne(x => x.Equipment).WithMany().HasForeignKey("EquipmentId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.VisitTeam).WithMany().HasForeignKey("VisitTeamId").OnDelete(DeleteBehavior.Restrict);
    }
}
