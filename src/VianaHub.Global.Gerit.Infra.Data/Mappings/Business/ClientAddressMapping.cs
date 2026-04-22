using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade ClientAddress
/// Endere�os do cliente com suporte a Row Level Security
/// </summary>
public class ClientAddressMapping : IEntityTypeConfiguration<ClientAddressEntity>
{
    public void Configure(EntityTypeBuilder<ClientAddressEntity> builder)
    {
        builder.ToTable("ClientAddresses", "dbo");

        // Chave Prim�ria
        builder.HasKey(x => x.Id)
            .HasName("PK_ClientAddresses");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.TenantId)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(x => x.ClientId)
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
        builder.HasOne(x => x.Client)
            .WithMany(c => c.Addresses)
            .HasForeignKey(x => x.ClientId)
            .HasConstraintName("FK_ClientAddresses_Client")
            .OnDelete(DeleteBehavior.NoAction);

        // �ndices �nicos com filtro para endere�o prim�rio
        builder.HasIndex(x => x.ClientId)
            .IsUnique()
            .HasDatabaseName("UX_ClientAddresses_Primary")
            .HasFilter("[IsPrimary] = 1 AND [IsDeleted] = 0");

        // �ndices n�o clusterizados
        builder.HasIndex(x => x.ClientId)
            .HasDatabaseName("IX_ClientAddresses_ClientId")
            .IncludeProperties(x => x.TenantId)
            .HasFilter("[IsDeleted] = 0");
    }
}

