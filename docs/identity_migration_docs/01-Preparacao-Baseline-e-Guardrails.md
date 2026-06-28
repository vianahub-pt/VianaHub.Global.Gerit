# 01 — Preparação, Baseline e Guardrails

## Objetivo

Preparar o repositório para uma migração grande, mas executada em passos pequenos. Esta etapa não deve alterar o comportamento funcional da API, exceto correções de infraestrutura claramente necessárias.

## Escopo

- Criar baseline técnico antes da migração.
- Corrigir duplicidade de DI que pode afetar interceptors de RLS.
- Definir estratégia de branch/PR.
- Criar testes mínimos de regressão.
- Garantir que arquivos gerados não contaminem commits.

## Arquivos analisados com impacto direto

```text
src/EBL.FIG.Process.Identity.Api/Program.cs
src/EBL.FIG.Process.Identity.Infra.IoC/DependencyInjection.cs
src/EBL.FIG.Process.Identity.Infra.Data/Context/IdentityDbContext.cs
src/EBL.FIG.Process.Identity.Infra.Data/Interceptors/*
src/EBL.FIG.Process.Identity.Api/EBL.FIG.Process.Identity.Api.csproj
.gitignore
.runsettings
tests/EBL.FIG.Process.Identity.Tests/*
```

## Problemas encontrados

### 1. DbContext registrado duas vezes

`Program.cs` registra `IdentityDbContext` com interceptors:

```csharp
builder.Services.AddDbContext<IdentityDbContext>((serviceProvider, options) =>
{
    options.UseSqlServer(...);
    options.AddInterceptors(...);
});
```

Mas `DependencyInjection.cs` registra novamente:

```csharp
services.AddDbContext<IdentityDbContext>(options =>
    options.UseSqlServer(connectionString));
```

Isso deve ser removido ou unificado, porque pode fazer o EF usar uma configuração sem interceptors de RLS.

### 2. Registros duplicados no IoC

`DependencyInjection.cs` tem blocos duplicados de `IAdminTenantContext` e serviços Admin. Deve ser limpo antes de adicionar novos serviços.

### 3. Pacotes EF Core 9 em aplicação .NET 8

`Infra.Data` usa `Microsoft.EntityFrameworkCore` 9.0.8. A aplicação é .NET 8 e o README técnico fala em EF Core 8. Decidir uma das duas estratégias antes da migração:

- manter EF Core 9 por decisão explícita; ou
- alinhar para EF Core 8 LTS.

Não misturar essa decisão com a refatoração de autenticação.

### 4. Diretórios gerados dentro do ZIP

O ZIP contém `.git`, `.vs`, `bin`, `obj` e logs. Isso não deve entrar no versionamento nem nos prompts de refatoração.

## Instruções de refatoração

### Passo 1 — Criar branch de migração

Sugestão:

```bash
git checkout develop
git pull
git checkout -b feature/identity-v2-multitenant-federated
```

### Passo 2 — Limpar `.gitignore`

Garantir que estes padrões existem:

```gitignore
.vs/
**/bin/
**/obj/
**/logs/
*.log
*.user
*.suo
```

### Passo 3 — Corrigir registro do DbContext

Escolha uma única fonte de registro.

Recomendação: manter o registro no `Program.cs` ou mover toda a lógica para `DependencyInjection.cs`, mas não duplicar.

Preferência para organização:

```text
Program.cs -> chama AddDependencyInjection
DependencyInjection.cs -> registra IdentityDbContext com interceptors
```

O registro final deve incluir:

```csharp
services.AddDbContext<IdentityDbContext>((serviceProvider, options) =>
{
    options.UseSqlServer(connectionString);

    options.AddInterceptors(
        serviceProvider.GetRequiredService<TenantSessionConnectionInterceptor>(),
        serviceProvider.GetRequiredService<TenantSessionCommandInterceptor>(),
        serviceProvider.GetRequiredService<TelemetryInterceptor>()
    );
});
```

Remover o outro registro.

### Passo 4 — Remover duplicidades no IoC

Remover blocos repetidos de:

```text
IAdminTenantContext
IAdminUserAppService
IAdminAppAppService
IAdminRoleAppService
IAdminResourceAppService
IAdminActionAppService
IAdminRolePermissionAppService
IAdminUserRoleAppService
```

### Passo 5 — Criar testes de baseline

Criar testes antes de alterar o modelo.

Testes mínimos:

```text
Auth_Login_CurrentFlow_ReturnsToken_WhenCredentialsAreValid
Auth_Refresh_CurrentFlow_RotatesToken_WhenRefreshTokenIsValid
AuthorizationFilter_Denies_WhenPermissionMissing
AuthorizationFilter_Allows_WhenPermissionExists
CurrentUserApiService_ReadsTenantIdAndAppId_FromClaims
```

Mesmo que alguns testes usem fakes/mocks, eles servem para detectar quebras acidentais durante a migração.

### Passo 6 — Criar snapshot de endpoints

Gerar uma lista dos endpoints atuais e guardar em `docs/migration/baseline-endpoints.md`.

Endpoints identificados:

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
/v1/admin/jwtkeys
/v1/admin/tenants/{tenantId}/...
```

## Fora do escopo desta etapa

Não alterar ainda:

- `UserEntity`;
- autenticação;
- token JWT;
- refresh token;
- entidades federadas;
- script SQL;
- DTOs públicos.

## Critérios de aceite

- A solução compila.
- Existe apenas um registro de `IdentityDbContext`.
- Interceptors continuam configurados.
- Duplicidades de DI removidas.
- Testes de baseline criados.
- `.gitignore` protege arquivos gerados.
- Nenhum comportamento funcional foi alterado intencionalmente.
