# 10 — Testes, Migração de Dados e Deploy

## Objetivo

Garantir que a migração para v2 seja validada tecnicamente, funcionalmente e operacionalmente antes de produção.

## 1. Testes unitários mínimos

Criar/expandir em:

```text
tests/EBL.FIG.Process.Identity.Tests/
```

### Domínio

```text
UserEntity_Create_GlobalUser_DoesNotRequireTenant
UserLocalCredential_RegisterFailedAccess_LocksAfterMaxAttempts
UserTenant_Deactivate_PreventsOperationalAccess
UserExternalIdentity_Key_IsProviderIssuerSubject
UserRole_Create_RequiresValidSource
```

### Application

```text
LocalLogin_ValidCredential_OneTenant_ReturnsAccessToken
LocalLogin_ValidCredential_MultipleTenants_ReturnsTenantSelection
LocalLogin_InvalidPassword_IncrementsAccessFailedCount
LocalLogin_LockedCredential_ReturnsLockedOut
SwitchTenant_ValidUserTenant_ReturnsNewToken
SwitchTenant_InvalidUserTenant_ReturnsForbidden
RefreshToken_ValidTenantApp_Rotates
RefreshToken_DifferentTenant_IsDenied
PasswordReset_LocalCredential_CreatesToken
PasswordReset_ExternalOnlyUser_IsDeniedOrRedirected
```

### Authorization

```text
AuthorizationCalculation_UsesOnlyTenantAndAppActive
AuthorizationCalculation_DoesNotLeakPermissionsFromOtherTenant
AuthorizationFilter_DeniesWhenPermissionMissing
AuthorizationFilter_AllowsPermissionArrayFormat
```

## 2. Testes de integração mínimos

Usar SQL Server real ou containerizado para validar RLS.

Cenários:

```text
RLS_TenantScoped_UserFromTenantA_CannotReadTenantBApps
RLS_UserGlobal_UserCanReadOwnUserRecord
RLS_UserTenant_UserCanSeeAssociatedTenantCatalog
RLS_SuperAdmin_CanReadAcrossTenants
RLS_BlockPredicate_PreventsInsertOutsideTenant
```

## 3. Testes de contrato de token

Validar access token emitido.

Claims obrigatórias:

```text
sub
tenant_id
user_tenant_id
app_id
auth_method
roles
permissions
jti
nbf
exp
iss
aud
```

Para externo:

```text
idp_id
external_identity_id
external_tenant_id
external_object_id
```

Validar que:

- `sub` é `Users.Id` interno;
- `tenant_id` é tenant interno;
- `external_tenant_id` não substitui `tenant_id`;
- permissions não incluem outro tenant/app.

## 4. Migração de dados v1 -> v2

O script `Create-Tables-v2-MultiTenant-Federated-Identity.sql` informa que é para base nova e não deve ser executado diretamente sobre v1. Se existir base v1 com dados, criar script separado.

### Estratégia segura

1. Criar backup da base v1.
2. Criar base v2 vazia com script v2.
3. Migrar dados por lotes.
4. Validar contagens e integridade.
5. Rodar testes de autenticação/autorização.
6. Trocar connection string apenas após validação.

### Mapeamento principal

#### `Tenants`

Copiar dados mantendo Id se possível ou criar tabela de correspondência.

#### `Apps`, `Roles`, `Resources`, `Actions`, `RolePermissions`

Copiar preservando TenantId/AppId e constraints.

#### `Users` v1 -> `Users` v2

Para cada usuário v1:

- criar ou reutilizar usuário global por login/e-mail normalizado;
- mover `Name`, `Email`, `UrlImage`, `LastAccessAt`, status e auditoria;
- não copiar `TenantId`, `PasswordHash`, `LoginIdentifier` para `Users`.

#### `UserLocalCredentials`

Para cada usuário v1 com senha:

```text
UserId novo
LoginIdentifier antigo
NormalizedLoginIdentifier antigo
PasswordHash antigo
AccessFailedCount = 0
IsActive = Users.IsActive
```

#### `UserTenants`

Para cada usuário v1:

```text
TenantId antigo
UserId novo
Source = Local ou Manual
IsDefault = 1 se era o único tenant do usuário
IsActive = Users.IsActive
IsDeleted = Users.IsDeleted
```

#### `UserRoles`

Copiar roles antigas apontando para `UserId` novo e preservando `TenantId/AppId/RoleId`.

Adicionar:

```text
Source = Manual
AddedBy
AddedOn
```

#### `RefreshTokens`

Se tokens antigos forem incompatíveis, recomendação segura: não migrar refresh tokens. Forçar novo login.

#### `PasswordResetTokens`

Não migrar tokens antigos. Forçar novo pedido de reset.

#### `JwtKeys`

Migrar chaves se o formato for compatível. Adicionar `AppId = NULL` inicialmente.

#### `JobDefinitions`

Adicionar `TenantId`. Para jobs globais, usar tenant Identity/técnico.

## 5. Validações pós-migração de dados

Queries de conferência:

```sql
-- Usuários globais sem credencial local são permitidos apenas se tiverem identidade externa.
SELECT u.Id, u.Name, u.Email
FROM dbo.Users u
LEFT JOIN dbo.UserLocalCredentials c ON c.UserId = u.Id
LEFT JOIN dbo.UserExternalIdentities e ON e.UserId = u.Id
WHERE c.UserId IS NULL AND e.UserId IS NULL;

-- UserRoles sem UserTenant não podem existir.
SELECT ur.*
FROM dbo.UserRoles ur
LEFT JOIN dbo.UserTenants ut
  ON ut.TenantId = ur.TenantId AND ut.UserId = ur.UserId
WHERE ut.Id IS NULL;

-- RefreshTokens sem UserTenant não podem existir.
SELECT rt.*
FROM dbo.RefreshTokens rt
LEFT JOIN dbo.UserTenants ut
  ON ut.TenantId = rt.TenantId AND ut.UserId = rt.UserId
WHERE ut.Id IS NULL;
```

## 6. Deploy

### Antes do deploy

- build verde;
- testes unitários verdes;
- testes integração/RLS verdes;
- script de migração testado em cópia da base;
- backup validado;
- secrets configurados;
- chave JWT ativa;
- Hangfire validado;
- Swagger revisado.

### Durante deploy

1. Colocar API em modo manutenção ou bloquear escrita se necessário.
2. Backup final.
3. Executar migração de schema/dados.
4. Executar validações SQL.
5. Subir API v2.
6. Rodar smoke tests.
7. Reabilitar tráfego.

### Smoke tests

```text
GET /health
POST /v1/auth/login usuário com 1 tenant
POST /v1/auth/login usuário com múltiplos tenants
POST /v1/auth/switch-tenant
POST /v1/auth/refresh
GET /v1/users protegido por token
GET /v1/authentication-events admin
```

## 7. Rollback

Rollback recomendado:

- manter base v1 intacta até validação v2;
- se migração for side-by-side, rollback é trocar connection string/API para v1;
- se migração for in-place, rollback exige restore de backup.

Preferência: side-by-side.

## Critérios de aceite

- Dados migrados sem UserRoles órfãs.
- Login local funciona após migração.
- Usuários com múltiplos tenants selecionam contexto.
- RLS bloqueia acesso indevido.
- Refresh antigo não compromete segurança.
- Jobs rodam com TenantId.
- Plano de rollback validado.
