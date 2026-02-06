using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VianaHub.Global.Gerit.Application.Configuration;
using VianaHub.Global.Gerit.Domain.Helpers;
using VianaHub.Global.Gerit.Domain.Interfaces;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Domain.Tools.Cryptography;
using VianaHub.Global.Gerit.Infra.Data.Context;
using System.Text.Json;
using VianaHub.Global.Gerit.Application.Interfaces.Identity;
using VianaHub.Global.Gerit.Application.Dtos.Response.Identity.Auth;
using VianaHub.Global.Gerit.Application.Dtos.Request.Identity.Auth;
using VianaHub.Global.Gerit.Domain.Entities.Identity;
using VianaHub.Global.Gerit.Domain.Interfaces.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Identity;

namespace VianaHub.Global.Gerit.Application.Services.Identity;

public class AuthAppService : IAuthAppService
{
    private readonly IUserDataRepository _userRepo;
    private readonly IRefreshTokenDataRepository _refreshRepo;
    private readonly IEmailSender _emailSender;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;
    private readonly ICurrentUserService _currentUser;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<AuthAppService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IJwtKeyDataRepository _jwtKeyRepo;
    private readonly GeritDbContext _dbContext;
    private readonly ISecretProvider _secretProvider;
    private readonly IUserRoleDataRepository _userRoleRepo;
    private readonly IRolePermissionDataRepository _rolePermissionRepo;
    private readonly ISubscriptionDomainService _subscriptionDomain;

    public AuthAppService(
        IUserDataRepository userRepo,
        IRefreshTokenDataRepository refreshRepo,
        IEmailSender emailSender,
        INotify notify,
        ILocalizationService localization,
        ICurrentUserService currentUser,
        IOptions<JwtSettings> jwtOptions,
        ILogger<AuthAppService> logger,
        IConfiguration configuration,
        IJwtKeyDataRepository jwtKeyRepo,
        GeritDbContext dbContext,
        ISecretProvider secretProvider,
        ISubscriptionDomainService subscriptionDomain,
        IUserRoleDataRepository userRoleRepo,
        IRolePermissionDataRepository rolePermissionRepo)
    {
        _userRepo = userRepo;
        _refreshRepo = refreshRepo;
        _emailSender = emailSender;
        _notify = notify;
        _localization = localization;
        _currentUser = currentUser;
        _jwtSettings = jwtOptions.Value;
        _logger = logger;
        _configuration = configuration;
        _jwtKeyRepo = jwtKeyRepo;
        _dbContext = dbContext;
        _secretProvider = secretProvider;
        _subscriptionDomain = subscriptionDomain;
        _userRoleRepo = userRoleRepo;
        _rolePermissionRepo = rolePermissionRepo;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct)
    {
        // Validar tenantId
        if (request.TenantId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Auth.Register.InvalidTenantId"), 400);
            return null;
        }

        // Definir contexto do tenant para RLS
        try
        {
            await _dbContext.SetTenantContextAsync(request.TenantId, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao setar tenant context");
        }

        // Verificar se email já existe (no tenant informado)
        var exists = await _userRepo.ExistsByEmailAsync(request.Email, ct);
        if (exists)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Auth.Register.EmailAlreadyExists"), 409);
            return null;
        }

        // Hash da senha
        var passwordHash = DomainExtensions.HashClientSecret(request.Password);

        // Criar entidade de usuário
        var user = new UserEntity(request.TenantId, request.Name, request.Email, passwordHash, request.PhoneNumber, 0);

        // Persistir via repositório
        var created = await _userRepo.CreateAsync(user, ct);
        if (!created)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Auth.Register.FailedToCreateUser"), 500);
            return null;
        }

        // Enviar email de confirmação (NoOp por enquanto)
        await _emailSender.SendAsync(user.Email, "Application.Service.Auth.Email.Confirm.Subject", "Application.Service.Auth.Email.Confirm.Body", user.Name);

        // Limpar contexto tenant
        try { await _dbContext.ClearTenantContextAsync(ct); } catch { }

        // Retornar sem tokens (login separado)
        return new AuthResponse
        {
            UserId = user.Id,
            TenantId = user.TenantId,
            Email = user.Email,
            Name = user.Name
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        // Validar tenantId básico
        if (request.TenantId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Auth.Login.InvalidTenantId"), 400);
            return null;
        }

        // Set tenant context for RLS
        try
        {
            await _dbContext.SetTenantContextAsync(request.TenantId, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao setar tenant context");
        }

        var user = await _userRepo.GetByEmailAsync(request.Email, ct);
        if (user == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Auth.Login.InvalidCredentials"), 401);
            return null;
        }

        // Verifica senha
        if (!DomainExtensions.VerifyClientSecret(user.PasswordHash, request.Password))
        {
            _notify.Add(_localization.GetMessage("Application.Service.Auth.Login.InvalidCredentials"), 401);
            return null;
        }

        if (!await _subscriptionDomain.IsActiveAsync(request.TenantId, ct))
        {
            _notify.Add(_localization.GetMessage("Application.Service.Auth.Login.Subscription.NonActive"), 403);
            return null;
        }
        if (await _subscriptionDomain.IsCanceledAsync(request.TenantId, ct))
        {
            _notify.Add(_localization.GetMessage("Application.Service.Auth.Login.Subscription.Canceled"), 403);
            return null;
        }
        if (await _subscriptionDomain.IsDeletedAsync(request.TenantId, ct))
        {
            _notify.Add(_localization.GetMessage("Application.Service.Auth.Login.Subscription.Deleted"), 403);
            return null;
        }

        var isTrial = await _subscriptionDomain.IsTrialAsync(request.TenantId, ct);
        if (isTrial)
        {
            if (await _subscriptionDomain.IsTrialPeriodExpiredAsync(request.TenantId, ct))
            {
                _notify.Add(_localization.GetMessage("Application.Service.Auth.Login.Subscription.TrialPeriodExpired"), 403);
                return null;
            }
        }
        else if (await _subscriptionDomain.IsSubscriptionPeriodExpiredAsync(request.TenantId, ct))
        {
            _notify.Add(_localization.GetMessage("Application.Service.Auth.Login.Subscription.PeriodExpired"), 403);
            return null;
        }

        // Gera tokens (usa chave RSA do tenant)
        var accessToken = await GenerateAccessTokenAsync(user, isTrial, ct);
        if (accessToken.Token == null)
        {
            // Erro já notificado
            return null;
        }

        var refreshTokenValue = GenerateRefreshTokenValue();

        var refreshTokenEntity = new RefreshTokenEntity(user.TenantId, user.Id, refreshTokenValue, DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays), user.Id);
        await _refreshRepo.AddAsync(refreshTokenEntity);

        try { await _dbContext.ClearTenantContextAsync(ct); } catch { }

        return new AuthResponse
        {
            AccessToken = accessToken.Token,
            RefreshToken = refreshTokenValue,
            AccessTokenExpiresAt = accessToken.ExpiresAt,
            RefreshTokenExpiresAt = refreshTokenEntity.ExpiresAt,
            UserId = user.Id,
            TenantId = user.TenantId,
            Email = user.Email,
            Name = user.Name
        };
    }

    public async Task<AuthResponse> RefreshAsync(RefreshRequest request, CancellationToken ct)
    {
        if (request.TenantId <= 0)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Auth.Refresh.InvalidTenantId"), 400);
            return null;
        }

        try
        {
            await _dbContext.SetTenantContextAsync(request.TenantId, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao setar tenant context");
        }

        var tokenEntity = await _refreshRepo.GetByTokenAsync(request.RefreshToken, request.TenantId);
        if (tokenEntity == null || !tokenEntity.IsActive())
        {
            _notify.Add(_localization.GetMessage("Application.Service.Auth.Refresh.InvalidRefreshToken"), 401);
            return null;
        }

        // Encontrar usuário
        var user = await _userRepo.GetByIdAsync(tokenEntity.UserId, ct);
        if (user == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Auth.Refresh.UserNotFound"), 410);
            return null;
        }

        if (!await _subscriptionDomain.IsActiveAsync(request.TenantId, ct))
        {
            _notify.Add(_localization.GetMessage("Application.Service.Auth.RefreshToken.Subscription.NonActive"), 403);
            return null;
        }
        if (await _subscriptionDomain.IsCanceledAsync(request.TenantId, ct))
        {
            _notify.Add(_localization.GetMessage("Application.Service.Auth.RefreshToken.Subscription.Canceled"), 403);
            return null;
        }
        if (await _subscriptionDomain.IsDeletedAsync(request.TenantId, ct))
        {
            _notify.Add(_localization.GetMessage("Application.Service.Auth.RefreshToken.Subscription.Deleted"), 403);
            return null;
        }

        var isTrial = await _subscriptionDomain.IsTrialAsync(user.Id, ct);
        if (isTrial)
        {
            if (await _subscriptionDomain.IsTrialPeriodExpiredAsync(user.Id, ct))
            {
                _notify.Add(_localization.GetMessage("Application.Service.Auth.RefreshToken.Subscription.TrialPeriodExpired"), 403);
                return null;
            }
        }
        else if (await _subscriptionDomain.IsSubscriptionPeriodExpiredAsync(user.Id, ct))
        {
            _notify.Add(_localization.GetMessage("Application.Service.Auth.RefreshToken.Subscription.PeriodExpired"), 403);
            return null;
        }

        // Rotação: revogar token antigo e criar novo
        await _refreshRepo.RevokeAsync(tokenEntity.Token, user.Id, request.TenantId);

        var newRefreshValue = GenerateRefreshTokenValue();
        var newRefresh = new RefreshTokenEntity(request.TenantId, user.Id, newRefreshValue, DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays), user.Id);
        await _refreshRepo.AddAsync(newRefresh);

        var accessToken = await GenerateAccessTokenAsync(user, isTrial, ct);

        try { await _dbContext.ClearTenantContextAsync(ct); } catch { }

        return new AuthResponse
        {
            AccessToken = accessToken.Token,
            RefreshToken = newRefreshValue,
            AccessTokenExpiresAt = accessToken.ExpiresAt,
            RefreshTokenExpiresAt = newRefresh.ExpiresAt,
            UserId = user.Id,
            TenantId = user.TenantId,
            Email = user.Email,
            Name = user.Name
        };
    }

    private async Task<(string Token, DateTime ExpiresAt)> GenerateAccessTokenAsync(UserEntity user, bool isTrial, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var expires = now.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);

        // Gera claims básicos
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim("tenant_id", user.TenantId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (isTrial)
        {
            claims.Add(new Claim("tenant_subscription_type", "trial"));
        }

        string permissionsJson = null;

        // Adicionar roles e permissões ao token para autorização stateless
        try
        {
            // Buscar roles do usuário
            var userRoles = await _userRoleRepo.GetByUserAsync(user.Id, user.TenantId, ct);

            var roleNames = userRoles?
                .Where(r => r?.Role != null)
                .Select(r => r.Role.Name?.Trim())
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Select(n => n.ToLower())
                .Distinct()
                .ToList() ?? new List<string>();

            foreach (var roleName in roleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
                // adicionar claim alternativa 'role' para compatibilidade
                claims.Add(new Claim("role", roleName));
            }

            // Coletar permissões de cada role agrupadas por recurso
            var permissionsByResource = new Dictionary<string, HashSet<string>>();

            foreach (var role in userRoles ?? Enumerable.Empty<dynamic>())
            {
                if (role?.Role == null) continue;
                var roleId = role.Role.Id;
                var rolePerms = await _rolePermissionRepo.GetByRoleAsync(roleId, user.TenantId, ct);
                if (rolePerms == null) continue;

                foreach (var rp in rolePerms)
                {
                    var resource = rp.Resource?.Name?.Trim();
                    var action = rp.Action?.Name?.Trim();
                    if (string.IsNullOrWhiteSpace(resource) || string.IsNullOrWhiteSpace(action)) continue;

                    var resourceKey = resource.ToLower();
                    var actionKey = action.ToLower();

                    if (!permissionsByResource.TryGetValue(resourceKey, out HashSet<string> actions))
                    {
                        actions = new HashSet<string>();
                        permissionsByResource[resourceKey] = actions;
                    }

                    actions.Add(actionKey);
                }
            }

            // Serializar o dicionário para JSON e guardar em variável (não adicionar como claim string)
            if (permissionsByResource.Any())
            {
                // Converter HashSet<string> para List<string> para serialização previsível
                var serializable = permissionsByResource.ToDictionary(k => k.Key, v => v.Value.OrderBy(x => x).ToList());
                permissionsJson = JsonSerializer.Serialize(serializable);
                // Não adicionar como Claim string para evitar escape no payload
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Falha ao incluir roles/permissions no token para user {UserId}", user.Id);
            // Não falhar a geração do token por causa das claims de permissão; conceder token básico
        }

        // Carregar chave ativa do tenant
        var keyEntity = await _jwtKeyRepo.GetActiveKeyAsync(user.TenantId, ct);
        if (keyEntity == null)
        {
            _notify.Add(_localization.GetMessage("Application.Service.Auth.Token.NoActiveKey"), 500);
            _logger.LogError("Nenhuma chave JWT ativa encontrada para tenant {TenantId}", user.TenantId);
            return (null, DateTime.MinValue);
        }

        // Obter chave mestra do provider de segredos
        var masterKey = _secretProvider.GetMasterKey();
        if (string.IsNullOrWhiteSpace(masterKey))
        {
            _notify.Add(_localization.GetMessage("Application.Service.Auth.Token.EncryptionKeyMissing"), 500);
            _logger.LogError("Master key for JWT decryption not available");
            return (null, DateTime.MinValue);
        }

        string privatePem;
        try
        {
            privatePem = CryptoRSA.DecryptPrivateKey(keyEntity.PrivateKeyEncrypted, masterKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao descriptografar chave privada para tenant {TenantId}", user.TenantId);
            _notify.Add(_localization.GetMessage("Application.Service.Auth.Token.InvalidPrivateKey"), 500);
            return (null, DateTime.MinValue);
        }

        // Import private key into RSA
        try
        {
            // Não usar 'using' aqui: precisamos garantir que a instância RSA permaneça viva
            // durante a operação de assinatura (WriteToken). Alguns provedores internos
            // podem acessar o objeto RSA durante a escrita do token e provocar
            // ObjectDisposedException se ele for descartado antes.
            var rsa = RSA.Create();
            try
            {
                var privateKeyBytes = CryptoRSA.ExtractBytesFromPem(privatePem, "PRIVATE KEY");
                rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);

                var rsaKey = new RsaSecurityKey(rsa) { KeyId = keyEntity.KeyId.ToString() };
                // Evitar cache de SignatureProviders que podem reter referência ao objeto RSA e
                // causar ObjectDisposedException em chamadas subsequentes. Garantir que cada
                // geração de token use um provider fresco.
                rsaKey.CryptoProviderFactory = new CryptoProviderFactory
                {
                    CacheSignatureProviders = false
                };
                var creds = new SigningCredentials(rsaKey, SecurityAlgorithms.RsaSha256);

                var token = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    notBefore: now,
                    expires: expires,
                    signingCredentials: creds
                );

                // Se temos permissionsJson, inserir como objeto JSON no payload para evitar escaping
                if (!string.IsNullOrWhiteSpace(permissionsJson))
                {
                    try
                    {
                        var doc = JsonDocument.Parse(permissionsJson);
                        token.Payload["permissions"] = doc.RootElement.Clone();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Falha ao parsear permissionsJson antes de inserir no payload");
                    }
                }

                var handler = new JwtSecurityTokenHandler();
                var tokenString = handler.WriteToken(token);
                return (tokenString, expires);
            }
            finally
            {
                // Garantir liberação explícita após a escrita do token
                try { rsa.Dispose(); } catch { }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar token JWT para tenant {TenantId}", user.TenantId);
            _notify.Add(_localization.GetMessage("Application.Service.Auth.Token.CreateFailed"), 500);
            return (null, DateTime.MinValue);
        }
    }

    private string GenerateRefreshTokenValue()
    {
        var bytes = new byte[64];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes).TrimEnd('=');
    }
}
