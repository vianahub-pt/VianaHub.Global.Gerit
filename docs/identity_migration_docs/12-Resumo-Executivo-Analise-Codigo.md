# Resumo Executivo da Análise do Código

## O que foi analisado

- `README.md`
- `docs/sql/Create-Tables-v2-MultiTenant-Federated-Identity.sql`
- estrutura e código-fonte do ZIP `EBL.FIG.Process.Identity.Api.zip`

## Arquitetura atual identificada

A solução está organizada em camadas:

```text
Api              -> Minimal APIs, Swagger, middlewares, filters
Application      -> DTOs, AppServices, AutoMapper, JWT/Refresh/Auth services
Domain           -> Entidades, validadores, interfaces e serviços de domínio
Infra.Data       -> EF Core, DbContext, mappings, repositories, RLS interceptors
Infra.IoC        -> Dependency Injection
Infra.Job        -> Hangfire jobs
Infra.Integration -> Integrações externas ainda embrionárias
```

A arquitetura por recurso já está presente e deve ser preservada. O maior trabalho não é reorganizar pastas; é alinhar o modelo de identidade ao v2.

## Principal gap

O código atual ainda trabalha com usuário pertencendo diretamente ao tenant:

```text
Users.TenantId
Users.LoginIdentifier
Users.NormalizedLoginIdentifier
Users.PasswordHash
```

O modelo v2 exige:

```text
Users global
UserLocalCredentials para login/senha
UserTenants para vínculos tenant
UserExternalIdentities para federação
TenantIdentityProviders para habilitar provider por tenant/app
```

## Riscos principais

1. Refatorar login, token, refresh e RLS em um único PR.
2. Continuar usando `FirstOrDefault()` de `UserRoles` para definir app/role ativa.
3. Manter `TenantId` em `Users` por compatibilidade.
4. Esquecer que RLS v2 exige `UserId`, `TenantId` e `IsSuperAdmin`.
5. Permitir que refresh token troque tenant implicitamente.
6. Usar e-mail como chave confiável de federação.
7. Não auditar falhas de login e callbacks externos.

## Recomendação central

Migrar em fatias pequenas, mantendo build verde:

```text
1. baseline/DI
2. domínio v2
3. EF mappings v2
4. repositórios v2
5. login local v2
6. token/refresh/RBAC v2
7. tenant switch
8. password reset v2
9. providers externos
10. audit/jobs
11. testes/migração/deploy
```
