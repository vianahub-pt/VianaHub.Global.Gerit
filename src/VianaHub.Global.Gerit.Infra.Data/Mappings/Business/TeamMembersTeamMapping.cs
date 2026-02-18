using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

public class TeamMembersTeamMapping : IEntityTypeConfiguration<TeamMembersTeamEntity>
{
    public void Configure(EntityTypeBuilder<TeamMembersTeamEntity> builder)
    {
        builder.ToTable("TeamMembersTeams", "dbo");

        builder.HasKey(x => x.Id).HasName("PK_TeamMembersTeams");

        builder.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn(1,1).IsRequired();

        builder.Property(x => x.TenantId).HasColumnName("TenantId").HasColumnType("INT").IsRequired();
        builder.Property(x => x.TeamId).HasColumnName("TeamId").HasColumnType("INT").IsRequired();
        builder.Property(x => x.TeamMemberId).HasColumnName("TeamMemberId").HasColumnType("INT").IsRequired();

        builder.Property(x => x.IsActive).HasColumnName("IsActive").HasColumnType("BIT").HasDefaultValue(true).IsRequired();
        builder.Property(x => x.IsDeleted).HasColumnName("IsDeleted").HasColumnType("BIT").HasDefaultValue(false).IsRequired();

        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").HasColumnType("INT").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("CreatedAt").HasColumnType("DATETIME2").HasDefaultValueSql("SYSDATETIME()").IsRequired();
        builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy").HasColumnType("INT").IsRequired(false);
        builder.Property(x => x.ModifiedAt).HasColumnName("ModifiedAt").HasColumnType("DATETIME2").IsRequired(false);

        // Relacionamentos
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .HasConstraintName("FK_TeamMemberAddresses_Tenant")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Team)
        .WithMany()
        .HasForeignKey(x => x.TeamId)
        .HasConstraintName("FK_TeamMembersTeams_Team")
        .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.TeamMember)
            .WithMany()
            .HasForeignKey(x => x.TeamMemberId)
            .HasConstraintName("FK_TeamMembersTeams_TeamMember")
            .OnDelete(DeleteBehavior.NoAction);
    }
}
