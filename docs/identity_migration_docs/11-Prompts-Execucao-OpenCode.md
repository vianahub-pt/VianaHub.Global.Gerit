# 11 — Prompts de Execução para OpenCode/Codex

Use estes prompts em sequência. Cada prompt foi desenhado para gerar uma alteração pequena e revisável.

## Prompt 01 — Baseline e DI

```text
Leia o README.md, docs/sql/Create-Tables-v2-MultiTenant-Federated-Identity.sql e faça uma análise do estado atual da solução EBL.FIG.Process.Identity.Api.

Tarefa desta etapa: não implementar o modelo v2 ainda. Apenas corrigir a infraestrutura de baseline.

Alterações obrigatórias:
1. Remover o registro duplicado de IdentityDbContext. Deve existir apenas um AddDbContext e ele deve configurar TenantSessionConnectionInterceptor, TenantSessionCommandInterceptor e TelemetryInterceptor.
2. Remover duplicidades de registros em DependencyInjection.cs, especialmente IAdminTenantContext e serviços Admin repetidos.
3. Garantir que .gitignore ignore .vs, bin, obj, logs e *.log.
4. Criar/atualizar um documento docs/migration/baseline-endpoints.md listando os endpoints existentes.
5. Não alterar entidades, DTOs ou fluxo de autenticação nesta etapa.

Critérios de aceite:
- A solução compila.
- Interceptors continuam registrados.
- Nenhum comportamento funcional de autenticação foi alterado.
```

## Prompt 02 — Entidades v2

```text
Implementar apenas a camada Domain do modelo Identity v2, com base no script docs/sql/Create-Tables-v2-MultiTenant-Federated-Identity.sql.

Alterações obrigatórias:
1. Refatorar UserEntity para ser usuário global: remover TenantId, LoginIdentifier, NormalizedLoginIdentifier e PasswordHash; adicionar NormalizedEmail; manter Name, Email, UrlImage, LastAccessAt, IsActive, IsDeleted e auditoria.
2. Criar entidades: UserLocalCredentialEntity, UserTenantEntity, IdentityProviderEntity, TenantIdentityProviderEntity, UserExternalIdentityEntity, ExternalGroupEntity, TenantExternalGroupRoleMappingEntity e AuthenticationEventEntity.
3. Ajustar AppEntity para incluir Audience.
4. Ajustar UserRoleEntity para incluir Source, TenantExternalGroupRoleMappingId e auditoria.
5. Ajustar RefreshTokenEntity para incluir IdentityProviderId e ExternalIdentityId.
6. Ajustar PasswordResetTokenEntity para remover TenantId.
7. Ajustar JwtKeyEntity para incluir AppId opcional.
8. Ajustar JobDefinitionEntity para incluir TenantId.
9. Atualizar validadores de domínio relacionados, sem tocar em EF mappings ainda.

Critérios de aceite:
- Projeto Domain compila.
- Não alterar endpoints nem AppServices nesta etapa.
```

## Prompt 03 — EF Core mappings v2

```text
Implementar os mappings EF Core e DbSets para o schema v2, sem alterar ainda o fluxo de login.

Alterações obrigatórias:
1. Atualizar IdentityDbContext com DbSets das novas entidades.
2. Atualizar UserMapping para Users global sem TenantId, LoginIdentifier, NormalizedLoginIdentifier e PasswordHash.
3. Criar mappings para UserLocalCredentials, UserTenants, IdentityProviders, TenantIdentityProviders, UserExternalIdentities, ExternalGroups, TenantExternalGroupRoleMappings e AuthenticationEvents.
4. Ajustar AppMapping para Audience.
5. Ajustar UserRoleMapping para FK com UserTenants, Source e TenantExternalGroupRoleMappingId.
6. Ajustar RefreshTokenMapping para FK com UserTenants e campos IdentityProviderId/ExternalIdentityId.
7. Ajustar PasswordResetTokenMapping para FK com UserLocalCredentials(UserId), sem TenantId.
8. Ajustar JwtKeyMapping com AppId opcional.
9. Ajustar JobDefinitionMapping com TenantId.
10. Não alterar AuthAppService nesta etapa.

Critérios de aceite:
- Infra.Data compila.
- Mappings refletem o SQL v2.
```

## Prompt 04 — RLS Session Context v2

```text
Atualizar os interceptors de RLS para o modelo v2.

Alterações obrigatórias:
1. TenantSessionConnectionInterceptor e TenantSessionCommandInterceptor devem setar sempre UserId, TenantId e IsSuperAdmin no SESSION_CONTEXT.
2. Para request autenticado normal, ler sub/user id, tenant_id e app_id do token.
3. Para request anônimo com contexto de login, usar IRequestTenantContext quando definido.
4. Para admin, usar IAdminTenantContext e setar IsSuperAdmin=1.
5. Limpar explicitamente contexto com NULL e IsSuperAdmin=0 quando não houver contexto, para evitar vazamento por connection pooling.
6. Atualizar IRequestTenantContext para armazenar UserId, TenantId e AppId.
7. Não alterar regras de autorização ainda.

Critérios de aceite:
- Teste ou log demonstra sp_set_session_context com UserId, TenantId e IsSuperAdmin.
```

## Prompt 05 — Repositórios v2

```text
Criar e ajustar repositórios para o modelo v2.

Alterações obrigatórias:
1. Criar repositórios/interfaces para UserLocalCredentials, UserTenants, IdentityProviders, TenantIdentityProviders, UserExternalIdentities, ExternalGroups, TenantExternalGroupRoleMappings e AuthenticationEvents.
2. Refatorar UserDataRepository para Users global. Remover dependência de TenantId em Users.
3. Refatorar UserRoleDataRepository para queries por userId + tenantId + appId.
4. Criar método de cálculo de permissions por tenant/app sem N+1.
5. Refatorar RefreshTokenDataRepository para tenantId + appId + tokenHash.
6. Registrar tudo em DependencyInjection.cs.

Critérios de aceite:
- Não usar Users.TenantId em nenhum repositório.
- UserRoles para token sempre filtram tenant/app.
```

## Prompt 06 — Login local v2

```text
Refatorar o fluxo de login local para usar Users global, UserLocalCredentials e UserTenants.

Alterações obrigatórias:
1. LoginRequest deve aceitar LoginIdentifier, Password, TenantId opcional e AppId opcional.
2. AuthAppService.LoginAsync deve buscar UserLocalCredentials por NormalizedLoginIdentifier.
3. Validar senha contra UserLocalCredentials.PasswordHash.
4. Validar User ativo e não deletado.
5. Buscar UserTenants ativos.
6. Se houver múltiplos tenants/apps e a request não informou contexto, retornar RequiresTenantSelection=true com AvailableTenants.
7. Se TenantId/AppId vierem na request, validar vínculo ativo e app ativa.
8. Não emitir token usando FirstOrDefault de UserRoles.
9. Auditar sucesso/falha em AuthenticationEvents, se o serviço já existir; se ainda não existir, deixar TODO explícito e não bloquear build.

Critérios de aceite:
- Login de usuário com um tenant retorna token.
- Login de usuário com múltiplos tenants retorna seleção ou aceita tenant/app informados.
```

## Prompt 07 — JWT e RefreshToken v2

```text
Refatorar emissão de JWT e refresh token para o modelo v2.

Alterações obrigatórias:
1. Criar TokenIssueContext explícito com UserId, TenantId, UserTenantId, AppId, AuthMethod, roles e permissions.
2. JwtTokenService.GenerateAccessTokenAsync deve receber TokenIssueContext, não UserEntity isolado.
3. Emitir claims v2: sub, tenant_id, user_tenant_id, app_id, auth_method, roles, permissions, jti, nbf, exp, iss, aud.
4. Para externo, suportar idp_id, external_identity_id, external_tenant_id e external_object_id.
5. Permissions devem sair em formato resource:action. Manter compatibilidade temporária com formato antigo se necessário.
6. RefreshTokenService deve emitir/rotacionar por tenantId + appId + userId.
7. Refresh não pode trocar tenant/app. Troca de tenant é outro endpoint.

Critérios de aceite:
- Token representa apenas um tenant/app.
- Permissions de outro tenant/app não aparecem.
```

## Prompt 08 — Switch tenant e available tenants

```text
Criar endpoints de seleção e troca de tenant.

Alterações obrigatórias:
1. Criar GET /v1/auth/available-tenants para listar tenants/apps disponíveis para o usuário autenticado ou token de seleção.
2. Criar POST /v1/auth/switch-tenant com TenantId e AppId.
3. Validar UserTenants ativo.
4. Validar App ativa no tenant.
5. Recalcular roles/permissions.
6. Emitir novo access token e novo refresh token.
7. Auditar TenantSwitch.

Critérios de aceite:
- Usuário com vínculo ativo troca de tenant.
- Usuário sem vínculo recebe 403.
```

## Prompt 09 — Password reset v2

```text
Refatorar ForgotPasswordAppService para UserLocalCredentials.

Alterações obrigatórias:
1. Forgot password deve buscar UserLocalCredentials por login normalizado.
2. PasswordResetTokenEntity não deve usar TenantId.
3. Password reset deve alterar UserLocalCredentials.PasswordHash, não Users.PasswordHash.
4. Usuário externo sem credencial local não deve resetar senha local.
5. Resposta de forgot password não deve revelar se a conta existe.
6. Auditar eventos de password reset.

Critérios de aceite:
- Reset funciona para credencial local.
- Reset não funciona para usuário apenas externo.
```

## Prompt 10 — Providers externos fundação

```text
Implementar a fundação de identidade federada sem integração real com todos os providers.

Alterações obrigatórias:
1. Criar AppServices, DTOs, AutoMapper, endpoints, validators e repositories para IdentityProviders e TenantIdentityProviders.
2. Criar contratos IExternalIdentityProviderClient e ExternalTokenValidationResult.
3. Criar endpoint de challenge e callback com implementação inicial no-op/fake controlada para teste, sem expor em produção se incompleto.
4. Criar UserExternalIdentity flow por issuer/subject.
5. Preparar auto provisionamento com AllowedDomains e AutoLinkByVerifiedEmail, mas manter desabilitado por padrão.

Critérios de aceite:
- CRUD de providers funciona.
- Provider só é usado se habilitado para tenant/app.
```

## Prompt 11 — Audit e jobs

```text
Implementar AuthenticationEvents e ajustar jobs ao modelo v2.

Alterações obrigatórias:
1. Criar AuthenticationEventAppService/repository/endpoint com filtros e paginação.
2. Registrar eventos em login, refresh, logout, switch tenant, forgot/reset password e external login.
3. Atualizar JobDefinition para TenantId nos services, repositories e endpoints.
4. Criar jobs de cleanup para refresh tokens, password reset tokens e authentication events, se ainda não existirem.
5. Garantir que jobs definam contexto de RLS antes de acessar o banco.

Critérios de aceite:
- Eventos aparecem após login/refresh/falha.
- Jobs rodam com TenantId definido.
```

## Prompt 12 — Testes e migração de dados

```text
Criar testes e script de migração de dados v1 para v2.

Alterações obrigatórias:
1. Criar testes unitários para domínio, login local, switch tenant, refresh, permissions e password reset.
2. Criar testes de integração para RLS com SQL Server real/container.
3. Criar script docs/sql/Migrate-v1-to-v2.sql separado do Create-Tables-v2.
4. Script deve migrar Users v1 para Users global, UserLocalCredentials e UserTenants.
5. Não migrar refresh tokens antigos por padrão; forçar novo login.
6. Criar docs/migration/deploy-checklist-v2.md.

Critérios de aceite:
- Testes críticos verdes.
- Script de migração validado em base de cópia.
```
