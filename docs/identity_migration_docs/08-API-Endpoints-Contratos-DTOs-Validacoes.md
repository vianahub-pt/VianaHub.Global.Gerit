# 08 — API, Endpoints, Contratos, DTOs e Validações

## Objetivo

Atualizar a camada HTTP para expor o modelo v2 mantendo a organização por recurso existente. A aplicação já usa Minimal APIs por endpoint; a recomendação é preservar esse padrão.

## Endpoints existentes que continuam

```text
/v1/auth
/v1/users
/v1/tenants
/v1/apps
/v1/roles
/v1/resources
/v1/actions
/v1/role-permissions
/v1/user-roles
/v1/job-definitions
/v1/admin/tenants/{tenantId}/...
```

## Endpoints novos recomendados

```text
/v1/auth/available-tenants
/v1/auth/switch-tenant
/v1/auth/external/{tenantId}/{appId}/{providerId}/challenge
/v1/auth/external/callback
/v1/identity-providers
/v1/tenants/{tenantId}/apps/{appId}/identity-providers
/v1/users/{userId}/tenants
/v1/users/{userId}/external-identities
/v1/external-groups
/v1/tenant-external-group-role-mappings
/v1/authentication-events
```

## DTOs de autenticação

### `LoginRequest`

```csharp
public sealed record LoginRequest(
    string LoginIdentifier,
    string Password,
    int? TenantId = null,
    int? AppId = null);
```

Validação:

```text
LoginIdentifier obrigatório
Password obrigatório
TenantId > 0 se preenchido
AppId > 0 se preenchido
TenantId e AppId devem ser informados juntos para login direto em contexto específico
```

### `AuthDetailResponse`

Adicionar:

```text
RequiresTenantSelection
SelectionToken
AvailableTenants
UserTenantId
AuthMethod
Roles
Permissions
IdentityProviderId
ExternalIdentityId
```

### `SwitchTenantRequest`

```csharp
public sealed record SwitchTenantRequest(int TenantId, int AppId);
```

### `AvailableTenantResponse`

```csharp
public sealed record AvailableTenantResponse(
    int UserTenantId,
    int TenantId,
    string TenantName,
    string TenantAlias,
    bool IsDefault,
    IReadOnlyCollection<AvailableAppResponse> Apps);
```

## DTOs de usuário

### `CreateUserRequest` v2

Separar criação de usuário global, credencial e vínculo.

Opção recomendada:

```csharp
public sealed record CreateUserRequest(
    string Name,
    string? Email,
    string? UrlImage,
    CreateLocalCredentialRequest? LocalCredential,
    IReadOnlyCollection<CreateUserTenantRequest> Tenants);
```

```csharp
public sealed record CreateLocalCredentialRequest(
    string LoginIdentifier,
    string Password,
    bool MustChangePassword = false);
```

```csharp
public sealed record CreateUserTenantRequest(
    int TenantId,
    bool IsDefault = false,
    string Source = "Manual");
```

Para endpoints tenant-scoped, permitir request mais simples:

```text
POST /v1/admin/tenants/{tenantId}/users
```

O `tenantId` vem da rota e o request cria automaticamente o vínculo.

## DTOs de UserTenant

Criar:

```text
CreateUserTenantRequest
UpdateUserTenantRequest
UserTenantResponse
UserTenantDetailResponse
```

Endpoints:

```text
GET    /v1/users/{userId}/tenants
POST   /v1/users/{userId}/tenants
PATCH  /v1/users/{userId}/tenants/{tenantId}/activate
PATCH  /v1/users/{userId}/tenants/{tenantId}/deactivate
PATCH  /v1/users/{userId}/tenants/{tenantId}/default
DELETE /v1/users/{userId}/tenants/{tenantId}
```

## DTOs de providers

Criar requests/responses:

```text
CreateIdentityProviderRequest
UpdateIdentityProviderRequest
IdentityProviderResponse
IdentityProviderDetailResponse
CreateTenantIdentityProviderRequest
UpdateTenantIdentityProviderRequest
TenantIdentityProviderResponse
TenantIdentityProviderDetailResponse
```

Validações:

```text
ProviderType dentro dos valores suportados
Protocol dentro dos valores suportados
Configuration JSON válido
AllowedDomains JSON válido
ClaimMappings JSON válido
ClientSecret nunca retornado em response
```

## DTOs de UserExternalIdentity

Criar:

```text
UserExternalIdentityResponse
LinkExternalIdentityRequest
UnlinkExternalIdentityRequest
```

Não permitir criação manual insegura por e-mail apenas. Para vínculo manual, exigir provider, issuer e subject.

## DTOs de AuthenticationEvents

Criar filtro:

```csharp
public sealed record AuthenticationEventFilterRequest(
    int? TenantId,
    int? AppId,
    int? UserId,
    int? IdentityProviderId,
    string? EventType,
    string? Result,
    DateTime? From,
    DateTime? To,
    int? PageNumber,
    int? PageSize);
```

## Atualizar endpoints existentes

### `/v1/users`

Deve listar usuários globais acessíveis ao contexto atual. Para listagem por tenant, preferir:

```text
/v1/admin/tenants/{tenantId}/users
```

### `/v1/user-roles`

Adicionar validação obrigatória:

- `UserTenant` ativo existe;
- role pertence ao tenant/app;
- app pertence ao tenant;
- `Source` válido.

### `/v1/apps`

Adicionar `Audience`.

### `/v1/job-definitions`

Adicionar `TenantId` no contexto ou rota admin.

## Atualizar Swagger

Cada endpoint novo deve ter:

```text
WithTags
WithGroupName("v1")
WithName
WithSummary
Produces
WithValidation quando aplicável
CustomAuthorize quando protegido
```

Criar tags traduzidas para:

```text
IdentityProviders
TenantIdentityProviders
UserTenants
ExternalIdentities
ExternalGroups
AuthenticationEvents
```

## Localization

Atualizar:

```text
src/EBL.FIG.Process.Identity.Api/Localization/common.pt-PT.json
src/EBL.FIG.Process.Identity.Api/Localization/common.en-US.json
src/EBL.FIG.Process.Identity.Api/Localization/common.es-ES.json
```

Adicionar mensagens para:

```text
Auth.RequiresTenantSelection
Auth.InvalidTenantSelection
Auth.UserTenantNotFound
Auth.UserTenantInactive
Auth.AppNotAvailableForTenant
Auth.LocalCredentialNotFound
Auth.LocalCredentialLocked
Auth.ExternalProviderDisabled
Auth.ExternalIdentityNotLinked
Auth.ExternalAutoProvisioningDenied
Auth.SwitchTenant.Success
```

## Compatibilidade v1 temporária

Se houver frontend usando claims antigas, manter fallback por período curto:

```text
tenantId -> tenant_id
appId -> app_id
role -> roles
permission -> permissions
```

Mas novos responses devem usar v2.

## Critérios de aceite

- Swagger mostra recursos v2.
- DTOs não expõem hash, secrets ou private keys.
- Endpoints novos têm validação e autorização.
- `UserRoles` não aceita role sem `UserTenant` ativo.
- Auth retorna tenant selection quando aplicável.
- Localization atualizada em PT/EN/ES.
