using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

public class VisitTeamMapping : IEntityTypeConfiguration<VisitTeamEntity>
{
    public void Configure(EntityTypeBuilder<VisitTeamEntity> builder)
    {
        builder.ToTable("VisitTeams", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        // Chave alternativa para suportar FKs compostas com TenantId
        builder.HasAlternateKey(x => new { x.Id, x.TenantId })
            .HasName("UQ_VisitTeam_Id_Tenant");

        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.VisitId).IsRequired();
        builder.Property(x => x.TeamId).IsRequired();
        builder.Property(x => x.IsActive).HasColumnType("BIT").HasDefaultValue(true);
        builder.Property(x => x.IsDeleted).HasColumnType("BIT").HasDefaultValue(false);

        builder.Property(x => x.CreatedAt).HasColumnType("DATETIME2").HasDefaultValueSql("SYSDATETIME()");
        builder.Property(x => x.ModifiedAt).HasColumnType("DATETIME2");

        builder.HasIndex(x => new { x.TenantId, x.VisitId, x.TeamId }).IsUnique();

        builder.HasOne(x => x.Team).WithMany().HasForeignKey(x => x.TeamId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Visit).WithMany().HasForeignKey(x => x.VisitId).OnDelete(DeleteBehavior.Restrict);
    }
}
