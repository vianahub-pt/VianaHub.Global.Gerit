using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade ClientContact
/// Contatos do cliente com suporte a Row Level Security
/// </summary>
public class ClientContactPersonsMapping : IEntityTypeConfiguration<ClientContactPersonsEntity>
{
    public void Configure(EntityTypeBuilder<ClientContactPersonsEntity> builder)
    {
        builder.ToTable("ClientContactPersons", "dbo");

        // Chave Primaria
        builder.HasKey(x => x.Id)
            .HasName("PK_ClientContactPersons");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.TenantId)
            .IsRequired();

        builder.Property(x => x.ClientId)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnType("NVARCHAR(150)")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(x => x.CellPhoneNumber)
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired(false);

        // IsWhatsapp � coluna fantasma — a coluna real no SQL � IsCellPhoneWhatsapp
        builder.Ignore(x => x.IsWhatsapp);

        builder.Property(x => x.Email)
            .HasColumnType("NVARCHAR(255)")
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(x => x.JobTitle)
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.Department)
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.IsCellPhoneWhatsapp)
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

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

        // Indices
        // Email unico por tenant+cliente (apenas ativos e nao deletados)
        builder.HasIndex(x => new { x.TenantId, x.ClientId, x.Email })
            .IsUnique()
            .HasDatabaseName("UX_ClientContactPersons_Email")
            .HasFilter("[Email] IS NOT NULL AND [IsActive] = 1 AND [IsDeleted] = 0");

        // Apenas um contato primario por tenant+cliente
        builder.HasIndex(x => new { x.TenantId, x.ClientId })
            .IsUnique()
            .HasDatabaseName("UX_ClientContactPersons_Primary")
            .HasFilter("[IsPrimary] = 1 AND [IsActive] = 1 AND [IsDeleted] = 0");

        // Indice nao clusterizado: busca por tenant + client
        builder.HasIndex(x => new { x.TenantId, x.ClientId })
            .HasDatabaseName("IX_ClientContactPersons_Client")
            .HasFilter("[IsDeleted] = 0");
    }
}
