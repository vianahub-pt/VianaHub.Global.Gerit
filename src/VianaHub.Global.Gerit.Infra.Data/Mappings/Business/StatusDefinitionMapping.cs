using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade StatusDefinition (Definições de Status por tenant e domínio).
/// Tabela: dbo.StatusDefinitions — tenant-scoped com FK composta para traduções.
/// </summary>
public class StatusDefinitionMapping : IEntityTypeConfiguration<StatusDefinitionEntity>
{
    public void Configure(EntityTypeBuilder<StatusDefinitionEntity> builder)
    {
        builder.ToTable("StatusDefinitions", "dbo");

        // Chave Primária
        builder.HasKey(x => x.Id)
            .HasName("PK_StatusDefinitions");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Chaves alternativas para suportar FKs compostas com TenantId e StatusDomainId
        builder.HasAlternateKey(x => new { x.Id, x.TenantId })
            .HasName("UQ_StatusDefinitions_Id_Tenant");

        builder.HasAlternateKey(x => new { x.Id, x.TenantId, x.StatusDomainId })
            .HasName("UQ_StatusDefinitions_Id_Tenant_Domain");

        // Propriedades
        builder.Property(x => x.TenantId)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(x => x.StatusDomainId)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(x => x.Code)
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.DisplayOrder)
            .HasColumnType("INT")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(x => x.IsSystem)
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

        // Auditoria
        builder.Property(x => x.CreatedBy)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnType("DATETIME2(7)")
            .HasDefaultValueSql("SYSUTCDATETIME()")
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
            .HasConstraintName("FK_StatusDefinitions_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.StatusDomain)
            .WithMany()
            .HasForeignKey(x => x.StatusDomainId)
            .HasConstraintName("FK_StatusDefinitions_StatusDomain")
            .OnDelete(DeleteBehavior.Restrict);

        // FK composta para traduções: (StatusDefinitionId, TenantId, StatusDomainId) → (Id, TenantId, StatusDomainId)
        builder.HasMany(x => x.Translations)
            .WithOne(x => x.StatusDefinition)
            .HasForeignKey(x => new { x.StatusDefinitionId, x.TenantId, x.StatusDomainId })
            .HasPrincipalKey(x => new { x.Id, x.TenantId, x.StatusDomainId })
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_StatusDefinitionTranslations_StatusDefinition");

        // Índice único composto (TenantId, StatusDomainId, Code) — código é único por tenant e domínio
        builder.HasIndex(x => new { x.TenantId, x.StatusDomainId, x.Code })
            .IsUnique()
            .HasFilter("IsDeleted = 0")
            .HasDatabaseName("UX_StatusDefinitions_Tenant_Domain_Code");

        // Índice não-clustered para consultas por tenant e domínio
        builder.HasIndex(x => new { x.TenantId, x.StatusDomainId })
            .IncludeProperties(x => new { x.Code, x.DisplayOrder, x.IsActive })
            .HasFilter("IsDeleted = 0")
            .HasDatabaseName("IX_StatusDefinitions_Tenant_Domain");
    }
}
