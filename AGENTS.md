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
| `src/*.Domain` | Entidades, serviços de domínio, validadores, interfaces. Rich domain model. |
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

- Arquivo: `{Entidade}Endpoint.cs` (singular). Classe: `{Entidade}Endpoint` (singular).
- Método: `Map{Entidade}Endpoints` (plural), anotado com `[EndpointMapper]`.
- Auto-descoberta via `MapEndpointsFromAssembly()` em `Program.cs`.
- Agrupados em `Endpoints/{Billing,Identity,Business,Job}/`.
- Política de autorização: `"BackOffice"` (requer usuário autenticado).

## Regras de Arquitetura (restrições rígidas)

- **Sem `throw` para mensagens ao usuário** — use `INotify` para acumular mensagens + status HTTP. Exceções são apenas para falhas técnicas (capturadas pelo `GlobalExceptionMiddleware`).
- **Todas as strings para o usuário DEVEM usar chaves de localização** — nunca mensagens hardcoded. Chaves em `src/*.Api/Localization/` por idioma (pt-PT, en-US, es-ES). Cultura do header `Accept-Language`, fallback pt-PT.
- **410 Gone** para recursos removidos/desativados por ID. **409 Conflict** para criação duplicada. Ambos tratados na camada Application via `INotify`.
- **Código em inglês**, comentários em português. Comunicação em português.

## Multi-tenant

- Row-Level Security (RLS) via `SESSION_CONTEXT` do SQL Server.
- Dois interceptors EF Core: `TenantSessionConnectionInterceptor` + `TenantSessionCommandInterceptor`.
- `IRequestTenantContext` para requisições não autenticadas (login/register).
- JWT por tenant com rotação de chaves RSA. Master key da env var `JWT_MASTER_KEY`.

## DI

- **Ponto único**: `VianaHub.Global.Gerit.Infra.IoC/DependencyInjection.cs`, método `AddGeritInfrastructure()`.
- Política nomeada: `"BackOffice"` definida em `Program.cs`.
- Notificações: `INotify` / `Notify` (Scoped).
- Usuário corrente: `ICurrentUserService` → `CurrentUserApiService`.
- Localização: `ILocalizationService` → `LocalizationService` (Singleton).
- Provedor de segredos: `ISecretProvider` → `SecretProviderEnvironment` (lê env var `JWT_MASTER_KEY`).

## Arquivos de Instrução

- `.opencode/opencode.json` — configuração principal do OpenCode
- `.opencode/instructions/kanban-flow.md` — fluxo Kanban compartilhado entre agentes
- `.opencode/agents/kanban-coordinator.md` — orquestrador do fluxo PO → Developer → QA
- `.opencode/agents/po.md` — formato de histórias de usuário e issues
- `.opencode/agents/developer-junior.md` — desenvolvedor de baixa complexidade
- `.opencode/agents/developer-pleno.md` — desenvolvedor de média complexidade
- `.opencode/agents/developer-senior.md` — desenvolvedor de alta complexidade/arquitetura
- `.opencode/agents/qa.md` — checklist de validação e testes
- `docs/ARCHITECTURE.md` — guia completo de arquitetura
- `docs/CONTEXTO_COMPLETO_APLICACAO.md` — documento de contexto abrangente
- `src/VianaHub.Global.Gerit.Infra.IoC/DependencyInjection.cs` — fonte da verdade para DI

---

## GitHub Projects

**Board:** `https://github.com/users/vianahub-pt/projects/1`
**Repo:** `vianahub-pt/VianaHub.Global.Gerit`

### Project IDs (para comandos `gh`)

| Campo | ID |
|-------|-----|
| Project ID | `PVT_kwHODGRT384BZCnv` |
| Status Field ID | `PVTSSF_lAHODGRT384BZCnvzhUEIlE` |

### Status Option IDs (Colunas Kanban)

| Coluna | Option ID | Responsável | Quando o card vai para cá | Ação |
|--------|-----------|-------------|---------------------------|------|
| **Backlog** | `f75ad846` | PO | Card criado | Cria issue no GitHub, documenta |
| **To do** | `eda9b53c` | PO → Developer | Card pronto para dev | Developer cria branch e implementa |
| **In Progress** | `47fc9ee4` | Developer | Developer pega o card | Implementa, testa, faz commit |
| **For Tests** | `a42b88c6` | Developer → QA | Developer termina | Passa para QA validar |
| **In Test** | `94a9d6f6` | QA | QA pega o card | Testa, valida, gera relatório |
| **For Deploy** | `add10e44` | QA → DevOps | QA aprova | Passa para deploy |
| **Done** | `98236657` | DevOps | Deploy completo | Card finalizado |

## Automação do Fluxo Kanban (REGRAS INVIOLÁVEIS)

### Fluxo Completo

```
Backlog → To do → In Progress → For Tests → In Test → Done → For Deploy → Done (Deployed)
```

### Cadeia de Automação entre Agentes

```
PO → DEVELOPER → QA → Done → Humano (só aprova PR)
```

**Regra de ouro: O humano SÓ aprova o PR. NUNCA faz merge, NUNCA implementa, NUNCA testa.**

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
│  - NUNCA move cards no board                                │
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
PO -> Kanban Coordinator -> Developer Junior | Developer Pleno | Developer Senior -> QA
Backlog -> To do -> In Progress -> For Tests -> In Test -> For Deploy -> Done
```

| Agente | Colunas | Ação | Automação |
|--------|---------|------|-----------|
| **PO** | Backlog → To do | Cria issue, documenta, move para To do | Entrega para o Kanban Coordinator |
| **Kanban Coordinator** | To do | Classifica complexidade, escolhe Developer | Aciona Developer Junior/Pleno/Senior |
| **Developer Junior** | To do → In Progress → For Tests | Implementa tarefas simples | Aciona QA ao mover para For Tests |
| **Developer Pleno** | To do → In Progress → For Tests | Implementa tarefas intermediárias | Aciona QA ao mover para For Tests |
| **Developer Senior** | To do → In Progress → For Tests | Implementa tarefas complexas/arquiteturais | Aciona QA ao mover para For Tests |
| **QA** | For Tests → In Test → For Deploy | Valida, testa, gera relatório | Move para For Deploy (aprovado) ou In Progress (reprovado) |
| **Humano** | For Deploy | **SÓ aprova o PR** (merge feature → develop) | **MANUAL — Nunca feito por agentes** |

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
