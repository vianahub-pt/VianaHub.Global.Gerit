using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

public class VisitTeamEmployeeMapping : IEntityTypeConfiguration<VisitTeamEmployeeEntity>
{
    public void Configure(EntityTypeBuilder<VisitTeamEmployeeEntity> builder)
    {
        builder.ToTable("VisitTeamEmployee");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .IsRequired();

        builder.Property(x => x.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(x => x.VisitTeamId)
            .HasColumnName("VisitTeamId")
            .IsRequired();

        builder.Property(x => x.EmployeeId)
            .HasColumnName("EmployeeId")
            .IsRequired();

        builder.Property(x => x.FunctionId)
            .HasColumnName("FunctionId")
            .IsRequired();

        builder.Property(x => x.IsLeader)
            .HasColumnName("IsLeader")
            .IsRequired();

        builder.Property(x => x.StartDateTime)
            .HasColumnName("StartDateTime")
            .HasColumnType("DATETIME2")
            .IsRequired();

        builder.Property(x => x.EndDateTime)
            .HasColumnName("EndDateTime")
            .HasColumnType("DATETIME2");

        builder.Property(x => x.IsActive)
            .HasColumnName("IsActive")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.IsDeleted)
            .HasColumnName("IsDeleted")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.CreatedBy)
            .HasColumnName("CreatedBy")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2(7)")
            .IsRequired();

        builder.Property(x => x.ModifiedBy)
            .HasColumnName("ModifiedBy");

        builder.Property(x => x.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2(7)");

        // Relacionamentos
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.VisitTeam)
            .WithMany()
            .HasForeignKey(x => new { x.VisitTeamId, x.TenantId })
            .HasPrincipalKey(x => new { x.Id, x.TenantId })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => new { x.EmployeeId, x.TenantId })
            .HasPrincipalKey(x => new { x.Id, x.TenantId })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Function)
            .WithMany()
            .HasForeignKey(x => new { x.FunctionId, x.TenantId })
            .HasPrincipalKey(x => new { x.Id, x.TenantId })
            .OnDelete(DeleteBehavior.Restrict);

        // Índices
        builder.HasIndex(x => new { x.Id, x.TenantId })
            .IsUnique()
            .HasDatabaseName("UQ_VisitTeamEmployee_Id_Tenant");

        builder.HasIndex(x => new { x.TenantId, x.VisitTeamId, x.EmployeeId })
            .HasFilter("[EndDateTime] IS NULL AND [IsDeleted] = 0")
            .IsUnique()
            .HasDatabaseName("UX_VisitTeamEmployee_Active");

        builder.HasIndex(x => new { x.TenantId, x.VisitTeamId })
            .HasFilter("[IsDeleted] = 0")
            .HasDatabaseName("IX_VisitTeamEmployee_VisitTeamId");

        builder.HasIndex(x => new { x.TenantId, x.EmployeeId })
            .HasFilter("[IsDeleted] = 0")
            .HasDatabaseName("IX_VisitTeamEmployee_EmployeeId");

        // Constraints
        builder.HasCheckConstraint("CK_VisitTeamEmployee_Active_Deleted", 
            "NOT ([IsActive] = 1 AND [IsDeleted] = 1)");

        builder.HasCheckConstraint("CK_VisitTeamEmployee_EndDateTime", 
            "[EndDateTime] IS NULL OR [EndDateTime] >= [StartDateTime]");
    }
}
