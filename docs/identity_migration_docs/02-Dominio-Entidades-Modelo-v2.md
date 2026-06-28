# 02 — Domínio: Entidades e Modelo v2

## Objetivo

Refatorar o domínio para refletir o schema v2, sem ainda obrigar todos os fluxos da aplicação a usarem o novo modelo. Esta etapa deve compilar isoladamente e preparar a base para EF, repositórios e autenticação v2.

## Mudança principal

O usuário deixa de ser tenant-scoped e passa a ser global.

### Antes

```csharp
public class UserEntity : Entity
{
    public int TenantId { get; private set; }
    public string? LoginIdentifier { get; private set; }
    public string? NormalizedLoginIdentifier { get; private set; }
    public string? PasswordHash { get; private set; }
    public string? Email { get; private set; }
}
```

### Depois

```csharp
public class UserEntity : Entity, IAggregateRoot
{
    public string Name { get; private set; }
    public string? Email { get; private set; }
    public string? NormalizedEmail { get; private set; }
    public string? UrlImage { get; private set; }
    public DateTime? LastAccessAt { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    public UserLocalCredentialEntity? LocalCredential { get; private set; }
    public IReadOnlyCollection<UserTenantEntity> UserTenants => _userTenants.AsReadOnly();
    public IReadOnlyCollection<UserRoleEntity> UserRoles => _userRoles.AsReadOnly();
    public IReadOnlyCollection<UserExternalIdentityEntity> ExternalIdentities => _externalIdentities.AsReadOnly();
}
```

## Entidades novas a criar

Criar arquivos em:

```text
src/EBL.FIG.Process.Identity.Domain/Entities/
```

### 1. `UserLocalCredentialEntity.cs`

Representa credencial local.

Campos:

```text
UserId
LoginIdentifier
NormalizedLoginIdentifier
PasswordHash
AccessFailedCount
LockoutUntilAt
PasswordChangedAt
MustChangePassword
IsActive
AddedBy
AddedOn
ModifiedBy
ModifiedAt
```

Métodos recomendados:

```csharp
RegisterFailedAccess(int modifiedBy, int maxFailures, TimeSpan lockoutDuration)
ResetFailedAccess(int modifiedBy)
ChangePassword(string passwordHash, int modifiedBy, bool mustChangePassword = false)
Activate(int modifiedBy)
Deactivate(int modifiedBy)
IsLocked(DateTime utcNow)
CanAuthenticate(DateTime utcNow)
```

Regras:

- `UserId` é chave primária e FK para `Users`.
- `NormalizedLoginIdentifier` é único globalmente.
- Não deve existir senha em `UserEntity`.
- Deve suportar lockout por falhas.

### 2. `UserTenantEntity.cs`

Representa o vínculo entre usuário global e tenant.

Campos:

```text
Id
TenantId
UserId
Source
TenantIdentityProviderId
UserExternalIdentityId
IsDefault
IsActive
IsDeleted
AddedBy
AddedOn
ModifiedBy
ModifiedAt
```

Criar enum ou constantes para `Source`:

```text
Manual
Local
ExternalLogin
Invitation
Provisioning
GroupMapping
```

Métodos:

```csharp
SetDefault(int modifiedBy)
Activate(int modifiedBy)
Deactivate(int modifiedBy)
Delete(int modifiedBy)
```

Regras:

- Um vínculo é único por `TenantId + UserId`.
- Usuário só pode receber role em tenant se houver `UserTenant` ativo.

### 3. `IdentityProviderEntity.cs`

Campos:

```text
Id
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
Configuration
IsActive
IsDeleted
AddedBy
AddedOn
ModifiedBy
ModifiedAt
```

Valores permitidos de `ProviderType`:

```text
Local
MicrosoftEntraId
MicrosoftEntraB2B
MicrosoftEntraB2C
MicrosoftEntraExternalId
AdFs
ActiveDirectory
Oidc
Saml2
Google
Okta
Auth0
```

Valores permitidos de `Protocol`:

```text
Local
OpenIdConnect
OAuth2
Saml2
Ldap
Kerberos
```

Métodos:

```csharp
UpdateConfiguration(...)
SetClientSecretEncrypted(string encryptedSecret, int modifiedBy)
Activate(int modifiedBy)
Deactivate(int modifiedBy)
Delete(int modifiedBy)
```

### 4. `TenantIdentityProviderEntity.cs`

Campos:

```text
Id
TenantId
AppId
IdentityProviderId
IsDefault
IsEnabled
AutoProvisionUsers
AutoLinkByVerifiedEmail
DefaultRoleId
AllowedDomains
ClaimMappings
AddedBy
AddedOn
ModifiedBy
ModifiedAt
```

Regras:

- O provider só pode autenticar no tenant/app quando `IsEnabled = true`.
- Só pode haver um default ativo por tenant/app.
- `AllowedDomains` e `ClaimMappings` devem ser JSON válido quando preenchidos.

### 5. `UserExternalIdentityEntity.cs`

Campos:

```text
Id
UserId
IdentityProviderId
Issuer
Subject
ExternalTenantId
ExternalObjectId
UserPrincipalName
Email
NormalizedEmail
EmailVerified
DisplayName
FirstLoginAt
LastLoginAt
IsActive
IsDeleted
AddedBy
AddedOn
ModifiedBy
ModifiedAt
```

Regras:

- Chave confiável: `IdentityProviderId + Issuer + Subject`.
- E-mail nunca deve ser a única chave de autenticação federada.
- `LastLoginAt` deve ser atualizado a cada login externo válido.

### 6. `ExternalGroupEntity.cs`

Campos:

```text
Id
IdentityProviderId
ExternalGroupId
DisplayName
Description
IsActive
IsDeleted
AddedBy
AddedOn
ModifiedBy
ModifiedAt
```

### 7. `TenantExternalGroupRoleMappingEntity.cs`

Campos:

```text
Id
TenantId
AppId
TenantIdentityProviderId
IdentityProviderId
ExternalGroupId
RoleId
IsActive
IsDeleted
AddedBy
AddedOn
ModifiedBy
ModifiedAt
```

Regras:

- Mapeia grupo externo para role interna.
- Deve respeitar tenant, app e provider.

### 8. `AuthenticationEventEntity.cs`

Campos:

```text
Id long
TenantId nullable
AppId nullable
UserId nullable
IdentityProviderId nullable
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

Eventos sugeridos:

```text
LocalLogin
ExternalLogin
RefreshToken
Logout
TenantSwitch
PasswordResetRequested
PasswordResetCompleted
ExternalIdentityLinked
AutoProvisioning
AccessDenied
```

Resultados sugeridos:

```text
Success
Failure
Denied
LockedOut
Expired
Revoked
```

## Entidades existentes a ajustar

### `AppEntity`

Adicionar:

```csharp
public string? Audience { get; private set; }
```

Atualizar construtor e método `Update` para aceitar `audience`.

### `UserRoleEntity`

Adicionar:

```csharp
public string Source { get; private set; }
public int? TenantExternalGroupRoleMappingId { get; private set; }
```

Valores de source:

```text
Manual
DefaultRole
GroupMapping
Provisioning
```

Alterar construtor:

```csharp
public UserRoleEntity(int tenantId, int appId, int userId, int roleId, string source, int createdBy, int? groupMappingId = null)
```

Adicionar `AddedBy`, `AddedOn`, `ModifiedBy`, `ModifiedAt` se ainda não existirem na entidade.

### `RolePermissionEntity`

O SQL v2 continua sem auditoria para `RolePermissions`. Pode permanecer como entidade simples. Avaliar se deve herdar `Entity` apenas se o banco for alterado; não inventar campos que não existem no SQL.

### `RefreshTokenEntity`

Adicionar:

```csharp
public int? IdentityProviderId { get; private set; }
public int? ExternalIdentityId { get; private set; }
```

Construtor deve aceitar esses campos opcionais.

### `PasswordResetTokenEntity`

Remover `TenantId` da entidade. A tabela v2 tem apenas `UserId`, pois reset só é permitido para usuário com credencial local.

### `JwtKeyEntity`

Adicionar:

```csharp
public int? AppId { get; private set; }
```

A chave pode ser por tenant ou por tenant/app.

### `JobDefinitionEntity`

Adicionar:

```csharp
public int TenantId { get; private set; }
```

Jobs passam a ser tenant-scoped no SQL v2.

## Validadores de domínio

Atualizar/criar validadores em:

```text
src/EBL.FIG.Process.Identity.Domain/Validators/
```

Novos validadores:

```text
UserLocalCredentialValidator
UserTenantValidator
IdentityProviderValidator
TenantIdentityProviderValidator
UserExternalIdentityValidator
ExternalGroupValidator
TenantExternalGroupRoleMappingValidator
AuthenticationEventValidator
```

Ajustar `UserValidator`:

- remover validação de `TenantId`;
- remover validação de `PasswordHash`;
- validar `Name`;
- validar `Email` se preenchido;
- validar `NormalizedEmail` quando `Email` existir.

## Interfaces de domínio a criar

Criar em:

```text
src/EBL.FIG.Process.Identity.Domain/Interfaces/
```

```text
IUserLocalCredentialDataRepository
IUserTenantDataRepository
IIdentityProviderDataRepository
ITenantIdentityProviderDataRepository
IUserExternalIdentityDataRepository
IExternalGroupDataRepository
ITenantExternalGroupRoleMappingDataRepository
IAuthenticationEventDataRepository
```

Serviços de domínio sugeridos:

```text
IUserLocalCredentialDomainService
IUserTenantDomainService
IIdentityProviderDomainService
IExternalIdentityDomainService
IAuthenticationAuditDomainService
```

## Cuidados importantes

- Não deixar `UserEntity` com senha por compatibilidade. A compatibilidade deve estar nos AppServices/adapters, não na entidade global.
- Não usar e-mail como chave primária lógica para federação.
- Não usar `FirstOrDefault()` de roles para descobrir app ativa.
- Não misturar `TenantId` externo do Entra com `TenantId` interno da aplicação.

## Critérios de aceite

- O projeto `Domain` compila.
- `UserEntity` está global.
- Novas entidades representam todas as tabelas novas do SQL v2.
- Entidades alteradas possuem campos equivalentes ao SQL v2.
- Validadores não exigem campos removidos do schema.
- Nenhum fluxo de autenticação precisa estar funcionando ainda nesta etapa.
