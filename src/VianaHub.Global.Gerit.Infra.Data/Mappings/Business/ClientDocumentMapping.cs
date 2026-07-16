using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade ClientDocument
/// Documentos do Client com suporte a Row Level Security
/// Tabela: dbo.ClientDocuments
/// </summary>
public class ClientDocumentMapping : IEntityTypeConfiguration<ClientDocumentEntity>
{
    public void Configure(EntityTypeBuilder<ClientDocumentEntity> builder)
    {
        builder.ToTable("ClientDocuments", "dbo");

        // Chave Primária
        builder.HasKey(x => x.Id)
            .HasName("PK_ClientDocuments");

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

        builder.Property(x => x.DocumentTypeId)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(x => x.DocumentNumber)
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.IssuingCountryCode)
            .HasColumnType("CHAR(2)")
            .HasMaxLength(2)
            .HasDefaultValue("PT")
            .IsRequired();

        builder.Property(x => x.IssuedAt)
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        builder.Property(x => x.ExpiresAt)
            .HasColumnType("DATETIME2")
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

        // Relacionamento com Client (FK composta ClientId + TenantId)
        builder.HasOne(x => x.Client)
            .WithMany()
            .HasForeignKey(x => new { x.ClientId, x.TenantId })
            .HasPrincipalKey(e => new { e.Id, e.TenantId })
            .HasConstraintName("FK_ClientDocuments_Client")
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento com Tenant
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .HasConstraintName("FK_ClientDocuments_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento com DocumentType
        builder.HasOne(x => x.DocumentType)
            .WithMany()
            .HasForeignKey(x => x.DocumentTypeId)
            .HasConstraintName("FK_ClientDocuments_DocumentType")
            .OnDelete(DeleteBehavior.Restrict);

        // Unique filtered index: apenas um documento primário por Client (ativo e não deletado)
        builder.HasIndex(x => new { x.ClientId, x.TenantId, x.DocumentTypeId })
            .IsUnique()
            .HasFilter("[IsPrimary] = 1 AND [IsActive] = 1 AND [IsDeleted] = 0")
            .HasDatabaseName("UX_ClientDocuments_Primary");

        // Check constraint: ExpiresAt >= IssuedAt quando ambos preenchidos
        builder.HasCheckConstraint(
            "CK_ClientDocuments_ExpiresAt_Gte_IssuedAt",
            "[IssuedAt] IS NULL OR [ExpiresAt] IS NULL OR [ExpiresAt] >= [IssuedAt]");

        // Check constraint: não pode estar ativo e deletado ao mesmo tempo
        builder.HasCheckConstraint(
            "CK_ClientDocuments_Active_Deleted",
            "NOT ([IsActive] = 1 AND [IsDeleted] = 1)");
    }
}
