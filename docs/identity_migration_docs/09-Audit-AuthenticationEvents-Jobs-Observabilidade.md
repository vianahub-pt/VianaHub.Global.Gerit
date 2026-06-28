# 09 — Auditoria, AuthenticationEvents, Jobs e Observabilidade

## Objetivo

Adicionar rastreabilidade obrigatória dos eventos de autenticação e ajustar jobs ao modelo v2 tenant-scoped/federated.

## Tabela `AuthenticationEvents`

Campos principais:

```text
Id BIGINT
TenantId NULL
AppId NULL
UserId NULL
IdentityProviderId NULL
EventType
Result
Issuer
Subject
ExternalTenantId
ExternalObjectId
LoginIdentifier
IpAddress
UserAgent
ErrorCode
ErrorDescription
AddedOn
```

## Serviço de auditoria

Criar:

```text
src/EBL.FIG.Process.Identity.Application/Interfaces/IAuthenticationAuditAppService.cs
src/EBL.FIG.Process.Identity.Application/Services/AuthenticationAuditAppService.cs
```

Ou, se preferir manter no domínio:

```text
IAuthenticationAuditDomainService
AuthenticationAuditDomainService
```

Contrato recomendado:

```csharp
Task RegisterAsync(AuthenticationAuditRequest request, CancellationToken ct);
```

Request:

```csharp
public sealed record AuthenticationAuditRequest
{
    public int? TenantId { get; init; }
    public int? AppId { get; init; }
    public int? UserId { get; init; }
    public int? IdentityProviderId { get; init; }
    public string EventType { get; init; }
    public string Result { get; init; }
    public string? Issuer { get; init; }
    public string? Subject { get; init; }
    public string? ExternalTenantId { get; init; }
    public string? ExternalObjectId { get; init; }
    public string? LoginIdentifier { get; init; }
    public string? IpAddress { get; init; }
    public string? UserAgent { get; init; }
    public string? ErrorCode { get; init; }
    public string? ErrorDescription { get; init; }
}
```

## Eventos mínimos a auditar

```text
LocalLogin.Success
LocalLogin.InvalidCredentials
LocalLogin.LockedOut
LocalLogin.UserWithoutTenant
LocalLogin.UserWithoutRole
ExternalLogin.Success
ExternalLogin.InvalidCallback
ExternalLogin.ProviderDisabled
ExternalLogin.ExternalIdentityNotLinked
ExternalLogin.AutoProvisioned
RefreshToken.Success
RefreshToken.Invalid
RefreshToken.Revoked
Logout.Success
TenantSwitch.Success
TenantSwitch.Denied
PasswordReset.Requested
PasswordReset.Completed
PasswordReset.InvalidToken
Authorization.AccessDenied
```

## Cuidados de segurança em logs/auditoria

Não gravar:

```text
senha
refresh token puro
access token completo
client secret puro
private key
SAML assertion completa
authorization code completo
```

Pode gravar:

```text
hash parcial seguro
jti
kid
issuer
subject
external object id
ip
user agent
erro técnico controlado
```

## Jobs existentes a ajustar

Pasta atual:

```text
src/EBL.FIG.Process.Identity.Infra.Job/
```

Jobs identificados:

```text
CleanupExpiredJwtKeysJob
ScheduledSyncJobDefinitionsJob
SyncJobDefinitionsJob
JwtKeyRotationJob
ReconcileJwtKeysJob
```

## `JobDefinitions` v2

A tabela agora tem `TenantId`. Atualizar:

- `JobDefinitionEntity`;
- `JobDefinitionMapping`;
- `JobDefinitionDataRepository`;
- `JobAppService`;
- `JobSyncService`;
- seeders;
- endpoints.

Jobs podem ser:

```text
System/global -> executados como superadmin, mas registrados para TenantId Identity ou tenant técnico
Tenant-scoped -> executados por tenant específico com RequestTenantContext
```

## Jobs novos recomendados

### 1. `CleanupExpiredRefreshTokensJob`

Remove/arquiva refresh tokens expirados e revogados antigos.

### 2. `CleanupPasswordResetTokensJob`

Remove tokens expirados/usados antigos.

### 3. `CleanupAuthenticationEventsJob`

Arquiva ou remove eventos antigos conforme política de retenção.

### 4. `SyncExternalGroupsJob`

Sincroniza grupos externos para providers que suportarem. Pode ser fase 2 da federação.

### 5. `ReconcileUserRolesFromExternalGroupsJob`

Recalcula roles oriundas de group mapping.

## Contexto de execução de jobs

Antes de acessar banco com RLS, jobs devem setar contexto:

```csharp
_requestTenantContext.SetContext(userId: systemUserId, tenantId: tenantId, appId: appId);
```

Para operações globais controladas:

```csharp
_adminTenantContext.Activate(adminUserId: systemUserId, targetTenantId: tenantId, targetAppId: appId);
```

Nunca deixar job tenant-scoped sem contexto; RLS deve bloquear.

## Observabilidade

Adicionar logs estruturados nos fluxos:

```text
LoginAttempt
LoginSucceeded
LoginFailed
TenantSelectionRequired
TenantSwitched
RefreshRotated
RefreshRevoked
ExternalProviderCallbackReceived
ExternalIdentityLinked
AutoProvisioningDenied
PermissionDenied
```

Campos úteis:

```text
CorrelationId
UserId
TenantId
AppId
IdentityProviderId
EventType
Result
IpAddress
UserAgent
```

## Health checks

Manter health check do SQL Server. Adicionar, se possível:

```text
Hangfire storage disponível
Chave JWT ativa por tenant/app crítico
Provider externo metadata endpoint disponível
```

## Critérios de aceite

- `AuthenticationEvents` recebe eventos de login, refresh, logout, troca de tenant e falhas.
- Jobs possuem `TenantId` e contexto de RLS definido.
- Tokens/secrets não são gravados em logs.
- Há endpoint administrativo para consultar eventos com paginação/filtro.
- Retenção/limpeza de eventos e tokens está definida.
