# 04 — Repositórios, Queries e Serviços de Domínio

## Objetivo

Atualizar a camada de acesso a dados para o modelo v2, criando queries específicas para autenticação, seleção de tenant/app e cálculo de autorização. Esta etapa evita que regras críticas fiquem espalhadas dentro de AppServices.

## Repositórios novos

Criar em:

```text
src/EBL.FIG.Process.Identity.Infra.Data/Repository/
```

```text
UserLocalCredentialDataRepository.cs
UserTenantDataRepository.cs
IdentityProviderDataRepository.cs
TenantIdentityProviderDataRepository.cs
UserExternalIdentityDataRepository.cs
ExternalGroupDataRepository.cs
TenantExternalGroupRoleMappingDataRepository.cs
AuthenticationEventDataRepository.cs
```

Criar interfaces correspondentes em:

```text
src/EBL.FIG.Process.Identity.Domain/Interfaces/
```

## 1. `IUserLocalCredentialDataRepository`

Métodos mínimos:

```csharp
Task<UserLocalCredentialEntity?> GetByNormalizedLoginAsync(string normalizedLoginIdentifier, CancellationToken ct);
Task<UserLocalCredentialEntity?> GetByUserIdAsync(int userId, CancellationToken ct);
Task<bool> ExistsByNormalizedLoginAsync(string normalizedLoginIdentifier, CancellationToken ct);
Task CreateAsync(UserLocalCredentialEntity entity, CancellationToken ct);
Task UpdateAsync(UserLocalCredentialEntity entity, CancellationToken ct);
```

Inclua `User` quando necessário:

```csharp
.Include(x => x.User)
```

A query de login local deve trazer usuário global ativo e não excluído.

## 2. `IUserTenantDataRepository`

Métodos mínimos:

```csharp
Task<UserTenantEntity?> GetActiveAsync(int userId, int tenantId, CancellationToken ct);
Task<IList<UserTenantEntity>> GetActiveTenantsByUserAsync(int userId, CancellationToken ct);
Task<UserTenantEntity?> GetDefaultTenantAsync(int userId, CancellationToken ct);
Task<bool> ExistsActiveAsync(int userId, int tenantId, CancellationToken ct);
Task CreateAsync(UserTenantEntity entity, CancellationToken ct);
Task UpdateAsync(UserTenantEntity entity, CancellationToken ct);
```

Query deve incluir:

```csharp
.Include(x => x.Tenant)
.Include(x => x.TenantIdentityProvider)
```

## 3. `ITenantIdentityProviderDataRepository`

Métodos mínimos:

```csharp
Task<TenantIdentityProviderEntity?> GetEnabledAsync(int tenantId, int appId, int identityProviderId, CancellationToken ct);
Task<TenantIdentityProviderEntity?> GetDefaultAsync(int tenantId, int appId, CancellationToken ct);
Task<IList<TenantIdentityProviderEntity>> GetEnabledByTenantAppAsync(int tenantId, int appId, CancellationToken ct);
```

## 4. `IUserExternalIdentityDataRepository`

Métodos mínimos:

```csharp
Task<UserExternalIdentityEntity?> GetByIssuerSubjectAsync(int identityProviderId, string issuer, string subject, CancellationToken ct);
Task<IList<UserExternalIdentityEntity>> GetByUserIdAsync(int userId, CancellationToken ct);
Task<UserExternalIdentityEntity?> GetByVerifiedEmailAsync(int identityProviderId, string normalizedEmail, CancellationToken ct);
Task CreateAsync(UserExternalIdentityEntity entity, CancellationToken ct);
Task UpdateAsync(UserExternalIdentityEntity entity, CancellationToken ct);
```

## 5. `IAuthenticationEventDataRepository`

Métodos mínimos:

```csharp
Task AddAsync(AuthenticationEventEntity entity, CancellationToken ct);
Task<ListPage<AuthenticationEventEntity>> GetPagedAsync(AuthenticationEventFilter filter, CancellationToken ct);
```

Eventos de falha podem não ter `UserId`/`TenantId`; repositório deve suportar campos nulos.

## Repositórios existentes a ajustar

### `UserDataRepository`

Remover métodos tenant-scoped antigos ou substituir por versões explícitas.

Antes:

```csharp
GetByIdAsync(int tenantId, int id)
GetByNormalizedLoginAsync(int tenantId, string login)
ExistsByNameAsync(int tenantId, string name)
```

Depois:

```csharp
Task<UserEntity?> GetByIdAsync(int id, CancellationToken ct);
Task<UserEntity?> GetByIdWithTenantsAsync(int id, CancellationToken ct);
Task<UserEntity?> GetByIdWithRolesAsync(int userId, int tenantId, int appId, CancellationToken ct);
Task<bool> ExistsByEmailAsync(string normalizedEmail, CancellationToken ct);
Task<ListPage<UserEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
```

Observação: para listagem administrativa por tenant, consultar `UserTenants` e projetar usuários associados ao tenant.

### `UserRoleDataRepository`

Adicionar tenant/app sempre nas queries por usuário:

```csharp
Task<IList<UserRoleEntity>> GetByUserTenantAppAsync(int userId, int tenantId, int appId, CancellationToken ct);
Task<bool> ExistsAsync(int tenantId, int appId, int userId, int roleId, CancellationToken ct);
```

A query atual `GetByUserIdAsync(int userId)` é perigosa para token v2 porque pode trazer roles de todos os tenants/apps. Manter apenas para administração explícita, nunca para emissão de token.

### `RolePermissionDataRepository`

Garantir método:

```csharp
Task<IList<RolePermissionEntity>> GetByRolesAsync(int tenantId, int appId, IEnumerable<int> roleIds, CancellationToken ct);
```

Evitar N+1 de permissões por role no `JwtTokenService`.

### `RefreshTokenDataRepository`

Métodos v2:

```csharp
Task<RefreshTokenEntity?> GetByTokenHashAsync(byte[] tokenHash, int tenantId, int appId, CancellationToken ct);
Task<int> RevokeAllByUserTenantAppAsync(int userId, int tenantId, int appId, int revokedBy, CancellationToken ct);
Task<int> RevokeAllByExternalIdentityAsync(int externalIdentityId, int revokedBy, CancellationToken ct);
```

## Serviços de domínio sugeridos

### `UserTenantDomainService`

Responsabilidades:

- validar vínculo ativo;
- impedir role sem `UserTenant` ativo;
- definir tenant default único;
- validar source.

### `LocalCredentialDomainService`

Responsabilidades:

- normalizar login identifier;
- validar lockout;
- registrar falha de login;
- resetar falhas;
- trocar senha;
- validar se usuário pode ter reset local.

### `AuthorizationCalculationDomainService`

Responsabilidades:

- receber `userId`, `tenantId`, `appId`;
- buscar roles ativas;
- buscar role permissions;
- devolver roles e permissions efetivas;
- não depender de `HttpContext`.

Contrato sugerido:

```csharp
Task<EffectiveAuthorizationResult> CalculateAsync(int userId, int tenantId, int appId, CancellationToken ct);
```

Resultado:

```csharp
public sealed record EffectiveAuthorizationResult(
    IReadOnlyCollection<string> Roles,
    IReadOnlyCollection<string> Permissions,
    IReadOnlyCollection<int> RoleIds);
```

Permissions em formato:

```text
resource:action
```

### `AuthenticationAuditDomainService`

Responsabilidades:

- registrar sucesso/falha de login;
- registrar refresh;
- registrar troca de tenant;
- registrar acesso negado;
- registrar auto provisionamento.

## Registro no IoC

Adicionar novos serviços e repositórios em `DependencyInjection.cs`, sem duplicar blocos.

```csharp
services.AddScoped<IUserLocalCredentialDataRepository, UserLocalCredentialDataRepository>();
services.AddScoped<IUserTenantDataRepository, UserTenantDataRepository>();
services.AddScoped<IIdentityProviderDataRepository, IdentityProviderDataRepository>();
services.AddScoped<ITenantIdentityProviderDataRepository, TenantIdentityProviderDataRepository>();
services.AddScoped<IUserExternalIdentityDataRepository, UserExternalIdentityDataRepository>();
services.AddScoped<IExternalGroupDataRepository, ExternalGroupDataRepository>();
services.AddScoped<ITenantExternalGroupRoleMappingDataRepository, TenantExternalGroupRoleMappingDataRepository>();
services.AddScoped<IAuthenticationEventDataRepository, AuthenticationEventDataRepository>();
```

## Cuidados

- Não usar `FirstOrDefault()` para determinar tenant/app/role ativa.
- Não buscar roles sem filtrar por tenant e app.
- Não permitir `UserRole` se `UserTenant` ativo não existir.
- Não retornar usuário soft-deleted em login/autorização.
- Não expor `PasswordHash` em projeções ou DTOs.

## Critérios de aceite

- Novos repositórios criados e registrados.
- Repositórios antigos não dependem mais de `Users.TenantId`.
- Existe query única para cálculo de permissões por `userId + tenantId + appId`.
- Existe query para tenants disponíveis do usuário.
- Existe query para credencial local por login normalizado.
- Testes cobrem usuário com dois tenants e roles diferentes por app.
