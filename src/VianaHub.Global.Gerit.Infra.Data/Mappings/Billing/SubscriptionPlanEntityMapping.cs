using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Billing;

public class SubscriptionPlanEntityMapping : IEntityTypeConfiguration<SubscriptionPlanEntity>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlanEntity> builder)
    {
        builder.ToTable("SubscriptionPlans", "dbo");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(x => x.PricePerHour)
            .HasColumnType("decimal(10,2)")
            .IsRequired(false);

        builder.Property(x => x.PricePerDay)
            .HasColumnType("decimal(10,2)")
            .IsRequired(false);

        builder.Property(x => x.PricePerMonth)
            .HasColumnType("decimal(10,2)")
            .IsRequired(false);

        builder.Property(x => x.PricePerYear)
            .HasColumnType("decimal(10,2)")
            .IsRequired(false);

        builder.Property(x => x.Currency)
            .HasMaxLength(3)
            .IsRequired()
            .HasDefaultValue("USD");

        builder.Property(x => x.MaxUsers)
            .IsRequired();

        builder.Property(x => x.MaxPhotosPerVisit)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.CreatedBy)
              .HasColumnType("INT")
              .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(x => x.ModifiedBy)
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(x => x.ModifiedAt)
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Constraint: Se IsDeleted = 1, ent�o IsActive = 0
        builder.HasCheckConstraint("CK_SubscriptionPlans_DeletedImpliesInactive", "[IsDeleted] = 0 OR [IsActive] = 0");

        // Navega��o
        builder.HasMany(x => x.Subscriptions)
            .WithOne()
            .HasForeignKey("PlanId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
