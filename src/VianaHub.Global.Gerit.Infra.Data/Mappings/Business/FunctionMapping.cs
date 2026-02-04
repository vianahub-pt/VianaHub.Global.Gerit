using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

public class FunctionMapping : IEntityTypeConfiguration<FunctionEntity>
{
    public void Configure(EntityTypeBuilder<FunctionEntity> builder)
    {
        builder.ToTable("Functions", "dbo");

        // Chave Primária
        builder.HasKey(e => e.Id)
            .HasName("PK_Functions");

        builder.Property(e => e.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(e => e.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(e => e.Name)
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR(150)")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasColumnName("Description")
            .HasColumnType("NVARCHAR(255)")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(e => e.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(e => e.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(e => e.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(e => e.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Relacionamentos
        builder.HasOne(e => e.Tenant)
            .WithMany()
            .HasForeignKey(e => e.TenantId)
            .HasConstraintName("FK_Functions_Tenant")
            .OnDelete(DeleteBehavior.Restrict);
    }
}

