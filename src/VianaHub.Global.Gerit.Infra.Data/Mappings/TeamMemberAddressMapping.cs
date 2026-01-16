using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings;

/// <summary>
/// Mapeamento da entidade TeamMemberAddress
/// Endereços do membro do time com suporte a Row Level Security
/// </summary>
public class TeamMemberAddressMapping : IEntityTypeConfiguration<TeamMemberAddressEntity>
{
    public void Configure(EntityTypeBuilder<TeamMemberAddressEntity> builder)
    {
        builder.ToTable("TeamMemberAddresses", "dbo");

        // Chave Primária
        builder.HasKey(tma => tma.Id)
            .HasName("PK_TeamMemberAddresses");

        builder.Property(tma => tma.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(tma => tma.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(tma => tma.TeamMemberId)
            .HasColumnName("TeamMemberId")
            .IsRequired();

        builder.Property(tma => tma.Street)
            .HasColumnName("Street")
            .HasColumnType("NVARCHAR(200)")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(tma => tma.City)
            .HasColumnName("City")
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(tma => tma.PostalCode)
            .HasColumnName("PostalCode")
            .HasColumnType("NVARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(tma => tma.District)
            .HasColumnName("District")
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(tma => tma.CountryCode)
            .HasColumnName("CountryCode")
            .HasColumnType("CHAR(2)")
            .HasMaxLength(2)
            .HasDefaultValue("PT")
            .IsRequired();

        builder.Property(tma => tma.IsPrimary)
            .HasColumnName("IsPrimary")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(tma => tma.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(tma => tma.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(tma => tma.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(tma => tma.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(tma => tma.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(tma => tma.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Índices
        builder.HasIndex(tma => tma.TeamMemberId)
            .HasDatabaseName("IX_TeamMemberAddresses_TeamMemberId")
            .IncludeProperties(tma => tma.TenantId)
            .HasFilter("[IsDeleted] = 0");

        // Relacionamentos
        builder.HasOne(tma => tma.Tenant)
            .WithMany()
            .HasForeignKey(tma => tma.TenantId)
            .HasConstraintName("FK_TeamMemberAddresses_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento com TeamMember configurado no TeamMemberMapping
    }
}
