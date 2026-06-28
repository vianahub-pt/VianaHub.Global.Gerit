# Pacote de Migração — EBL.FIG.Process.Identity.Api para v2 MultiTenant Federated Identity

Este pacote foi criado a partir da leitura do `README.md`, do script `docs/sql/Create-Tables-v2-MultiTenant-Federated-Identity.sql` e de uma varredura estática do código-fonte do ZIP `EBL.FIG.Process.Identity.Api.zip`.

## Como usar estes arquivos

A recomendação é executar as refatorações em lotes pequenos, nesta ordem:

1. `00-Plano-Geral-Migracao-Identity-v2.md`
2. `01-Preparacao-Baseline-e-Guardrails.md`
3. `02-Dominio-Entidades-Modelo-v2.md`
4. `03-Infra-Data-EFCore-Mapeamentos-RLS.md`
5. `04-Repositorios-Queries-e-Servicos-Dominio.md`
6. `05-Auth-Local-UserTenants-TenantSwitch.md`
7. `06-JWT-RefreshTokens-RBAC-Permissions.md`
8. `07-Federated-Identity-Providers-External-Login.md`
9. `08-API-Endpoints-Contratos-DTOs-Validacoes.md`
10. `09-Audit-AuthenticationEvents-Jobs-Observabilidade.md`
11. `10-Testes-Migracao-Dados-Deploy.md`
12. `11-Prompts-Execucao-OpenCode.md`

## Princípio da migração

Não tratar esta mudança como uma única migração grande. O script v2 muda o conceito central da aplicação: o usuário deixa de pertencer diretamente a um tenant e passa a ser uma identidade global com vínculos em `UserTenants`, credenciais locais em `UserLocalCredentials` e identidades externas em `UserExternalIdentities`.

Por isso, a sequência correta é:

```text
modelo de domínio -> EF/mapeamentos -> repositórios -> autenticação local -> token/RBAC -> federação -> auditoria/jobs -> testes/deploy
```

## Resultado esperado

Ao final da migração, a API deverá suportar:

- usuário global sem `TenantId` direto em `Users`;
- login local por `UserLocalCredentials`;
- usuário associado a múltiplos tenants por `UserTenants`;
- token JWT representando apenas um tenant ativo e uma app ativa;
- refresh token vinculado a tenant, app e usuário;
- providers externos por tenant/app;
- identidade externa vinculada por issuer/subject, não apenas por e-mail;
- RLS com `SESSION_CONTEXT` contendo `UserId`, `TenantId` e `IsSuperAdmin`;
- auditoria de autenticação em `AuthenticationEvents`;
- jobs com `TenantId` e compatíveis com o novo modelo.
