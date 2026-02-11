using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade AddressType
/// Tipos de endereço (residencial, comercial, etc)
/// </summary>
public class AddressTypeMapping : IEntityTypeConfiguration<AddressTypeEntity>
{
    public void Configure(EntityTypeBuilder<AddressTypeEntity> builder)
    {
        builder.ToTable("AddressTypes", "dbo");

        // Chave Primária
        builder.HasKey(x => x.Id)
            .HasName("PK_AddressTypes");

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // TenantId
        builder.Property(x => x.TenantId)
            .HasColumnName("TenantId")
            .HasColumnType("INT")
            .IsRequired();

        // Propriedades
        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("Description")
            .HasColumnType("NVARCHAR(255)")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

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

        // Foreign Key para Tenant
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .HasConstraintName("FK_AddressTypes_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        // Constraint Única: Tipo de endereço único por tenant
        builder.HasIndex(x => new { x.TenantId, x.Name })
            .IsUnique()
            .HasDatabaseName("UQ_AddressTypes_TenantId_Name");
    }
}
