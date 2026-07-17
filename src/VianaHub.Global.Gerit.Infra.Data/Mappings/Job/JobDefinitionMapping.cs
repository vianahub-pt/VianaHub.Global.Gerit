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
            .UseIdentityColumn(1, 1)
            .IsRequired();

        builder.Property(x => x.JobCategory)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(x => x.JobName)
            .HasMaxLength(150)
            .IsRequired();
        
        builder.Property(x => x.Description)
            .HasMaxLength(500);
        
        builder.Property(x => x.JobPurpose)
            .HasMaxLength(500);
        
        builder.Property(x => x.JobType)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.Property(x => x.JobMethod)
            .HasMaxLength(100);
        
        builder.Property(x => x.CronExpression).
            HasMaxLength(100);
        
        builder.Property(x => x.TimeZoneId)
            .HasColumnName("Timezone")
            .HasMaxLength(100);
        
        builder.Property(x => x.ExecuteOnlyOnce)
            .IsRequired();
        
        builder.Property(x => x.TimeoutMinutes)
            .IsRequired();
        
        builder.Property(x => x.Priority)
            .IsRequired();
        
        builder.Property(x => x.Queue)
            .HasMaxLength(50);
        
        builder.Property(x => x.MaxRetries)
            .IsRequired();
        
        builder.Property(x => x.JobConfiguration)
            .HasColumnType("NVARCHAR(MAX)");
        
        builder.Property(x => x.IsSystemJob)
            .IsRequired();
        
        builder.Property(x => x.HangfireJobId)
            .HasMaxLength(100);
        
        builder.Property(x => x.LastRegisteredAt);

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

        // Indexes
        builder.HasIndex(x => new { x.JobCategory, x.IsActive, x.IsDeleted }).HasDatabaseName("IX_JobDefinitions_Category_Active");
        builder.HasIndex(x => new { x.IsActive, x.IsSystemJob }).HasFilter("IsDeleted = 0").HasDatabaseName("IX_JobDefinitions_Active_System");
        builder.HasIndex(x => x.HangfireJobId).HasFilter("HangfireJobId IS NOT NULL").HasDatabaseName("IX_JobDefinitions_HangfireJobId");

        // Unique constraint
        builder.HasIndex(x => x.JobName).IsUnique().HasFilter("[IsDeleted] = 0").HasDatabaseName("UX_JobDefinitions_JobName");


    }
}
