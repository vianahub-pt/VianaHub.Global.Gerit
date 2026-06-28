# 03 — Infra.Data: EF Core, Mapeamentos e RLS

## Objetivo

Atualizar `IdentityDbContext`, mappings e interceptors para que a aplicação consiga operar sobre a base v2 com segurança por tenant.

## Arquivos impactados

```text
src/EBL.FIG.Process.Identity.Infra.Data/Context/IdentityDbContext.cs
src/EBL.FIG.Process.Identity.Infra.Data/Mappings/*.cs
src/EBL.FIG.Process.Identity.Infra.Data/Repository/*.cs
src/EBL.FIG.Process.Identity.Infra.Data/Interceptors/TenantSessionConnectionInterceptor.cs
src/EBL.FIG.Process.Identity.Infra.Data/Interceptors/TenantSessionCommandInterceptor.cs
src/EBL.FIG.Process.Identity.Infra.Data/Context/RequestTenantContext.cs
src/EBL.FIG.Process.Identity.Infra.Data/Context/AdminTenantContext.cs
src/EBL.FIG.Process.Identity.Infra.Data/Seeders/DatabaseSeeder.cs
src/EBL.FIG.Process.Identity.Infra.IoC/DependencyInjection.cs
```

## 1. Atualizar `IdentityDbContext`

Adicionar DbSets:

```csharp
public DbSet<UserLocalCredentialEntity> UserLocalCredentials { get; set; }
public DbSet<UserTenantEntity> UserTenants { get; set; }
public DbSet<IdentityProviderEntity> IdentityProviders { get; set; }
public DbSet<TenantIdentityProviderEntity> TenantIdentityProviders { get; set; }
public DbSet<UserExternalIdentityEntity> UserExternalIdentities { get; set; }
public DbSet<ExternalGroupEntity> ExternalGroups { get; set; }
public DbSet<TenantExternalGroupRoleMappingEntity> TenantExternalGroupRoleMappings { get; set; }
public DbSet<AuthenticationEventEntity> AuthenticationEvents { get; set; }
```

Ajustar nome do DbSet de jobs para ficar claro:

```csharp
public DbSet<JobDefinitionEntity> JobDefinitions { get; set; }
```

Se existir código usando `JobDefinitionEntities`, migrar com cuidado.

## 2. Atualizar mappings existentes

### `UserMapping.cs`

Remover:

```text
TenantId
LoginIdentifier
NormalizedLoginIdentifier
PasswordHash
FK_Users_Tenant
UQ_Users_Tenant_NormalizedLoginIdentifier
UQ_Users_Id_Tenant
```

Adicionar:

```text
Name NVARCHAR(256) NOT NULL
Email NVARCHAR(500) NULL
NormalizedEmail NVARCHAR(500) NULL
UrlImage NVARCHAR(500) NULL
LastAccessAt DATETIME2(7) NULL
IsActive BIT NOT NULL DEFAULT 1
IsDeleted BIT NOT NULL DEFAULT 0
AddedBy INT NOT NULL
AddedOn DATETIME2(7) NOT NULL DEFAULT SYSDATETIME()
ModifiedBy INT NULL
ModifiedAt DATETIME2(7) NULL
```

Índice:

```csharp
builder.HasIndex(x => x.NormalizedEmail)
    .HasDatabaseName("IX_Users_NormalizedEmail");
```

Relacionamentos:

```csharp
builder.HasOne(x => x.LocalCredential)
    .WithOne(x => x.User)
    .HasForeignKey<UserLocalCredentialEntity>(x => x.UserId)
    .OnDelete(DeleteBehavior.Restrict);

builder.HasMany(x => x.UserTenants)
    .WithOne(x => x.User)
    .HasForeignKey(x => x.UserId)
    .OnDelete(DeleteBehavior.Restrict);

builder.HasMany(x => x.ExternalIdentities)
    .WithOne(x => x.User)
    .HasForeignKey(x => x.UserId)
    .OnDelete(DeleteBehavior.Restrict);
```

### `AppMapping.cs`

Adicionar `Audience`:

```csharp
builder.Property(x => x.Audience)
    .HasColumnType("NVARCHAR(300)")
    .HasMaxLength(300)
    .IsRequired(false);
```

Manter alternate key:

```csharp
builder.HasAlternateKey(x => new { x.TenantId, x.Id })
    .HasName("UQ_Apps_Tenant_Id");
```

### `UserRoleMapping.cs`

Adicionar campos:

```text
Source NVARCHAR(50) NOT NULL DEFAULT 'Manual'
TenantExternalGroupRoleMappingId INT NULL
AddedBy INT NOT NULL
AddedOn DATETIME2(7) NOT NULL
ModifiedBy INT NULL
ModifiedAt DATETIME2(7) NULL
```

Alterar FK de User para UserTenant:

```csharp
builder.HasOne(x => x.UserTenant)
    .WithMany(x => x.UserRoles)
    .HasForeignKey(x => new { x.TenantId, x.UserId })
    .HasPrincipalKey(x => new { x.TenantId, x.UserId })
    .HasConstraintName("FK_UserRoles_UserTenant")
    .OnDelete(DeleteBehavior.Restrict);
```

Não usar mais principal key `{ Id, TenantId }` de `Users`, porque `Users` é global.

### `RefreshTokenMapping.cs`

Adicionar:

```text
IdentityProviderId INT NULL
ExternalIdentityId INT NULL
```

A FK de usuário deve apontar para `UserTenants`:

```csharp
builder.HasOne(x => x.UserTenant)
    .WithMany()
    .HasForeignKey(x => new { x.TenantId, x.UserId })
    .HasPrincipalKey(x => new { x.TenantId, x.UserId })
    .HasConstraintName("FK_RefreshTokens_UserTenant");
```

### `PasswordResetTokenMapping.cs`

Remover `TenantId`.

FK:

```csharp
builder.HasOne(x => x.LocalCredential)
    .WithMany()
    .HasForeignKey(x => x.UserId)
    .HasPrincipalKey(x => x.UserId)
    .HasConstraintName("FK_PasswordResetTokens_UserLocalCredentials");
```

### `JwtKeyMapping.cs`

Adicionar `AppId` opcional e FK composta para Apps:

```csharp
builder.Property(x => x.AppId).IsRequired(false);

builder.HasOne(x => x.App)
    .WithMany()
    .HasForeignKey(x => new { x.TenantId, x.AppId })
    .HasPrincipalKey(x => new { x.TenantId, x.Id })
    .HasConstraintName("FK_JwtKeys_App")
    .OnDelete(DeleteBehavior.Restrict);
```

### `JobDefinitionMapping.cs`

Adicionar `TenantId` obrigatório e ajustar índices:

```text
IX_JobDefinitions_Category_Active -> TenantId, Category, IsActive, IsDeleted
IX_JobDefinitions_Active_System -> TenantId, IsActive, IsSystemJob WHERE IsDeleted = 0
```

## 3. Criar novos mappings

Criar arquivos:

```text
UserLocalCredentialMapping.cs
UserTenantMapping.cs
IdentityProviderMapping.cs
TenantIdentityProviderMapping.cs
UserExternalIdentityMapping.cs
ExternalGroupMapping.cs
TenantExternalGroupRoleMappingMapping.cs
AuthenticationEventMapping.cs
```

Cada mapping deve seguir exatamente nomes de constraints/índices do SQL v2 quando possível. Isso reduz divergências entre EF e banco.

## 4. Atualizar RLS interceptors

O SQL v2 espera sempre estes valores:

```sql
UserId
TenantId
IsSuperAdmin
```

Hoje os interceptors setam principalmente `TenantId` e `IsSuperAdmin` em admin. Atualizar para:

### Request autenticado normal

```sql
EXEC sp_set_session_context @key=N'UserId', @value=@userId;
EXEC sp_set_session_context @key=N'TenantId', @value=@tenantId;
EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=0;
```

### Pós-login antes da escolha de tenant

```sql
EXEC sp_set_session_context @key=N'UserId', @value=@userId;
EXEC sp_set_session_context @key=N'TenantId', @value=NULL;
EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=0;
```

### SuperAdmin/admin interno

```sql
EXEC sp_set_session_context @key=N'UserId', @value=@userId;
EXEC sp_set_session_context @key=N'TenantId', @value=@targetTenantId;
EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=1;
```

### Request anônimo sem contexto

Definir explicitamente:

```sql
EXEC sp_set_session_context @key=N'UserId', @value=NULL;
EXEC sp_set_session_context @key=N'TenantId', @value=NULL;
EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=0;
```

Isso evita vazamento de contexto em connection pooling.

## 5. Atualizar contratos de contexto

### `IRequestTenantContext`

Adicionar suporte a UserId e AppId:

```csharp
int? UserId { get; }
int? TenantId { get; }
int? AppId { get; }
void SetUserId(int userId);
void SetTenantId(int tenantId);
void SetAppId(int appId);
void SetContext(int? userId, int? tenantId, int? appId);
void Clear();
```

### `IAdminTenantContext`

Adicionar `UserId` se necessário para auditoria:

```csharp
int? AdminUserId { get; }
int? TargetTenantId { get; }
int? TargetAppId { get; }
bool IsActive { get; }
void Activate(int adminUserId, int targetTenantId, int? targetAppId = null);
void Clear();
```

## 6. Seeders

Atualizar `DatabaseSeeder` para popular no mínimo:

```text
Tenants: Identity
Apps: Identity com Audience
Users: usuário global admin
UserLocalCredentials: credencial local do admin
UserTenants: admin associado ao tenant Identity
Roles: BackOffice/Admin
Resources/Actions/RolePermissions
UserRoles: admin com role BackOffice/Admin
JwtKeys: chave do tenant/app
IdentityProviders: Local
TenantIdentityProviders: Local habilitado para Identity/Identity
JobDefinitions: com TenantId
```

## 7. Cuidados com RLS e EF

- Não confiar apenas em filtros EF (`Where(x => x.TenantId == ...)`). O banco deve bloquear por RLS.
- Mesmo assim, manter filtros explícitos por clareza e performance.
- Em login anônimo, o contexto precisa permitir busca de credencial local. O script v2 filtra `UserLocalCredentials` por `UserId`, então o login local pode exigir operação controlada de sistema/superadmin ou uma função específica de lookup. Documentar a decisão.
- Evitar que o login anônimo rode com `IsSuperAdmin=1` amplo. Se for necessário bypass para localizar credencial, encapsular em repositório específico e auditar.

## Critérios de aceite

- `IdentityDbContext` contém todos os DbSets v2.
- Todos os mappings compilam.
- As FKs compostas usam `UserTenants`, não `Users.TenantId`.
- Interceptors setam `UserId`, `TenantId`, `IsSuperAdmin` explicitamente.
- Registro do DbContext é único e inclui interceptors.
- Seeder cria dados compatíveis com v2.
- Teste de RLS confirma que tenant A não lê dados do tenant B.
