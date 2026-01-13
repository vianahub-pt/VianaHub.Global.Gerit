namespace VianaHub.Global.Gerit.Application.Configuration;

/// <summary>
/// Configurações do JWT (JSON Web Token) para autenticação e autorização.
/// Esta classe é compartilhada entre as camadas Application e API.
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Emissor do token (issuer).
    /// Identifica quem emitiu o token.
    /// </summary>
    public string Issuer { get; set; } = "VianaHub.VianaID";

    /// <summary>
    /// Audiência do token (audience).
    /// Identifica para quem o token foi emitido.
    /// </summary>
    public string Audience { get; set; } = "VianaHub.VianaID.Api";

    /// <summary>
    /// Múltiplos emissores válidos (para suporte a múltiplos ambientes).
    /// </summary>
    public string[]? ValidIssuers { get; set; }

    /// <summary>
    /// Múltiplas audiências válidas.
    /// </summary>
    public string[]? ValidAudiences { get; set; }

    /// <summary>
    /// Tempo de expiração do Access Token em minutos (padrão: 15 minutos).
    /// </summary>
    public int AccessTokenExpirationMinutes { get; set; } = 15;

    /// <summary>
    /// Tempo de expiração do Access Token em minutos (compatibilidade retroativa).
    /// Mapeado para AccessTokenExpirationMinutes.
    /// </summary>
    [Obsolete("Use AccessTokenExpirationMinutes. Esta propriedade será removida na versão 2.0")]
    public int ExpirationInMinutes
    {
        get => AccessTokenExpirationMinutes;
        set => AccessTokenExpirationMinutes = value;
    }

    /// <summary>
    /// Tempo de expiração do Refresh Token em dias (padrão: 7 dias).
    /// </summary>
    public int RefreshTokenExpirationDays { get; set; } = 7;

    /// <summary>
    /// Tempo de expiração do Refresh Token em dias (compatibilidade retroativa).
    /// Mapeado para RefreshTokenExpirationDays.
    /// </summary>
    [Obsolete("Use RefreshTokenExpirationDays. Esta propriedade será removida na versão 2.0")]
    public int RefreshTokenExpirationInDays
    {
        get => RefreshTokenExpirationDays;
        set => RefreshTokenExpirationDays = value;
    }

    /// <summary>
    /// Clock skew em minutos para compensar diferenças de relógio (padrão: 5 minutos).
    /// </summary>
    public int ClockSkewMinutes { get; set; } = 5;

    /// <summary>
    /// Exigir HTTPS para metadata (deve ser true em produção).
    /// </summary>
    public bool RequireHttpsMetadata { get; set; } = true;

    /// <summary>
    /// Habilitar rotação automática de Refresh Tokens.
    /// </summary>
    public bool EnableRefreshTokenRotation { get; set; } = true;

    /// <summary>
    /// Período de graça para tokens antigos após rotação (em segundos).
    /// </summary>
    public int RotationGracePeriodSeconds { get; set; } = 30;
}
