# 05 — Autenticação Local, UserTenants e Troca de Tenant

## Objetivo

Refatorar o fluxo de autenticação local para o modelo v2, onde o usuário é global e pode estar associado a múltiplos tenants. Esta etapa implementa os fluxos 7.1 e 7.3 do README.

## Arquivos impactados

```text
src/EBL.FIG.Process.Identity.Application/Services/AuthAppService.cs
src/EBL.FIG.Process.Identity.Application/Services/ForgotPasswordAppService.cs
src/EBL.FIG.Process.Identity.Application/Services/UserAppService.cs
src/EBL.FIG.Process.Identity.Application/Interfaces/IAuthAppService.cs
src/EBL.FIG.Process.Identity.Application/Dto/Request/Auth/*.cs
src/EBL.FIG.Process.Identity.Application/Dto/Response/Auth/*.cs
src/EBL.FIG.Process.Identity.Api/Endpoints/AuthEndpoint.cs
src/EBL.FIG.Process.Identity.Api/Validations/Auth/*.cs
```

## Problema atual

O fluxo atual faz:

```text
LoginIdentifier -> Tenant -> User dentro do Tenant -> PasswordHash em Users -> First UserRole -> Token
```

No v2 o fluxo deve ser:

```text
LoginIdentifier -> UserLocalCredentials -> User global -> UserTenants ativos -> seleção Tenant/App -> Roles/Permissions -> Token
```

## Novo fluxo de login local

### Entrada recomendada

`LoginRequest` deve evoluir para:

```csharp
public sealed record LoginRequest(
    string LoginIdentifier,
    string Password,
    int? TenantId = null,
    int? AppId = null);
```

`TenantId` e `AppId` são opcionais porque o usuário pode ter um ou múltiplos contextos.

### Resultado recomendado

`AuthDetailResponse` deve suportar dois cenários:

#### 1. Login completo com token

```json
{
  "requiresTenantSelection": false,
  "accessToken": "...",
  "refreshToken": "...",
  "tenantId": 1,
  "appId": 1,
  "userId": 10,
  "roles": ["backoffice"],
  "permissions": ["users:read", "dashboard:read"]
}
```

#### 2. Login válido, mas precisa escolher tenant/app

```json
{
  "requiresTenantSelection": true,
  "selectionToken": "curto-ou-transitório",
  "userId": 10,
  "availableTenants": [
    {
      "tenantId": 1,
      "tenantName": "Identity",
      "isDefault": true,
      "apps": [
        { "appId": 1, "appName": "Identity" }
      ]
    }
  ]
}
```

O `selectionToken` pode ser JWT curto com `sub`, `auth_stage=tenant_selection`, sem permissões operacionais. Alternativamente, pode-se exigir que o cliente reenvie credenciais com tenant/app; porém isso é pior para UX.

## Passo a passo do login local v2

### Passo 1 — Normalizar login

```csharp
var normalizedLogin = request.LoginIdentifier.Trim().ToUpperInvariant();
```

### Passo 2 — Buscar credencial local

```csharp
var credential = await _localCredentialRepo.GetByNormalizedLoginAsync(normalizedLogin, ct);
```

Validar:

```text
credential != null
credential.IsActive == true
credential.User.IsActive == true
credential.User.IsDeleted == false
credential não está locked out
```

### Passo 3 — Verificar senha

```csharp
if (!VerifyClientSecret(credential.PasswordHash, request.Password))
{
    credential.RegisterFailedAccess(...);
    await audit.Failure(...);
    return 401;
}
```

Ao sucesso:

```csharp
credential.ResetFailedAccess(...);
user.UpdateLastAccess();
```

### Passo 4 — Buscar tenants ativos

```csharp
var userTenants = await _userTenantRepo.GetActiveTenantsByUserAsync(user.Id, ct);
```

Validar:

```text
pelo menos 1 vínculo ativo
Tenant ativo e não deletado
```

### Passo 5 — Resolver tenant/app

Cenários:

#### A. Request informa TenantId/AppId

Validar se:

```text
UserTenant ativo existe
App existe no Tenant
App está ativa
Provider local está habilitado no Tenant/App, se a regra for aplicada
```

#### B. Request não informa TenantId/AppId e existe apenas um contexto possível

Selecionar automaticamente.

#### C. Request não informa TenantId/AppId e existem múltiplos contextos

Retornar `RequiresTenantSelection = true`.

### Passo 6 — Calcular roles/permissões

Chamar serviço específico:

```csharp
var authorization = await _authorizationCalculation.CalculateAsync(user.Id, tenantId, appId, ct);
```

Se não houver role/permissão, negar por padrão ou retornar contexto sem acesso, conforme regra de produto. Recomendação: negar login operacional com 403 para app/tenant sem role.

### Passo 7 — Emitir token

Chamar token service com contexto explícito:

```csharp
await _jwtTokenService.GenerateAccessTokenAsync(new TokenIssueContext
{
    UserId = user.Id,
    TenantId = tenantId,
    AppId = appId,
    UserTenantId = userTenant.Id,
    AuthMethod = "local",
    Roles = authorization.Roles,
    Permissions = authorization.Permissions
}, ct);
```

### Passo 8 — Emitir refresh token

```csharp
await _refreshTokenService.IssueAsync(tenantId, appId, user.Id, ct);
```

### Passo 9 — Auditar

Registrar `AuthenticationEvents`:

```text
EventType = LocalLogin
Result = Success/Failure/LockedOut/Denied
TenantId/AppId/UserId quando resolvidos
LoginIdentifier
IpAddress
UserAgent
```

## Endpoint de tenants disponíveis

Criar endpoint:

```text
GET /v1/auth/available-tenants
```

Uso:

- após login parcial;
- ou para usuário autenticado listar tenants associados.

Resposta:

```json
{
  "userId": 10,
  "tenants": [
    {
      "userTenantId": 5,
      "tenantId": 1,
      "tenantName": "Identity",
      "tenantAlias": "identity",
      "isDefault": true,
      "apps": [
        { "appId": 1, "name": "Identity", "audience": "EBL.FIG.Process.Identity.Api" }
      ]
    }
  ]
}
```

## Endpoint de troca de tenant

Criar endpoint:

```text
POST /v1/auth/switch-tenant
```

Request:

```csharp
public sealed record SwitchTenantRequest(int TenantId, int AppId);
```

Regras:

- usuário precisa estar autenticado;
- validar `UserTenants` ativo para `UserId + TenantId`;
- validar App ativa;
- recalcular roles/permissões;
- emitir novo access token e, preferencialmente, novo refresh token;
- auditar `TenantSwitch`.

## Register v2

O endpoint `/v1/auth/register` hoje cria usuário dentro do tenant. No v2 há duas opções:

### Opção recomendada para backoffice

Mover criação de usuário para `/v1/users` ou `/v1/admin/.../users`, criando:

```text
Users
UserLocalCredentials opcional
UserTenants
UserRoles opcional
```

### Opção pública controlada

Permitir register apenas quando um tenant/app/provider local permitir auto cadastro. Isso exige configuração clara e normalmente não deve ser default.

## Forgot/reset password v2

`ForgotPasswordAppService` hoje resolve tenant por login identifier e usa `PasswordResetTokenEntity(tenantId, userId, ...)`.

Alterar para:

1. buscar `UserLocalCredentials` por login normalizado;
2. se não existir, retornar resposta genérica sem revelar conta;
3. se usuário só tiver provider externo, orientar recuperação no provider;
4. criar `PasswordResetToken` com `UserId`, sem `TenantId`;
5. enviar email para `User.Email` ou `credential.LoginIdentifier`, conforme regra;
6. no reset, alterar `UserLocalCredentials.PasswordHash`, não `Users.PasswordHash`.

## Lockout por falha

Implementar política configurável:

```json
"LocalAuthentication": {
  "MaxFailedAccessAttempts": 5,
  "LockoutMinutes": 15
}
```

Campos usados:

```text
AccessFailedCount
LockoutUntilAt
```

## Cuidados

- Não devolver se login existe ou não em forgot password.
- Não emitir token com roles de outro tenant/app.
- Não usar e-mail como substituto de login sem normalização.
- Não usar `TenantId` default `1` em autenticação real.
- Não selecionar app via primeira role encontrada.

## Critérios de aceite

- Login local usa `UserLocalCredentials`.
- Usuário sem tenant ativo não autentica operacionalmente.
- Usuário com um único tenant/app recebe token direto.
- Usuário com múltiplos tenants recebe lista para seleção ou consegue informar tenant/app.
- Troca de tenant emite novo token.
- Password reset altera apenas credencial local.
- Falhas de login incrementam contador e podem bloquear temporariamente.
- Todos os eventos são auditados.
