using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade VisitContact
/// Contatos da intervenção com suporte a Row Level Security
/// </summary>
public class VisitContactMapping : IEntityTypeConfiguration<VisitContactEntity>
{
    public void Configure(EntityTypeBuilder<VisitContactEntity> builder)
    {
        builder.ToTable("VisitContacts", "dbo");

        // Chave Primária
        builder.HasKey(x => x.Id)
            .HasName("PK_VisitContacts");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.TenantId)
            .IsRequired();

        builder.Property(x => x.VisitId)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnType("NVARCHAR(150)")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnType("NVARCHAR(255)")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Phone)
            .HasColumnType("NVARCHAR(30)")
            .HasMaxLength(30)
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

        // Índices
        builder.HasIndex(x => x.VisitId)
            .HasDatabaseName("IX_VisitContacts_VisitId")
            .IncludeProperties(x => x.TenantId)
            .HasFilter("[IsDeleted] = 0");

        // Relacionamentos
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .HasConstraintName("FK_VisitContacts_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento com Visit configurado no VisitMapping
    }
}
