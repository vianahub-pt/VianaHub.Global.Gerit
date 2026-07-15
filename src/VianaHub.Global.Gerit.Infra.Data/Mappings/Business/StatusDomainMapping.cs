using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade StatusDomain (Domínios de Status: VisitStatus, EquipmentStatus, etc.).
/// Tabela: dbo.StatusDomains
/// Lookup global sem TenantId. Não é Aggregate Root.
/// </summary>
public class StatusDomainMapping : IEntityTypeConfiguration<StatusDomainEntity>
{
    public void Configure(EntityTypeBuilder<StatusDomainEntity> builder)
    {
        builder.ToTable("StatusDomains", "dbo");

        // Chave Primária
        builder.HasKey(x => x.Id)
            .HasName("PK_StatusDomains");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.Code)
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        // FK para a tabela de traduções StatusDomainTranslations
        builder.HasMany(x => x.Translations)
            .WithOne(x => x.StatusDomain)
            .HasForeignKey(x => x.StatusDomainId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_StatusDomainTranslations_StatusDomains_StatusDomainId");

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
    }
}
