# Gerit API

Construída com **.NET 8**, seguindo **Arquitetura Hexagonal**, **DDD**, **SOLID** e **Clean Architecture**.

## Project

- **Solution**: `VianaHub.Global.Gerit.sln` — .NET 8 ASP.NET Core Minimal API
- **Architecture**: DDD + Clean Architecture + Hexagonal, multi-tenant SaaS (Gerit)
- **Stack**: EF Core 8 + SQL Server, FluentValidation 12, AutoMapper, Serilog, Hangfire, JWT RS256 (per-tenant keys), OpenTelemetry

## Solution structure

| Project | Responsibility |
|---|---|
| `src/*.Api` | Minimal API endpoints (`[EndpointMapper]` + `MapEndpointsFromAssembly()`), Swagger, middleware |
| `src/*.Application` | Use-case orchestration, DTOs, AutoMapper profiles, semantic HTTP codes (409, 410) |
| `src/*.Domain` | Entities, domain services, validators, interfaces. Rich domain model. |
| `src/*.Infra.Data` | EF Core DbContext, SQL Server, mappings, repositories, tenant interceptors |
| `src/*.Infra.IoC` | Single `DependencyInjection.cs` — all DI registration |
| `src/*.Infra.Integration` | External integrations (NoOpEmailSender) |
| `src/*.Infra.Job` | Hangfire jobs, hosted services |
| `tests/*.Tests` | xUnit + Moq + NBuilder + coverlet |

Leftover folders (not .csproj projects): `Application.Services`, `Infra.Data.Repository.Business`, `Infra.Messaging` — may hold code in transition.

## Commands

```powershell
dotnet build                      # build solution
dotnet test                       # run all tests (xUnit)
dotnet test --filter "Category=Unit"
dotnet run --project src/*.Api    # run API (launches Swagger at /swagger)
```

## Endpoint conventions

- File: `{Entity}Endpoint.cs` (singular). Class: `{Entity}Endpoint` (singular).
- Method: `Map{Entity}Endpoints` (plural), annotated with `[EndpointMapper]`.
- Auto-discovered via `MapEndpointsFromAssembly()` in `Program.cs`.
- Grouped in `Endpoints/{Billing,Identity,Business,Job}/`.
- Auth policy: `"BackOffice"` (requires authenticated user).

## Architecture rules (hard constraints)

- **No `throw` for user-facing messages** — use `INotify` to accumulate messages + HTTP status. Exceptions are for technical failures only (captured by `GlobalExceptionMiddleware`).
- **All user-facing strings must use localization keys** — never hardcode messages. Keys in `src/*.Api/Localization/` per language (pt-PT, en-US, es-ES). Culture from `Accept-Language` header, fallback pt-PT.
- **410 Gone** for missing/deactivated resources by ID. **409 Conflict** for duplicate creation. Both handled in Application layer via `INotify`.
- **Code in English**, comments in Portuguese (Brazilian Portuguese). Communication in Portuguese.

## Multi-tenant

- Row-Level Security (RLS) via SQL Server `SESSION_CONTEXT`.
- Two EF Core interceptors: `TenantSessionConnectionInterceptor` + `TenantSessionCommandInterceptor`.
- `IRequestTenantContext` for unauthenticated requests (login/register).
- JWT per tenant with RSA key rotation. Master key from env var `JWT_MASTER_KEY`.

## DI

- **Single entry point**: `VianaHub.Global.Gerit.Infra.IoC/DependencyInjection.cs`, method `AddGeritInfrastructure()`.
- Named policy: `"BackOffice"` defined in `Program.cs`.
- Notifications: `INotify` / `Notify` (Scoped).
- Current user: `ICurrentUserService` → `CurrentUserApiService`.
- Localization: `ILocalizationService` → `LocalizationService` (Singleton).
- Secret provider: `ISecretProvider` → `SecretProviderEnvironment` (reads `JWT_MASTER_KEY` env var).

## Existing instruction files

- `.opencode/agents/developer.md` — dev workflow, layer conventions
- `.opencode/agents/po.md` — user story format
- `.opencode/agents/qa.md` — validation checklist
- `docs/ARCHITECTURE.md` — full architecture guide
- `docs/CONTEXTO_COMPLETO_APLICACAO.md` — comprehensive context document
- `src/VianaHub.Global.Gerit.Infra.IoC/DependencyInjection.cs` — source of truth for DI wiring

---

## GitHub Projects

**Board:** `https://github.com/users/vianahub-pt/projects/1`
**Repo:** `vianahub-pt/VianaHub.Global.Gerit`

### Project IDs (for `gh` commands)

| Field | ID |
|-------|-----|
| Project ID | `PVT_kwHODGRT384BZCnv` |
| Status Field ID | `PVTSSF_lAHODGRT384BZCnvzhUEIlE` |

### Status Option IDs (Kanban Columns)

| Coluna | Option ID | Responsável | Quando card vai para cá | Ação |
|--------|-----------|-------------|------------------------|------|
| **Backlog** | `f75ad846` | PO | Card criado | Cria issue no GitHub, documenta |
| **To do** | `eda9b53c` | PO → Developer | Card pronto para dev | Developer cria branch e implementa |
| **In Progress** | `47fc9ee4` | Developer | Developer pega o card | Implementa, testa, faz commit |
| **For Tests** | `a42b88c6` | Developer → QA | Developer termina | Passa para QA validar |
| **In Test** | `94a9d6f6` | QA | QA pega o card | Testa, valida, gera relatório |
| **For Deploy** | `add10e44` | QA → DevOps | QA aprova | Passa para deploy |
| **Done** | `98236657` | DevOps | Deploy completo | Card finalizado |

---

## Automação do Fluxo Kanban (REGRAS INVIOLÁVEIS)

### Fluxo Completo

```
Backlog → To do → In Progress → For Tests → In Test → Done → For Deploy → Done (Deployed)
```

### Cadeia de Automação entre Agentes

```
PO → DEVELOPER → QA → Done → Humano (só aprova PR)
```

**REgra de ouro: O humano SÓ aprova o PR. NUNCA faz merge, NUNCA implementa, NUNCA testa.**

### Regras de Automação (CRÍTICAS)

| Transição | Gatilho | Ação Automática |
|-----------|---------|-----------------|
| **PO → Developer** | PO move card para "To do" | PO aciona agente Developer via task tool |
| **Developer → QA** | Developer move card para "For Tests" | Developer aciona agente QA via task tool |
| **QA → Done** | QA valida tudo (build + testes OK) | QA move card para "Done" |
| **Done → Humano** | Card em "Done" | Humano aprova PR no GitHub |
| **Humano → Deploy** | PR aprovado | Deploy automático |

### Intervenção Humana (ÚNICA PERMITIDA)

```
┌─────────────────────────────────────────────────────────────┐
│  O HUMANO SÓ PODE FAZER UMA COISA: APROVAR O PR NO GITHUB  │
│                                                             │
│  - NUNCA implementa código                                   │
│  - NUNCA testa                                              │
│  - NUNCA faz merge                                          │
│  - NÚNCA move cards no board                                │
│  - NUNCA invoca agentes                                     │
│                                                             │
│  TODO O RESTO É AUTOMÁTICO ENTRE AGENTES                    │
└─────────────────────────────────────────────────────────────┘
```

### Fluxo Visual de Automação

```
┌──────┐    ┌────────┐    ┌──────────┐    ┌──────────┐    ┌────────┐    ┌──────┐    ┌─────────┐    ┌──────┐
│      │    │        │    │          │    │          │    │        │    │      │    │         │    │      │
│Backlog│───▶│ To do  │───▶│In Progress│───▶│For Tests │───▶│In Test │───▶│ Done │───▶│For Deploy│───▶│Done  │
│      │    │        │    │          │    │          │    │        │    │      │    │         │    │      │
└──────┘    └────────┘    └──────────┘    └──────────┘    └────────┘    └──────┘    └─────────┘    └──────┘
   PO          PO            Developer       Developer         QA           QA          DevOps        DevOps
              │              │               │                │            │             │             │
              │              │               │                │            │             │             │
              ▼              ▼               ▼                ▼            ▼             ▼             ▼
           Aciona         Aciona          Aciona           Move para    Move para    Deploy auto   Deploy
           Developer      Developer       QA               Done         Done         + Done        completo
```

### Orquestração entre Subagentes

O workflow segue uma cadeia estrita através das colunas do Kanban. **A AUTOMAÇÃO É OBRIGATÓRIA** — os agentes devem invocar automaticamente o próximo agente da cadeia sem intervenção humana.

```
PO → DEVELOPER → QA → Done → Human (PR Approval Only)
Backlog → To do → In Progress → For Tests → In Test → Done → For Deploy → Done
```

### Cadeia Automatizada de Agentes

| Agente | Colunas | Ação | Automação |
|--------|---------|------|-----------|
| **PO** | Backlog → To do | Cria issue, documenta, move para To do | **AC automaticamente o DEVELOPER** |
| **DEVELOPER** | To do → In Progress → For Tests | Implementa, testa, commita, cria PR | **AC automaticamente o QA** |
| **QA** | For Tests → In Test → Done | Valida, testa, gera relatório | **Move para Done automaticamente** |
| **Humano** | Done | **SÓ aprova o PR** (merge feature → develop) | **MANUAL — Nunca feito por agentes** |

### Regras de Automação (CRÍTICAS)

1. **PO → DEVELOPER**: Quando o PO termina de criar issues e move para "To do", o PO **DEVE** automaticamente chamar o agente DEVELOPER usando a task tool. Sem intervenção humana.

2. **DEVELOPER → QA**: Quando o DEVELOPER termina de implementar e move para "For Tests", o DEVELOPER **DEVE** automaticamente chamar o agente QA usando a task tool. Sem intervenção humana.

3. **QA → Done**: Quando o QA termina de validar e tudo está OK, move para "Done" automaticamente. Se encontrar bugs, move para "In Progress" e aciona o DEVELOPER automaticamente.

4. **Aprovação de PR pelo Humano**: A **ÚNICA** intervenção humana é aprovar o Pull Request (merge da feature branch para develop). Agentes **NUNCA** aprovam PRs.

5. **Sem Pular Etapas**: Cards devem seguir a ordem: Backlog → To do → In Progress → For Tests → In Test → Done → For Deploy → Done

6. **Sem Intervenção Humana no Fluxo de Agentes**: Humanos NÃO invocam agentes. Agentes invocam uns aos outros automaticamente.

### Fluxo de Aprovação de PR

```
DEVELOPER cria PR (feature/issue-XXX → develop)
    ↓
Humano revisa e APROVA o PR (merge)
    ↓
Deploy automático para produção
    ↓
Card move para Done
```

**Regras:**
- Agentes criam PRs mas **NUNCA** aprovam ou fazem merge
- Apenas humanos podem aprovar PRs (merge de feature para develop)
- Após PR aprovado, deploy automático é executado e card move para Done
- Todas as issues devem viver no GitHub Projects

### Environment Variable

Agents must have `GH_TOKEN` set to interact with GitHub Projects:
```powershell
$env:GH_TOKEN = "ghp_..."  # classic token with 'project' scope
```
