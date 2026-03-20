using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Identity;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Identity;

public class UserPreferencesMapping : IEntityTypeConfiguration<UserPreferencesEntity>
{
    public void Configure(EntityTypeBuilder<UserPreferencesEntity> builder)
    {
        builder.ToTable("UserPreferences", "dbo");

        builder.HasKey(x => x.Id).HasName("PK_UserPreferences");

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        builder.HasAlternateKey(x => new { x.Id, x.TenantId }).HasName("UQ_UserPreferences_Id_Tenant");

        builder.Property(x => x.TenantId).HasColumnName("TenantId").IsRequired();
        builder.Property(x => x.UserId).HasColumnName("UserId").IsRequired();

        builder.Property(x => x.Appearance)
            .HasColumnName("Appearance")
            .HasColumnType("NVARCHAR(10)")
            .HasMaxLength(10)
            .HasDefaultValue("light")
            .IsRequired();

        builder.Property(x => x.CurrencyCode)
            .HasColumnName("CurrencyCode")
            .HasColumnType("NVARCHAR(3)")
            .HasMaxLength(3)
            .HasDefaultValue("EUR")
            .IsRequired();

        builder.Property(x => x.Locale)
            .HasColumnName("Locale")
            .HasColumnType("NVARCHAR(10)")
            .HasMaxLength(10)
            .HasDefaultValue("pt-PT")
            .IsRequired();

        builder.Property(x => x.Timezone)
            .HasColumnName("Timezone")
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .HasDefaultValue("Europe/Lisbon")
            .IsRequired();

        builder.Property(x => x.DateFormat)
            .HasColumnName("DateFormat")
            .HasColumnType("NVARCHAR(20)")
            .HasMaxLength(20)
            .HasDefaultValue("DD-MM-YYYY")
            .IsRequired();

        builder.Property(x => x.TimeFormat)
            .HasColumnName("TimeFormat")
            .HasColumnType("NVARCHAR(10)")
            .HasMaxLength(10)
            .HasDefaultValue("24h")
            .IsRequired();

        builder.Property(x => x.DayStart)
            .HasColumnName("DayStart")
            .HasColumnType("TIME(0)")
            .HasDefaultValueSql("('09:00')")
            .IsRequired();

        builder.Property(x => x.DayEnd)
            .HasColumnName("DayEnd")
            .HasColumnType("TIME(0)")
            .HasDefaultValueSql("('18:00')")
            .IsRequired();

        builder.Property(x => x.EmailNewsletter).HasColumnName("EmailNewsletter").HasColumnType("BIT").HasDefaultValue(false).IsRequired();
        builder.Property(x => x.EmailWeeklyReport).HasColumnName("EmailWeeklyReport").HasColumnType("BIT").HasDefaultValue(false).IsRequired();
        builder.Property(x => x.EmailApproval).HasColumnName("EmailApproval").HasColumnType("BIT").HasDefaultValue(false).IsRequired();
        builder.Property(x => x.EmailAlerts).HasColumnName("EmailAlerts").HasColumnType("BIT").HasDefaultValue(true).IsRequired();
        builder.Property(x => x.EmailReminders).HasColumnName("EmailReminders").HasColumnType("BIT").HasDefaultValue(true).IsRequired();
        builder.Property(x => x.EmailPlanner).HasColumnName("EmailPlanner").HasColumnType("BIT").HasDefaultValue(true).IsRequired();

        builder.Property(x => x.IsActive).HasColumnName("IsActive").HasColumnType("BIT").HasDefaultValue(true).IsRequired();
        builder.Property(x => x.IsDeleted).HasColumnName("IsDeleted").HasColumnType("BIT").HasDefaultValue(false).IsRequired();

        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").HasColumnType("INT").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("CreatedAt").HasColumnType("DATETIME2(7)").HasDefaultValueSql("SYSDATETIME()").IsRequired();
        builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy").HasColumnType("INT").IsRequired(false);
        builder.Property(x => x.ModifiedAt).HasColumnName("ModifiedAt").HasColumnType("DATETIME2(7)").IsRequired(false);

        // Check constraints to match SQL table
        builder.HasCheckConstraint("CK_UserPreferences_Active_Deleted", "NOT (IsActive = 1 AND IsDeleted = 1)");
        builder.HasCheckConstraint("CK_UserPreferences_Appearance", "Appearance IN ('light', 'dark')");
        builder.HasCheckConstraint("CK_UserPreferences_Locale", "Locale IN ('pt-PT', 'en-US')");
        builder.HasCheckConstraint("CK_UserPreferences_TimeFormat", "TimeFormat IN ('24h', '12h')");

        // Relationships
        // Composite FK (UserId, TenantId) targeting Users(Id, TenantId) as defined in SQL
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => new { x.UserId, x.TenantId })
            .HasPrincipalKey(u => new { u.Id, u.TenantId })
            .HasConstraintName("FK_UserPreferences_User")
            .OnDelete(DeleteBehavior.Restrict);

        // FK to Tenant (use explicit navigation to avoid EF Core creating shadow property)
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .HasConstraintName("FK_UserPreferences_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        // Unique index TenantId + UserId active (match SQL index name and filter)
        builder.HasIndex(x => new { x.TenantId, x.UserId })
            .HasDatabaseName("UX_UserPreferences_Tenant_User_Active")
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");
    }
}
