using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Job;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Job;

public class JobDefinitionMapping : IEntityTypeConfiguration<JobDefinitionEntity>
{
    public void Configure(EntityTypeBuilder<JobDefinitionEntity> builder)
    {
        // Map to dbo schema - the database currently uses dbo.JobDefinitions
        builder.ToTable("JobDefinitions", "dbo");

        // Primary key and identity
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        builder.Property(x => x.JobCategory).HasMaxLength(100).IsRequired();
        builder.Property(x => x.JobName).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.JobPurpose).HasMaxLength(1000);
        builder.Property(x => x.JobType).HasMaxLength(200).IsRequired();
        builder.Property(x => x.JobMethod).HasMaxLength(100);
        builder.Property(x => x.CronExpression).HasMaxLength(200);
        builder.Property(x => x.TimeZoneId).HasMaxLength(150);
        builder.Property(x => x.ExecuteOnlyOnce).IsRequired();
        builder.Property(x => x.TimeoutMinutes).IsRequired();
        builder.Property(x => x.Priority).IsRequired();
        builder.Property(x => x.Queue).HasMaxLength(100);
        builder.Property(x => x.MaxRetries).IsRequired();
        builder.Property(x => x.JobConfiguration).HasColumnType("text");
        builder.Property(x => x.IsSystemJob).IsRequired();
        builder.Property(x => x.HangfireJobId).HasMaxLength(200);
        builder.Property(x => x.LastRegisteredAt);

        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.IsDeleted).IsRequired();

        // Indexes
        builder.HasIndex(x => new { x.JobCategory, x.IsActive, x.IsDeleted }).HasDatabaseName("IX_Services_Category_Active");
        builder.HasIndex(x => new { x.IsActive, x.IsSystemJob }).HasFilter("IsDeleted = 0").HasDatabaseName("IX_Services_Active_SYSTEM");
        builder.HasIndex(x => x.HangfireJobId).HasFilter("HangfireJobId IS NOT NULL").HasDatabaseName("IX_Services_HangfireJobId");

        // Unique constraint
        builder.HasIndex(x => x.JobName).IsUnique().HasDatabaseName("UQ_Job_JobName");

        // Audit
        builder.Property(x => x.CreatedBy).IsRequired().HasColumnName("CreatedBy");
        builder.Property(x => x.CreatedAt).IsRequired().HasColumnName("CreatedAt");
        builder.Property(x => x.ModifiedBy).HasColumnName("UpdatedBy");
        builder.Property(x => x.ModifiedAt).HasColumnName("UpdatedAt");
    }
}
