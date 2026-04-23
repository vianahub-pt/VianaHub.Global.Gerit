using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade VisitAddress
/// Endereços da intervenção com suporte a Row Level Security
/// </summary>
public class VisitAddressMapping : IEntityTypeConfiguration<VisitAddressEntity>
{
    public void Configure(EntityTypeBuilder<VisitAddressEntity> builder)
    {
        builder.ToTable("VisitAddresses", "dbo");

        // Chave Primária
        builder.HasKey(x => x.Id)
            .HasName("PK_VisitAddresses");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.TenantId)
            .HasColumnType("INT")
            .IsRequired();

        // Corrigido: coluna correta é VisitId
        builder.Property(x => x.VisitId)
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

        // Relacionamentos
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .HasConstraintName("FK_VisitAddresses_Tenant")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Visit)
            .WithMany(i => i.Addresses)
            .HasForeignKey(x => x.VisitId)
            .HasConstraintName("FK_VisitAddresses_Visit")
            .OnDelete(DeleteBehavior.NoAction);

        // Índices
        builder.HasIndex(x => x.VisitId)
            .HasDatabaseName("IX_VisitAddresses_VisitId")
            .IncludeProperties(x => x.TenantId)
            .HasFilter("[IsDeleted] = 0");
    }
}
