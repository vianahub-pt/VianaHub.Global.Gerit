# Gerit API — Backend

Construída com **.NET 8**, seguindo **Arquitetura Hexagonal**, **DDD**, **SOLID** e **Clean Architecture**.

## Projeto

- **Solution**: `VianaHub.Global.Gerit.sln` — .NET 8 ASP.NET Core Minimal API
- **Arquitetura**: DDD + Clean Architecture + Hexagonal, multi-tenant SaaS (Gerit)
- **Stack**: EF Core 8 + SQL Server, FluentValidation 12, AutoMapper, Serilog, Hangfire, JWT RS256 (chaves por tenant), OpenTelemetry
- **Testes**: xUnit + Moq + NBuilder + coverlet

## Estrutura da Solution

| Projeto | Responsabilidade |
|---|---|
| `src/*.Api` | Minimal API endpoints (`[EndpointMapper]` + `MapEndpointsFromAssembly()`), Swagger, middleware |
| `src/*.Application` | Orquestração de use-cases, DTOs, perfis AutoMapper, códigos HTTP semânticos (409, 410) |
| `src/*.Domain` | Entidades, serviços de domínio, validadores, interfaces. Rich domain model |
| `src/*.Infra.Data` | EF Core DbContext, SQL Server, mappings, repositórios, interceptors tenant |
| `src/*.Infra.IoC` | `DependencyInjection.cs` — registro único de DI |
| `src/*.Infra.Integration` | Integrações externas (NoOpEmailSender) |
| `src/*.Infra.Job` | Jobs Hangfire, hosted services |
| `tests/*.Tests` | xUnit + Moq + NBuilder + coverlet |

## Comandos

```powershell
dotnet build                      # build da solution
dotnet test                       # executa todos os testes (xUnit)
dotnet test --filter "Category=Unit"
dotnet run --project src/*.Api    # executa a API (Swagger em /swagger)
```

## Convenções de Endpoints

- Arquivo: `{Entidade}Endpoint.cs` (singular). Classe: `{Entidade}Endpoint` (singular)
- Método: `Map{Entidade}Endpoints` (plural), anotado com `[EndpointMapper]`
- Auto-descoberta via `MapEndpointsFromAssembly()` em `Program.cs`
- Agrupados em `Endpoints/{Billing,Identity,Business,Job}/`
- Política de autorização: `"BackOffice"` (requer usuário autenticado)

## Regras de Arquitetura (restrições rígidas)

- **Sem `throw` para mensagens ao usuário** — use `INotify` para acumular mensagens + status HTTP. Exceções são apenas para falhas técnicas (capturadas pelo `GlobalExceptionMiddleware`)
- **Todas as strings para o usuário DEVEM usar chaves de localização** — nunca mensagens hardcoded. Chaves em `src/*.Api/Localization/` por idioma (pt-PT, en-US, es-ES). Cultura do header `Accept-Language`, fallback pt-PT
- **410 Gone** para recursos removidos/desativados por ID. **409 Conflict** para criação duplicada. Ambos tratados na camada Application via `INotify`
- **Código em inglês**, comentários em português. Comunicação em português

## Multi-tenant

- Row-Level Security (RLS) via `SESSION_CONTEXT` do SQL Server
- Dois interceptors EF Core: `TenantSessionConnectionInterceptor` + `TenantSessionCommandInterceptor`
- `IRequestTenantContext` para requisições não autenticadas (login/register)
- JWT por tenant com rotação de chaves RSA. Master key da env var `JWT_MASTER_KEY`

## DI

- **Ponto único**: `VianaHub.Global.Gerit.Infra.IoC/DependencyInjection.cs`, método `AddGeritInfrastructure()`
- Política nomeada: `"BackOffice"` definida em `Program.cs`
- Notificações: `INotify` / `Notify` (Scoped)
- Usuário corrente: `ICurrentUserService` → `CurrentUserApiService`
- Localização: `ILocalizationService` → `LocalizationService` (Singleton)
- Provedor de segredos: `ISecretProvider` → `SecretProviderEnvironment` (lê env var `JWT_MASTER_KEY`)

## Arquivos de Instrução

- `.opencode/agents/*.md` — configuração dos agentes OpenCode via frontmatter YAML (modelos, tools, mode)
- `.opencode/instructions/kanban-flow.md` — fluxo Kanban compartilhado entre agentes
- `.opencode/agents/kanban-coordinator.md` — orquestrador central do fluxo (cria/move cards e invoca agentes)
- `.opencode/agents/po.md` — analisa demandas e escreve Tasks em BDD
- `.opencode/agents/developer-junior.md` — desenvolvedor de baixa complexidade
- `.opencode/agents/developer-pleno.md` — desenvolvedor de média complexidade
- `.opencode/agents/developer-senior.md` — desenvolvedor de alta complexidade/arquitetura
- `.opencode/agents/qa.md` — valida implementações e reporta resultados
- `docs/ARCHITECTURE.md` — guia completo de arquitetura
- `docs/CONTEXTO_COMPLETO_APLICACAO.md` — documento de contexto abrangente
- `src/VianaHub.Global.Gerit.Infra.IoC/DependencyInjection.cs` — fonte da verdade para DI

---

## Fluxo Kanban — Visão Geral

```
Usuário → Kanban Coordinator → PO → Kanban Coordinator → Developer → Kanban Coordinator → QA → Kanban Coordinator → Usuário
```

### Agentes e Responsabilidades

| Agente | Responsabilidade | Interage com o board? |
|--------|-----------------|----------------------|
| **Kanban Coordinator** | Orquestrador central: cria/move cards e invoca agentes | ✅ Sim — único responsável |
| **PO** | Analisa demanda, escreve Task em BDD, define complexidade | ❌ Não |
| **Developer Junior** | Implementa tarefas de baixa complexidade | ❌ Não |
| **Developer Pleno** | Implementa tarefas de média complexidade | ❌ Não |
| **Developer Senior** | Implementa tarefas de alta complexidade | ❌ Não |
| **QA** | Valida implementações e reporta resultados | ❌ Não |
| **Usuário** | Solicita demandas, revisa, aprova e mergea PRs | ❌ Não (via GitHub) |

> 📌 **O fluxo detalhado (passo a passo, com comandos e regras operacionais) está centralizado em:**
> **`.opencode/agents/kanban-coordinator.md`** — fonte da verdade.
> **`.opencode/instructions/kanban-flow.md`** — regras transversais partilhadas.

### GitHub Projects

**Board:** `https://github.com/users/vianahub-pt/projects/4`
**Repo:** `vianahub-pt/VianaHub.Global.Gerit`

> ⚠️ **Multi-repo:** Este board gerencia issues de VÁRIOS repositórios. Apenas o Kanban Coordinator interage com o board.

---

## Environment Variable

Agents must have `GH_TOKEN` set to interact with GitHub Projects:
```powershell
$env:GH_TOKEN = "ghp_..."  # classic token with 'project' scope
```
