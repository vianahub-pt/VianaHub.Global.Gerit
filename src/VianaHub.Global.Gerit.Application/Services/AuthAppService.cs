using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VianaHub.Global.Gerit.Application.Configuration;
using VianaHub.Global.Gerit.Application.Dtos.Request.Auth;
using VianaHub.Global.Gerit.Application.Dtos.Response.Auth;
using VianaHub.Global.Gerit.Application.Interfaces;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Helpers;
using VianaHub.Global.Gerit.Domain.Interfaces;
using VianaHub.Global.Gerit.Domain.Interfaces.Repository;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Domain.Tools.Cryptography;
using VianaHub.Global.Gerit.Infra.Data.Context;
using System.Linq;

namespace VianaHub.Global.Gerit.Application.Services;

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
    private readonly VianaHub.Global.Gerit.Domain.Interfaces.ISecretProvider _secretProvider;
    private readonly IUserRoleDataRepository _userRoleRepo;
    private readonly IRolePermissionDataRepository _rolePermissionRepo;

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
        VianaHub.Global.Gerit.Domain.Interfaces.ISecretProvider secretProvider,
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
        await _emailSender.SendAsync(user.Email, "Email.Auth.Confirm.Subject", "Email.Auth.Confirm.Body", user.Name);

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

        // Gera tokens (usa chave RSA do tenant)
        var accessToken = await GenerateAccessTokenAsync(user, ct);
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

        // Rotação: revogar token antigo e criar novo
        await _refreshRepo.RevokeAsync(tokenEntity.Token, user.Id, request.TenantId);

        var newRefreshValue = GenerateRefreshTokenValue();
        var newRefresh = new RefreshTokenEntity(request.TenantId, user.Id, newRefreshValue, DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays), user.Id);
        await _refreshRepo.AddAsync(newRefresh);

        var accessToken = await GenerateAccessTokenAsync(user, ct);

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

    private async Task<(string Token, DateTime ExpiresAt)> GenerateAccessTokenAsync(UserEntity user, CancellationToken ct)
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

        // Adicionar roles e permissões ao token para autorização stateless
        try
        {
            // Buscar roles do usuário
            var userRoles = await _userRoleRepo.GetByUserAsync(user.Id, user.TenantId);

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

            // Coletar permissões de cada role
            var permissions = new HashSet<string>();
            foreach (var role in userRoles ?? Enumerable.Empty<dynamic>())
            {
                if (role?.Role == null) continue;
                var roleId = role.Role.Id;
                var rolePerms = await _rolePermissionRepo.GetByRoleAsync(roleId, user.TenantId);
                if (rolePerms == null) continue;

                foreach (var rp in rolePerms)
                {
                    var resource = rp.Resource?.Name?.Trim();
                    var action = rp.Action?.Name?.Trim();
                    if (string.IsNullOrWhiteSpace(resource) || string.IsNullOrWhiteSpace(action)) continue;

                    var perm = ($"{resource}:{action}").ToLower();
                    permissions.Add(perm);
                }
            }

            foreach (var perm in permissions)
            {
                claims.Add(new Claim("permission", perm));
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
            using var rsa = RSA.Create();
            var privateKeyBytes = CryptoRSA.ExtractBytesFromPem(privatePem, "PRIVATE KEY");
            rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);

            var rsaKey = new RsaSecurityKey(rsa) { KeyId = keyEntity.KeyId.ToString() };
            var creds = new SigningCredentials(rsaKey, SecurityAlgorithms.RsaSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: creds
            );

            var handler = new JwtSecurityTokenHandler();
            var tokenString = handler.WriteToken(token);
            return (tokenString, expires);
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
