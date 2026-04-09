using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Billing;

public class PlanEntityMapping : IEntityTypeConfiguration<PlanEntity>
{
    public void Configure(EntityTypeBuilder<PlanEntity> builder)
    {
        builder.ToTable("Plans", "dbo");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("Description")
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(x => x.PricePerHour)
            .HasColumnName("PricePerHour")
            .HasColumnType("decimal(10,2)")
            .IsRequired(false);

        builder.Property(x => x.PricePerDay)
            .HasColumnName("PricePerDay")
            .HasColumnType("decimal(10,2)")
            .IsRequired(false);

        builder.Property(x => x.PricePerMonth)
            .HasColumnName("PricePerMonth")
            .HasColumnType("decimal(10,2)")
            .IsRequired(false);

        builder.Property(x => x.PricePerYear)
            .HasColumnName("PricePerYear")
            .HasColumnType("decimal(10,2)")
            .IsRequired(false);

        builder.Property(x => x.Currency)
            .HasColumnName("Currency")
            .HasMaxLength(3)
            .IsRequired()
            .HasDefaultValue("USD");

        builder.Property(x => x.MaxUsers)
            .HasColumnName("MaxUsers")
            .IsRequired();

        builder.Property(x => x.MaxPhotosPerVisits)
            .HasColumnName("MaxPhotosPerVisits")
            .IsRequired();

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
              .HasColumnType("INT")
              .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(x => x.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(x => x.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Constraint: Se IsDeleted = 1, então IsActive = 0
        builder.HasCheckConstraint("CK_Plans_DeletedImpliesInactive", "[IsDeleted] = 0 OR [IsActive] = 0");

        // Navegação
        builder.HasMany(x => x.Subscriptions)
            .WithOne()
            .HasForeignKey("PlanId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
