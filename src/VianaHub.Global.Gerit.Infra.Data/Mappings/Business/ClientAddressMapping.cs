using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade ClientAddress
/// Endereços do cliente com suporte a Row Level Security
/// </summary>
public class ClientAddressMapping : IEntityTypeConfiguration<ClientAddressEntity>
{
    public void Configure(EntityTypeBuilder<ClientAddressEntity> builder)
    {
        builder.ToTable("ClientAddresses", "dbo");

        // Chave Primária
        builder.HasKey(ca => ca.Id)
            .HasName("PK_ClientAddresses");

        builder.Property(ca => ca.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(ca => ca.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(ca => ca.ClientId)
            .HasColumnName("ClientId")
            .IsRequired();

        builder.Property(ca => ca.Street)
            .HasColumnName("Street")
            .HasColumnType("NVARCHAR(200)")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(ca => ca.City)
            .HasColumnName("City")
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(ca => ca.PostalCode)
            .HasColumnName("PostalCode")
            .HasColumnType("NVARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(ca => ca.District)
            .HasColumnName("District")
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(ca => ca.CountryCode)
            .HasColumnName("CountryCode")
            .HasColumnType("CHAR(2)")
            .HasMaxLength(2)
            .HasDefaultValue("PT")
            .IsRequired();

        builder.Property(ca => ca.IsPrimary)
            .HasColumnName("IsPrimary")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(ca => ca.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(ca => ca.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(ca => ca.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(ca => ca.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(ca => ca.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(ca => ca.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Índices únicos com filtro para endereço primário
        builder.HasIndex(ca => ca.ClientId)
            .IsUnique()
            .HasDatabaseName("UX_ClientAddresses_Primary")
            .HasFilter("[IsPrimary] = 1 AND [IsDeleted] = 0");

        // Índices não clusterizados
        builder.HasIndex(ca => ca.ClientId)
            .HasDatabaseName("IX_ClientAddresses_ClientId")
            .IncludeProperties(ca => ca.TenantId)
            .HasFilter("[IsDeleted] = 0");

        // Relacionamentos
        builder.HasOne(ca => ca.Tenant)
            .WithMany()
            .HasForeignKey(ca => ca.TenantId)
            .HasConstraintName("FK_ClientAddresses_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento com Client configurado no ClientMapping
    }
}
