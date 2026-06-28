# Plano Geral de Migração — Identity v1 para v2 MultiTenant Federated Identity

## 1. Diagnóstico da implementação atual

A solução atual está organizada em projetos separados:

```text
src/EBL.FIG.Process.Identity.Api              -> Minimal APIs, Swagger, filtros, middlewares, autenticação, autorização
src/EBL.FIG.Process.Identity.Application      -> DTOs, AutoMapper, AppServices, serviços de token e autenticação
src/EBL.FIG.Process.Identity.Domain           -> entidades, interfaces, validadores, serviços de domínio
src/EBL.FIG.Process.Identity.Infra.Data       -> DbContext, mappings EF Core, repositórios, RLS interceptors, seeders
src/EBL.FIG.Process.Identity.Infra.IoC        -> registro de dependências
src/EBL.FIG.Process.Identity.Infra.Job        -> Hangfire, jobs de manutenção e segurança
src/EBL.FIG.Process.Identity.Infra.Integration -> integrações externas, hoje praticamente no-op
```

Foram identificados 358 arquivos `.cs` úteis fora de `bin/obj`, distribuídos aproximadamente assim:

```text
Api:             73 arquivos
Application:    121 arquivos
Domain:         111 arquivos
Infra.Data:      36 arquivos
Infra.Job:       13 arquivos
Infra.IoC:        1 arquivo
Infra.Integration: 2 arquivos
```

A API usa Minimal APIs por recurso, com endpoints como:

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

A estrutura por recurso já existe e deve ser preservada. A migração deve trocar o modelo interno sem desmontar a arquitetura inteira.

## 2. Principal incompatibilidade com o modelo v2

Hoje `UserEntity` possui:

```csharp
TenantId
LoginIdentifier
NormalizedLoginIdentifier
PasswordHash
Email
LastAccessAt
```

No modelo v2, isso muda para:

```text
Users                    -> identidade global, sem TenantId e sem senha
UserLocalCredentials     -> login/senha local por UserId
UserTenants              -> vínculo UserId + TenantId
UserExternalIdentities   -> vínculo com provedores externos
UserRoles                -> role por Tenant + App + User, validando existência em UserTenants
```

Essa é a mudança estrutural mais crítica. Ela impacta:

- `UserEntity`;
- `UserMapping`;
- `UserDataRepository`;
- `AuthAppService`;
- `ForgotPasswordAppService`;
- `UserAppService`;
- `UserRoleAppService`;
- `JwtTokenService`;
- `RefreshTokenService`;
- `CurrentUserApiService`;
- interceptors de RLS;
- DTOs de usuário, autenticação e token;
- filtros de autorização;
- seeders e jobs.

## 3. Tabelas v2 que devem ser suportadas

O script `Create-Tables-v2-MultiTenant-Federated-Identity.sql` define 20 tabelas:

```text
Tenants
Apps
Users
UserLocalCredentials
Roles
Resources
Actions
RolePermissions
IdentityProviders
TenantIdentityProviders
UserExternalIdentities
UserTenants
ExternalGroups
TenantExternalGroupRoleMappings
UserRoles
RefreshTokens
PasswordResetTokens
JwtKeys
JobDefinitions
AuthenticationEvents
```

As novas tabelas que não existem no domínio atual e precisam de implementação completa são:

```text
UserLocalCredentials
IdentityProviders
TenantIdentityProviders
UserExternalIdentities
UserTenants
ExternalGroups
TenantExternalGroupRoleMappings
AuthenticationEvents
```

Além disso, algumas tabelas atuais mudaram:

```text
Users                -> remove TenantId, LoginIdentifier, NormalizedLoginIdentifier, PasswordHash; adiciona NormalizedEmail
Apps                 -> adiciona Audience
UserRoles            -> adiciona Source e TenantExternalGroupRoleMappingId; FK passa a depender de UserTenants
RefreshTokens        -> adiciona IdentityProviderId e ExternalIdentityId; FK passa a depender de UserTenants
PasswordResetTokens  -> remove TenantId e aponta para UserLocalCredentials(UserId)
JwtKeys              -> adiciona AppId opcional
JobDefinitions       -> adiciona TenantId
```

## 4. Estado atual que deve ser preservado

A aplicação já possui pontos bons que devem ser reaproveitados:

- separação em camadas;
- endpoints por recurso;
- AppServices por recurso;
- repositórios por entidade;
- AutoMapper por recurso;
- validação com FluentValidation;
- autorização customizada por role/resource/action;
- JWT RS256 com `kid`;
- refresh token com hash;
- Hangfire;
- RLS por `SESSION_CONTEXT`;
- endpoints administrativos cross-tenant;
- localization `common.pt-PT.json`, `common.en-US.json`, `common.es-ES.json`.

## 5. Problemas atuais que devem ser corrigidos durante a migração

### 5.1 `Users` ainda é tenant-scoped

O modelo atual não permite um usuário global com múltiplos tenants sem duplicação. Isso contradiz as regras RN01, RN02, RN03, RN28 e RN29 do README.

### 5.2 Credencial local está misturada com identidade global

Senha e login estão em `Users`. No v2 devem estar em `UserLocalCredentials`. Isso impacta RN08, RN09, RN22 e RN30.

### 5.3 Login atual descobre tenant por login identifier

`AuthAppService.LoginAsync` chama `ITenantDataRepository.GetByLoginIdentifierAsync`, depois busca usuário dentro daquele tenant. No v2 o login local deve encontrar a credencial global, validar senha, listar vínculos ativos em `UserTenants` e só então selecionar o tenant/app.

### 5.4 JWT ainda usa claims mistas

O token atual usa `tenantId`, `appId`, `UserId` e `role`. O README v2 recomenda claims snake_case:

```text
sub
tenant_id
user_tenant_id
app_id
auth_method
roles
permissions
idp_id
external_identity_id
external_tenant_id
external_object_id
```

### 5.5 Permissões são agrupadas em JSON

O README recomenda `permissions` no formato objetivo `resource:action`. O código atual insere um dicionário JSON por resource. Pode ser mantida compatibilidade temporária, mas o contrato v2 deve emitir também lista plana.

### 5.6 RLS não seta todo o contexto esperado

Os interceptors atuais resolvem principalmente `TenantId`; o SQL v2 espera:

```sql
UserId
TenantId
IsSuperAdmin
```

A migração deve garantir que os três valores sejam sempre definidos/limpos explicitamente.

### 5.7 `Infra.IoC` registra DbContext duplicado

`Program.cs` registra `IdentityDbContext` com interceptors. `DependencyInjection.cs` registra `IdentityDbContext` novamente sem interceptors. Isso pode anular a configuração com interceptors dependendo da ordem de registro. Deve ser corrigido antes de validar RLS.

### 5.8 Application depende de Infra.Data

`EBL.FIG.Process.Identity.Application.csproj` referencia `Infra.Data`, e `AuthAppService` recebe `IdentityDbContext` diretamente. Isso viola a separação desejada. Não precisa ser corrigido em uma única etapa, mas deve ser removido gradualmente dos fluxos alterados.

## 6. Estratégia de migração incremental

### Fase 0 — Preparação e baseline

Objetivo: criar segurança antes de alterar o coração da autenticação.

Entregas:

- branch dedicada;
- build e testes de baseline;
- documentação de endpoints atuais;
- remoção de duplicidades óbvias de DI;
- garantir que `bin`, `obj`, `.vs` e logs não sejam parte da análise/commit;
- criar testes mínimos de login atual, refresh, autorização e CRUD básico.

Arquivo detalhado: `01-Preparacao-Baseline-e-Guardrails.md`.

### Fase 1 — Domínio v2

Objetivo: criar o modelo de entidades v2 sem ainda trocar todo o fluxo de autenticação.

Entregas:

- `UserEntity` global;
- novas entidades `UserLocalCredentialEntity`, `UserTenantEntity`, `IdentityProviderEntity`, `TenantIdentityProviderEntity`, `UserExternalIdentityEntity`, `ExternalGroupEntity`, `TenantExternalGroupRoleMappingEntity`, `AuthenticationEventEntity`;
- ajustes em `AppEntity`, `UserRoleEntity`, `RefreshTokenEntity`, `PasswordResetTokenEntity`, `JwtKeyEntity`, `JobDefinitionEntity`;
- validadores de domínio mínimos.

Arquivo detalhado: `02-Dominio-Entidades-Modelo-v2.md`.

### Fase 2 — EF Core, mapeamentos e RLS

Objetivo: fazer o código conversar corretamente com a base v2.

Entregas:

- novos `DbSet<>`;
- mappings EF Core fiéis ao SQL v2;
- correção dos relacionamentos compostos;
- interceptors atualizados para `UserId`, `TenantId`, `IsSuperAdmin`;
- remoção do registro duplicado do DbContext;
- `DatabaseSeeder` adaptado.

Arquivo detalhado: `03-Infra-Data-EFCore-Mapeamentos-RLS.md`.

### Fase 3 — Repositórios e serviços de domínio

Objetivo: criar acesso a dados v2 sem deixar regras espalhadas nos AppServices.

Entregas:

- novos repositórios para tabelas v2;
- métodos específicos para login local, tenants disponíveis, tenant/app ativa, permissões efetivas;
- serviços de domínio para UserTenant, local credentials, providers e audit.

Arquivo detalhado: `04-Repositorios-Queries-e-Servicos-Dominio.md`.

### Fase 4 — Login local, seleção e troca de tenant

Objetivo: implementar o fluxo RN01-RN03, RN08, RN21, RN28, RN29 e RN30.

Entregas:

- login local por `UserLocalCredentials`;
- retorno de lista de tenants quando houver múltiplos vínculos;
- seleção automática quando houver apenas um tenant/app;
- endpoint de tenants disponíveis;
- endpoint de troca de tenant;
- reset de senha apenas para usuários com credencial local;
- lockout por falhas.

Arquivo detalhado: `05-Auth-Local-UserTenants-TenantSwitch.md`.

### Fase 5 — JWT, refresh token e RBAC

Objetivo: alinhar o contrato de token e autorização ao README v2.

Entregas:

- token com claims v2;
- refresh token por Tenant + App + User;
- permissões calculadas por Tenant + App;
- `UserRoles` validado contra `UserTenants` ativo;
- claims `roles` e `permissions` consistentes;
- autorização por permission deny-by-default.

Arquivo detalhado: `06-JWT-RefreshTokens-RBAC-Permissions.md`.

### Fase 6 — Identidade federada

Objetivo: criar suporte incremental a providers externos sem tentar implementar todos os protocolos de uma vez.

Entregas:

- CRUD/configuração de `IdentityProviders` e `TenantIdentityProviders`;
- contratos para iniciar login externo e receber callback;
- interface por protocolo/provider;
- vínculo por issuer/subject;
- auto provisionamento controlado;
- mapeamento básico de grupos externos para roles.

Arquivo detalhado: `07-Federated-Identity-Providers-External-Login.md`.

### Fase 7 — API, contratos, validação e localization

Objetivo: expor os novos recursos de forma limpa e compatível com o frontend.

Entregas:

- novos DTOs;
- novos endpoints;
- atualização dos endpoints existentes;
- swagger organizado;
- localization em PT/EN/ES;
- compatibilidade temporária para clientes v1, quando necessário.

Arquivo detalhado: `08-API-Endpoints-Contratos-DTOs-Validacoes.md`.

### Fase 8 — Auditoria, jobs e observabilidade

Objetivo: registrar eventos críticos e manter dados de segurança.

Entregas:

- `AuthenticationEvents`;
- audit service;
- jobs de limpeza/rotação/sincronização;
- logs com correlation id;
- métricas de autenticação.

Arquivo detalhado: `09-Audit-AuthenticationEvents-Jobs-Observabilidade.md`.

### Fase 9 — Testes, migração de dados e deploy

Objetivo: transformar a migração em release controlada.

Entregas:

- testes unitários, integração e RLS;
- script de migração de dados v1 -> v2, se houver base existente;
- plano de rollback;
- checklist de deploy;
- validação pós-deploy.

Arquivo detalhado: `10-Testes-Migracao-Dados-Deploy.md`.

## 7. Ordem recomendada de PRs pequenos

```text
PR-01 Baseline, DI e guardrails
PR-02 Entidades v2 sem trocar fluxos
PR-03 EF mappings v2 + DbSets + build
PR-04 Repositórios v2 básicos
PR-05 User global + UserLocalCredentials + UserTenants
PR-06 Login local v2 com seleção de tenant/app
PR-07 JWT v2 + refresh v2
PR-08 RBAC/permissions v2
PR-09 RLS UserId/TenantId/IsSuperAdmin
PR-10 Password reset local-only + lockout
PR-11 Providers e tenant providers
PR-12 External identities + callback abstraído
PR-13 External groups mapping
PR-14 AuthenticationEvents + audit service
PR-15 Jobs v2
PR-16 Testes integração/RLS/deploy
```

Cada PR deve manter build verde. Evite misturar mudança de schema, fluxo de login, token e endpoints no mesmo PR.

## 8. Decisões arquiteturais recomendadas

### 8.1 Manter a estrutura por recurso

A aplicação já está organizada por recurso. A migração deve adicionar novos recursos em vez de criar uma reestruturação completa.

### 8.2 Criar camada de aplicação por caso de uso sensível

Para autenticação, não usar apenas CRUD genérico. Criar serviços específicos:

```text
ILocalAuthenticationAppService
ITenantSelectionAppService
IExternalAuthenticationAppService
IAuthorizationCalculationService
IAuthenticationAuditService
```

### 8.3 Separar credencial local da identidade

`UserEntity` não deve saber validar senha. Essa regra pertence à credencial local e ao serviço de autenticação local.

### 8.4 Usar contratos explícitos para tenant/app

Toda emissão de token deve receber explicitamente:

```text
UserId
TenantId
AppId
AuthMethod
UserTenantId
IdentityProviderId opcional
ExternalIdentityId opcional
```

Não usar `FirstOrDefault()` de `UserRoles` para decidir AppId ou RoleId.

### 8.5 Não confiar apenas em e-mail para federação

O vínculo confiável deve ser:

```text
IdentityProviderId + Issuer + Subject
```

E-mail verificado pode ser usado apenas para auto-link quando `AutoLinkByVerifiedEmail = true`.

## 9. Critérios globais de aceite

A migração só deve ser considerada completa quando:

- `Users` não tiver `TenantId`, `PasswordHash`, `LoginIdentifier` nem `NormalizedLoginIdentifier`;
- login local validar `UserLocalCredentials`;
- usuário puder estar em múltiplos tenants;
- token emitido representar apenas um tenant/app ativo;
- troca de tenant emitir novo token;
- permissões do token vierem apenas do tenant/app ativo;
- `RefreshTokens` estiverem vinculados a tenant/app/user;
- password reset só funcionar para credential local;
- RLS usar `UserId`, `TenantId`, `IsSuperAdmin`;
- providers externos estiverem modelados e com fluxo inicial implementado;
- eventos de autenticação forem registrados;
- testes cobrirem isolamento por tenant e permissões.
