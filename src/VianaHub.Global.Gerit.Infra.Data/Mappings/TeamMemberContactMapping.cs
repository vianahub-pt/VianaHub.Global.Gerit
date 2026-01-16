using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings;

/// <summary>
/// Mapeamento da entidade TeamMemberContact
/// Contatos do membro do time com suporte a Row Level Security
/// </summary>
public class TeamMemberContactMapping : IEntityTypeConfiguration<TeamMemberContactEntity>
{
    public void Configure(EntityTypeBuilder<TeamMemberContactEntity> builder)
    {
        builder.ToTable("TeamMemberContacts", "dbo");

        // Chave Primária
        builder.HasKey(tmc => tmc.Id)
            .HasName("PK_TeamMemberContacts");

        builder.Property(tmc => tmc.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(tmc => tmc.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(tmc => tmc.TeamMemberId)
            .HasColumnName("TeamMemberId")
            .IsRequired();

        builder.Property(tmc => tmc.Name)
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR(150)")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(tmc => tmc.Email)
            .HasColumnName("Email")
            .HasColumnType("NVARCHAR(255)")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(tmc => tmc.Phone)
            .HasColumnName("Phone")
            .HasColumnType("NVARCHAR(30)")
            .HasMaxLength(30)
            .IsRequired(false);

        builder.Property(tmc => tmc.IsPrimary)
            .HasColumnName("IsPrimary")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(tmc => tmc.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(tmc => tmc.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(tmc => tmc.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(tmc => tmc.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(tmc => tmc.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(tmc => tmc.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Índices
        builder.HasIndex(tmc => tmc.TeamMemberId)
            .HasDatabaseName("IX_TeamMemberContacts_TeamMemberId")
            .IncludeProperties(tmc => tmc.TenantId)
            .HasFilter("[IsDeleted] = 0");

        // Relacionamentos
        builder.HasOne(tmc => tmc.Tenant)
            .WithMany()
            .HasForeignKey(tmc => tmc.TenantId)
            .HasConstraintName("FK_TeamMemberContacts_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento com TeamMember configurado no TeamMemberMapping
    }
}
