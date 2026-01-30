using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade TeamMember
/// Membros do time com suporte a Row Level Security
/// </summary>
public class TeamMemberMapping : IEntityTypeConfiguration<TeamMemberEntity>
{
    public void Configure(EntityTypeBuilder<TeamMemberEntity> builder)
    {
        builder.ToTable("TeamMembers", "dbo");

        // Chave Primária
        builder.HasKey(tm => tm.Id)
            .HasName("PK_TeamMembers");

        builder.Property(tm => tm.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(tm => tm.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(tm => tm.Name)
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR(150)")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(tm => tm.TaxNumber)
            .HasColumnName("TaxNumber")
            .HasColumnType("NVARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(tm => tm.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(tm => tm.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(tm => tm.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(tm => tm.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(tm => tm.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(tm => tm.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Relacionamentos
        builder.HasOne(tm => tm.Tenant)
            .WithMany()
            .HasForeignKey(tm => tm.TenantId)
            .HasConstraintName("FK_TeamMembers_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(tm => tm.Contacts)
            .WithOne(tmc => tmc.TeamMember)
            .HasForeignKey(tmc => tmc.TeamMemberId)
            .HasConstraintName("FK_TeamMemberContacts_TeamMember")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(tm => tm.Addresses)
            .WithOne(tma => tma.TeamMember)
            .HasForeignKey(tma => tma.TeamMemberId)
            .HasConstraintName("FK_TeamMemberAddresses_TeamMember")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
