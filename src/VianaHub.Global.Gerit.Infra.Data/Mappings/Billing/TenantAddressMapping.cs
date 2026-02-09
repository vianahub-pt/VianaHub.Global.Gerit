using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Billing;

/// <summary>
/// Mapeamento da entidade TenantAddress
/// Endereços do tenant com suporte a Row Level Security
/// </summary>
public class TenantAddressMapping : IEntityTypeConfiguration<TenantAddress>
{
    public void Configure(EntityTypeBuilder<TenantAddress> builder)
    {
        builder.ToTable("TenantAddresses", "dbo");

        // Chave Primária
        builder.HasKey(x => x.Id)
            .HasName("PK_TenantAddresses");

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.TenantId)
            .HasColumnName("TenantId")
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(x => x.AddressTypeId)
            .HasColumnName("AddressTypeId")
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(x => x.CountryCode)
            .HasColumnName("CountryCode")
            .HasColumnType("CHAR(2)")
            .HasMaxLength(2)
            .HasDefaultValue("PT")
            .IsRequired();

        builder.Property(x => x.Street)
            .HasColumnName("Street")
            .HasColumnType("NVARCHAR(200)")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Neighborhood)
            .HasColumnName("Neighborhood")
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.City)
            .HasColumnName("City")
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.District)
            .HasColumnName("District")
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.PostalCode)
            .HasColumnName("PostalCode")
            .HasColumnType("NVARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.StreetNumber)
            .HasColumnName("StreetNumber")
            .HasColumnType("NVARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(x => x.Complement)
            .HasColumnName("Complement")
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.Latitude)
            .HasColumnName("Latitude")
            .HasColumnType("DECIMAL(9,6)")
            .IsRequired(false);

        builder.Property(x => x.Longitude)
            .HasColumnName("Longitude")
            .HasColumnType("DECIMAL(9,6)")
            .IsRequired(false);

        builder.Property(x => x.Notes)
            .HasColumnName("Notes")
            .HasColumnType("NVARCHAR(500)")
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(x => x.IsPrimary)
            .HasColumnName("IsPrimary")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
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

        // Relacionamentos
        builder.HasOne(x => x.Tenant)
            .WithMany(t => t.Addresses)
            .HasForeignKey(x => x.TenantId)
            .HasConstraintName("FK_TenantAddresses_Tenant")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.AddressType)
            .WithMany()
            .HasForeignKey(x => new { x.AddressTypeId, x.TenantId })
            .HasConstraintName("FK_TenantAddresses_AddressType")
            .HasPrincipalKey(a => new { a.Id, a.TenantId })
            .OnDelete(DeleteBehavior.NoAction);

        // Constraint Única: Garantir que o Id é único dentro do tenant (para FKs compostas)
        builder.HasIndex(x => new { x.Id, x.TenantId })
            .IsUnique()
            .HasDatabaseName("UQ_TenantAddresses_Id_Tenant");
    }
}
