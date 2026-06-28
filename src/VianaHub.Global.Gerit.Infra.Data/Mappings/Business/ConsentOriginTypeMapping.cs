using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

public class ConsentOriginTypeMapping : IEntityTypeConfiguration<ConsentOriginTypeEntity>
{
    public void Configure(EntityTypeBuilder<ConsentOriginTypeEntity> builder)
    {
        builder.ToTable("ConsentOriginTypes", "dbo");

        builder.HasKey(x => x.Id)
            .HasName("PK_ConsentOriginTypes");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        builder.Property(x => x.Code)
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .HasDatabaseName("UQ_ConsentOriginTypes_Code")
            .IsUnique();

        builder.Property(x => x.Name)
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .HasDatabaseName("UQ_ConsentOriginTypes_Name")
            .IsUnique();

        builder.Property(x => x.Description)
            .HasColumnType("NVARCHAR(300)")
            .HasMaxLength(300)
            .IsRequired(false);

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
    }
}
