using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

public class EmployeeTeamMapping : IEntityTypeConfiguration<EmployeeTeamEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeTeamEntity> builder)
    {
        builder.ToTable("EmployeeTeams", "dbo");

        builder.HasKey(x => x.Id).HasName("PK_EmployeeTeams");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1,1)
            .IsRequired();

        builder.Property(x => x.TenantId)
            .HasColumnType("INT")
            .IsRequired();
        builder.Property(x => x.TeamId)
            .HasColumnType("INT")
            .IsRequired();
        builder.Property(x => x.EmployeeId)
            .HasColumnType("INT")
            .IsRequired();

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
            .HasConstraintName("FK_EmployeeAddresses_Tenant")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Team)
        .WithMany()
        .HasForeignKey(x => x.TeamId)
        .HasConstraintName("FK_EmployeeTeams_Team")
        .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .HasConstraintName("FK_EmployeeTeams_Employee")
            .OnDelete(DeleteBehavior.NoAction);
    }
}
