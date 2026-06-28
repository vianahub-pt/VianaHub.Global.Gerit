# 07 — Federated Identity: Providers, External Identities e External Login

## Objetivo

Adicionar suporte incremental a autenticação federada/externa conforme o README e o SQL v2, sem tentar implementar todos os provedores em um único lote.

## Escopo inicial recomendado

Implementar primeiro a fundação:

```text
IdentityProviders
TenantIdentityProviders
UserExternalIdentities
ExternalGroups
TenantExternalGroupRoleMappings
interfaces de provider
contratos de callback/validação
```

Depois implementar provedores específicos:

```text
OIDC genérico
Microsoft Entra ID
Google/OAuth2
SAML2/ADFS em fase posterior
```

## Arquivos/pastas a criar

```text
src/EBL.FIG.Process.Identity.Application/Interfaces/IExternalAuthenticationAppService.cs
src/EBL.FIG.Process.Identity.Application/Services/ExternalAuthenticationAppService.cs
src/EBL.FIG.Process.Identity.Application/Interfaces/IIdentityProviderAppService.cs
src/EBL.FIG.Process.Identity.Application/Services/IdentityProviderAppService.cs
src/EBL.FIG.Process.Identity.Application/Interfaces/ITenantIdentityProviderAppService.cs
src/EBL.FIG.Process.Identity.Application/Services/TenantIdentityProviderAppService.cs
src/EBL.FIG.Process.Identity.Infra.Integration/IdentityProviders/
src/EBL.FIG.Process.Identity.Api/Endpoints/IdentityProviderEndpoint.cs
src/EBL.FIG.Process.Identity.Api/Endpoints/TenantIdentityProviderEndpoint.cs
src/EBL.FIG.Process.Identity.Api/Endpoints/ExternalAuthEndpoint.cs
```

## Modelo de providers

### `IdentityProviders`

Define o provider global:

```text
Name
ProviderType
Protocol
AuthorityUrl
MetadataUrl
Issuer
ClientId
ClientSecretEncrypted
ExternalTenantId
DomainHint
Configuration JSON
IsActive
IsDeleted
```

### `TenantIdentityProviders`

Habilita provider para tenant/app:

```text
TenantId
AppId
IdentityProviderId
IsDefault
IsEnabled
AutoProvisionUsers
AutoLinkByVerifiedEmail
DefaultRoleId
AllowedDomains JSON
ClaimMappings JSON
```

## Interfaces técnicas

Criar abstração para providers externos:

```csharp
public interface IExternalIdentityProviderClient
{
    string ProviderType { get; }
    Task<ExternalLoginChallengeResult> BuildChallengeAsync(TenantIdentityProviderEntity config, string redirectUri, CancellationToken ct);
    Task<ExternalTokenValidationResult> ValidateCallbackAsync(TenantIdentityProviderEntity config, ExternalCallbackRequest request, CancellationToken ct);
}
```

Resultado de validação:

```csharp
public sealed record ExternalTokenValidationResult
{
    public bool IsValid { get; init; }
    public string Issuer { get; init; }
    public string Subject { get; init; }
    public string? ExternalTenantId { get; init; }
    public string? ExternalObjectId { get; init; }
    public string? UserPrincipalName { get; init; }
    public string? Email { get; init; }
    public bool? EmailVerified { get; init; }
    public string? DisplayName { get; init; }
    public IReadOnlyCollection<string> ExternalGroupIds { get; init; } = [];
    public string? ErrorCode { get; init; }
    public string? ErrorDescription { get; init; }
}
```

## Endpoints recomendados

### Iniciar login externo

```text
GET /v1/auth/external/{tenantId}/{appId}/{providerId}/challenge
```

Retorna redirect/challenge ou URL de autenticação.

### Callback externo

```text
POST /v1/auth/external/callback
```

Recebe code/token/SAMLResponse conforme provider.

### Providers

```text
GET    /v1/identity-providers
GET    /v1/identity-providers/{id}
POST   /v1/identity-providers
PUT    /v1/identity-providers/{id}
PATCH  /v1/identity-providers/{id}/activate
PATCH  /v1/identity-providers/{id}/deactivate
DELETE /v1/identity-providers/{id}
```

### Tenant providers

```text
GET    /v1/tenants/{tenantId}/apps/{appId}/identity-providers
POST   /v1/tenants/{tenantId}/apps/{appId}/identity-providers
PUT    /v1/tenants/{tenantId}/apps/{appId}/identity-providers/{id}
PATCH  /v1/tenants/{tenantId}/apps/{appId}/identity-providers/{id}/enable
PATCH  /v1/tenants/{tenantId}/apps/{appId}/identity-providers/{id}/disable
```

## Fluxo de login externo

1. Validar `TenantIdentityProvider` ativo para `tenantId/appId/providerId`.
2. Redirecionar ou validar callback.
3. Validar assinatura, issuer, audience, expiração e claims obrigatórias no provider.
4. Buscar `UserExternalIdentities` por:

```text
IdentityProviderId + Issuer + Subject
```

5. Se existir:

```text
validar User ativo
validar UserExternalIdentity ativa
validar UserTenant ativo para tenant
atualizar LastLoginAt
calcular roles/permissões
emitir token interno
```

6. Se não existir e `AutoProvisionUsers = true`:

```text
validar AllowedDomains
criar Users
criar UserExternalIdentities
criar UserTenants
atribuir DefaultRoleId se configurada
mapear grupos externos para roles se configurado
emitir token interno
```

7. Se não existir e `AutoLinkByVerifiedEmail = true`:

```text
validar EmailVerified == true
buscar User.NormalizedEmail
criar vínculo UserExternalIdentity
criar UserTenant se permitido
emitir token interno
```

8. Caso contrário, negar acesso e auditar.

## Auto provisionamento

Regras obrigatórias:

- só executar se `TenantIdentityProvider.AutoProvisionUsers = true`;
- respeitar `AllowedDomains` se preenchido;
- aplicar `ClaimMappings` para `Name`, `Email`, `DisplayName`;
- criar `UserTenant` com `Source = Provisioning` ou `ExternalLogin`;
- aplicar `DefaultRoleId` apenas se a role pertence ao mesmo tenant/app;
- registrar `AuthenticationEvents`.

## Mapeamento de grupos externos

Fluxo:

1. Provider retorna IDs de grupos externos.
2. Buscar `ExternalGroups` por `IdentityProviderId + ExternalGroupId`.
3. Buscar `TenantExternalGroupRoleMappings` ativos por tenant/app/provider/grupo.
4. Criar `UserRoles` com `Source = GroupMapping`.
5. Remover ou inativar roles antigas de group mapping que não aparecem mais, se essa política for habilitada.

## Segurança

- Não armazenar `ClientSecret` puro.
- `ClientSecretEncrypted` deve usar provider seguro (`ISecretProvider`, Key Vault ou equivalente).
- Não confiar em e-mail sem `EmailVerified`.
- Não aceitar issuer diferente do configurado.
- Não misturar tenant externo com tenant interno.
- Auditar todos os callbacks com sucesso/falha.

## Critérios de aceite

- CRUD de providers funciona.
- Provider só autentica quando habilitado para tenant/app.
- Vínculo externo usa issuer/subject.
- Auto provisionamento respeita domínios permitidos.
- Auto-link por e-mail só ocorre com e-mail verificado e configuração habilitada.
- Token interno é emitido pela Identity API, não pelo provider externo.
- Eventos de autenticação externa são auditados.
