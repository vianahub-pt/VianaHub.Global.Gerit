using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

public class InterventionTeamMapping : IEntityTypeConfiguration<InterventionTeamEntity>
{
    public void Configure(EntityTypeBuilder<InterventionTeamEntity> builder)
    {
        builder.ToTable("InterventionTeams", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.InterventionId).IsRequired();
        builder.Property(x => x.TeamId).IsRequired();
        builder.Property(x => x.IsActive).HasColumnType("BIT").HasDefaultValue(true);
        builder.Property(x => x.IsDeleted).HasColumnType("BIT").HasDefaultValue(false);

        builder.Property(x => x.CreatedAt).HasColumnType("DATETIME2").HasDefaultValueSql("SYSDATETIME()");
        builder.Property(x => x.ModifiedAt).HasColumnType("DATETIME2");

        builder.HasIndex(x => new { x.TenantId, x.InterventionId, x.TeamId }).IsUnique();

        builder.HasOne(x => x.Team).WithMany().HasForeignKey("TeamId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Intervention).WithMany().HasForeignKey("InterventionId").OnDelete(DeleteBehavior.Restrict);
    }
}
