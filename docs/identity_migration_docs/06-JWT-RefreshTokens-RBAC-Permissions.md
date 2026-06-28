# 06 — JWT, Refresh Tokens, RBAC e Permissions

## Objetivo

Alinhar emissão de tokens, refresh token e autorização ao modelo v2. Cada access token deve representar exatamente um usuário, um tenant ativo e uma app ativa.

## Arquivos impactados

```text
src/EBL.FIG.Process.Identity.Application/Services/JwtTokenService.cs
src/EBL.FIG.Process.Identity.Application/Services/RefreshTokenService.cs
src/EBL.FIG.Process.Identity.Application/Interfaces/IJwtTokenService.cs
src/EBL.FIG.Process.Identity.Application/Interfaces/IRefreshTokenService.cs
src/EBL.FIG.Process.Identity.Domain/Entities/RefreshTokenEntity.cs
src/EBL.FIG.Process.Identity.Domain/Entities/JwtKeyEntity.cs
src/EBL.FIG.Process.Identity.Infra.Data/Repository/JwtKeyDataRepository.cs
src/EBL.FIG.Process.Identity.Infra.Data/Repository/RefreshTokenDataRepository.cs
src/EBL.FIG.Process.Identity.Api/Filters/AuthorizationFilter.cs
src/EBL.FIG.Process.Identity.Api/i18n/CurrentUserApiService.cs
src/EBL.FIG.Process.Identity.Api/Configuration/AuthenticationSetup.cs
src/EBL.FIG.Process.Identity.Api/Configuration/JwtSetup.cs
```

## Problemas atuais

### 1. Token depende de `UserEntity.TenantId`

No v2 `UserEntity` não terá `TenantId`. O token service deve receber contexto explícito.

### 2. AppId vem de `FirstOrDefault()` de UserRoles

Isso é incorreto no v2. O usuário pode ter várias roles em apps diferentes.

### 3. Roles/permissões podem vir de múltiplos tenants/apps

`GetByUserIdAsync(user.Id)` sem tenant/app pode contaminar o token.

### 4. Claims estão fora do padrão v2

O README recomenda `tenant_id`, `app_id`, `user_tenant_id`, etc.

## Novo contrato de emissão de token

Criar DTO/record em Application:

```csharp
public sealed record TokenIssueContext
{
    public int UserId { get; init; }
    public string UserName { get; init; }
    public string? Email { get; init; }
    public int TenantId { get; init; }
    public string TenantName { get; init; }
    public int UserTenantId { get; init; }
    public int AppId { get; init; }
    public string AppName { get; init; }
    public string? Audience { get; init; }
    public string AuthMethod { get; init; } // local | external
    public int? IdentityProviderId { get; init; }
    public string? IdentityProviderName { get; init; }
    public int? ExternalIdentityId { get; init; }
    public string? ExternalTenantId { get; init; }
    public string? ExternalObjectId { get; init; }
    public IReadOnlyCollection<string> Roles { get; init; } = [];
    public IReadOnlyCollection<string> Permissions { get; init; } = [];
}
```

Alterar interface:

```csharp
Task<TokenIssueResult> GenerateAccessTokenAsync(TokenIssueContext context, CancellationToken ct);
```

## Claims obrigatórias

Emitir:

```text
ver = 1.0
sub = Users.Id
name
email
tenant_id = Tenants.Id interno
tenant = Tenants.Name
user_tenant_id = UserTenants.Id
app_id = Apps.Id
app = Apps.Name
auth_method = local/external
jti
nbf
exp
iss
aud
roles
permissions
```

Para login externo, adicionar:

```text
idp
idp_id
external_identity_id
external_tenant_id
external_object_id
```

## Formato de permissions

O README recomenda:

```text
users:read
users:create
roles:update
dashboard:read
```

O código atual suporta `permissions` como JSON agrupado por resource. Para migração segura:

### Etapa 1 — Compatibilidade

Emitir ambos:

```text
permission = users:read  // múltiplas claims, compatibilidade simples
permissions = ["users:read", "users:create"] // claim JSON array ou array no payload
```

### Etapa 2 — Contrato final

Padronizar `permissions` como array no payload JWT.

Exemplo:

```json
"permissions": [
  "users:read",
  "users:create",
  "dashboard:read"
]
```

Atualizar `AuthorizationFilter` para aceitar:

1. múltiplas claims `permission`;
2. payload `permissions` como array;
3. temporariamente, JSON antigo agrupado por resource.

## JWT Keys

`JwtKeys` no v2 tem `AppId` opcional.

Atualizar `IJwtKeyDataRepository`:

```csharp
Task<JwtKeyEntity?> GetActiveKeyAsync(int tenantId, int? appId, CancellationToken ct);
```

Regra:

1. tentar chave ativa específica de tenant/app;
2. se não existir, tentar chave ativa do tenant com `AppId NULL`;
3. se não existir, erro operacional.

Atualizar uso do `kid` no header.

## RefreshToken v2

Atualizar `RefreshTokenEntity` e serviço.

### Issue

```csharp
Task<RefreshTokenIssueResult> IssueAsync(
    int tenantId,
    int appId,
    int userId,
    int? identityProviderId,
    int? externalIdentityId,
    CancellationToken ct);
```

### Rotate

Refresh deve validar:

```text
TokenHash
TenantId
AppId
UserId
ExpiresAt
RevokedAt == null
UserTenant ativo
App ativa
Usuário ativo
```

Request recomendado:

```csharp
public sealed record RefreshRequest(
    string RefreshToken,
    int TenantId,
    int AppId);
```

Não permitir refresh token de um tenant renovar sessão em outro tenant. Troca de tenant deve ser endpoint próprio.

## Cálculo de roles e permissions

Criar/usar serviço:

```csharp
IAuthorizationCalculationService.CalculateAsync(userId, tenantId, appId, ct)
```

Regras:

- considerar apenas `UserRoles` do tenant/app ativo;
- garantir `UserTenants` ativo;
- role ativa e não deletada;
- resource/action ativos e não deletados;
- permission pertence ao mesmo tenant/app.

Query ideal:

```text
UserRoles
  -> Roles
  -> RolePermissions
  -> Resources
  -> Actions
WHERE UserRoles.UserId = @userId
  AND UserRoles.TenantId = @tenantId
  AND UserRoles.AppId = @appId
```

## Atualizar `CurrentUserApiService`

Passar a ler prioritariamente claims v2:

```text
sub
user_id legado apenas fallback
tenant_id
app_id
user_tenant_id
```

Remover default silencioso `TenantId = 1` e `AppId = 1` para fluxos protegidos. Default só pode existir em ambiente Development e de forma explícita/configurada.

## Atualizar `AuthorizationFilter`

A autorização deve seguir deny-by-default:

- se endpoint exige role e token não tem role, negar;
- se endpoint exige permission e token não tem permission, negar;
- aceitar `roles` como array/payload e claims `role` por compatibilidade;
- aceitar `permissions` como array e claims `permission` por compatibilidade.

## Critérios de aceite

- Token não depende de `UserEntity.TenantId`.
- Token não usa `FirstOrDefault()` para app/role.
- Token contém `tenant_id`, `app_id`, `user_tenant_id`, `auth_method`.
- Permissions são calculadas apenas para tenant/app ativo.
- Refresh token exige tenant/app e não troca contexto indevidamente.
- AuthorizationFilter valida permissions v2 e mantém compatibilidade transitória.
- CurrentUserApiService não assume tenant/app 1 em produção.
