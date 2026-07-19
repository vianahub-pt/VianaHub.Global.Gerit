using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Identity;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Identity;

/// <summary>
/// Mapeamento da entidade RefreshToken
/// Tabela: dbo.RefreshTokens com suporte a Row Level Security
/// </summary>
public class RefreshTokensMapping : IEntityTypeConfiguration<RefreshTokensEntity>
{
    public void Configure(EntityTypeBuilder<RefreshTokensEntity> builder)
    {
        builder.ToTable("RefreshTokens", "dbo");

        // Chave Primaria
        builder.HasKey(x => x.Id)
            .HasName("PK_RefreshTokens");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.TenantId)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(x => x.TokenHash)
            .HasColumnName("TokenHash")
            .HasColumnType("VARBINARY(64)")
            .IsRequired()
            .HasConversion(
                v => Convert.FromBase64String(v!),           // string → byte[] para INSERT/UPDATE
                v => Convert.ToBase64String(v).TrimEnd('=')  // byte[] → string para SELECT
            );

        builder.Property(x => x.ExpiresAt)
            .HasColumnType("DATETIME2(7)")
            .IsRequired();

        builder.Property(x => x.RevokedAt)
            .HasColumnType("DATETIME2(7)")
            .IsRequired(false);

        builder.Property(x => x.RevokedBy)
            .HasColumnType("INT")
            .IsRequired(false);

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

        // Indice unico: TokenHash
        builder.HasIndex(x => x.TokenHash)
            .IsUnique()
            .HasDatabaseName("UX_RefreshTokens_TokenHash");

        // Indice: tokens ativos por usuario
        builder.HasIndex(x => new { x.TenantId, x.UserId })
            .HasDatabaseName("IX_RefreshTokens_User_Active")
            .HasFilter("[RevokedAt] IS NULL");

        // Indice: tokens por expiracao
        builder.HasIndex(x => new { x.TenantId, x.ExpiresAt })
            .HasDatabaseName("IX_RefreshTokens_ExpiresAt")
            .HasFilter("[RevokedAt] IS NULL");
    }
}
