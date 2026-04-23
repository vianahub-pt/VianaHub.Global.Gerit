using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Billing;

/// <summary>
/// Mapeamento da entidade TenantAddress
/// Endereços do tenant com suporte a Row Level Security
/// </summary>
public class TenantAddressMapping : IEntityTypeConfiguration<TenantAddressEntity>
{
    public void Configure(EntityTypeBuilder<TenantAddressEntity> builder)
    {
        builder.ToTable("TenantAddresses", "dbo");

        // Chave Primária
        builder.HasKey(x => x.Id)
            .HasName("PK_TenantAddresses");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.TenantId)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(x => x.AddressTypeId)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(x => x.CountryCode)
            .HasColumnType("CHAR(2)")
            .HasMaxLength(2)
            .HasDefaultValue("PT")
            .IsRequired();

        builder.Property(x => x.Street)
            .HasColumnType("NVARCHAR(200)")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Neighborhood)
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.City)
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.District)
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.PostalCode)
            .HasColumnType("NVARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.StreetNumber)
            .HasColumnType("NVARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(x => x.Complement)
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.Latitude)
            .HasColumnType("DECIMAL(9,6)")
            .IsRequired(false);

        builder.Property(x => x.Longitude)
            .HasColumnType("DECIMAL(9,6)")
            .IsRequired(false);

        builder.Property(x => x.Note)
            .HasColumnType("NVARCHAR(500)")
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(x => x.IsPrimary)
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

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

        // Relacionamentos
        builder.HasOne(x => x.Tenant)
            .WithMany(t => t.Addresses)
            .HasForeignKey(x => x.TenantId)
            .HasConstraintName("FK_TenantAddresses_Tenant")
            .OnDelete(DeleteBehavior.NoAction);

        // Constraint Única: Garantir que o Id é único dentro do tenant (para FKs compostas)
        builder.HasIndex(x => new { x.Id, x.TenantId })
            .IsUnique()
            .HasDatabaseName("UQ_TenantAddresses_Id_Tenant");
    }
}
