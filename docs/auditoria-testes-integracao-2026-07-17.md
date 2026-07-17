# Auditoria de Testes de Integração — VianaHub Global Gerit

**Data:** 2026-07-17
**Issue:** vianahub-pt/VianaHub.Global.Gerit#251
**Escopo:** Verificar a cobertura de testes de integração para os recursos existentes, comparando App Services com testes correspondentes e estimando o esforço para atingir 80% de cobertura.

---

## 1. Contexto

A arquitetura do projeto (DDD + Clean Architecture + Hexagonal) tem 7 camadas testáveis: `Api`, `Application`, `Domain`, `Infra.Data`, `Infra.Integration`, `Infra.IoC` e `Infra.Job`. Cada camada expõe contratos públicos (endpoints, App Services, Domain Services, Repositories, Entities) que devem ser cobertos por testes unitários e/ou de integração.

Esta auditoria cruza os recursos públicos da API (`Api/Endpoints` + `Application/Services`) com os arquivos de teste existentes para identificar gaps e estimar o esforço de cobertura de 80%.

---

## 2. Resumo Executivo

| Camada | Recursos Auditados | Com Teste | Sem Teste | Cobertura |
|---|---|---|---|---|
| `Api` (Endpoints) | 42 | 0 | 42 | **0%** |
| `Application` (App Services) | 40 | 3 | 37 | **7,5%** |
| `Domain` (Entities + Services) | 100 | 1 | 99 | **1%** |
| `Infra.Data` (Repositories) | 45 | 1 | 44 | **2,2%** |
| `Infra.Integration` | — | — | — | — |
| `Infra.IoC` | — | — | — | — |
| `Infra.Job` | — | — | — | — |
| **TOTAL** | **227** | **5** | **222** | **~2,2%** |

> Apenas 5 classes de produção têm pelo menos um arquivo de teste dedicado (3 App Services + 1 Entity + 1 Repository). Os demais 222 recursos estão completamente descobertos.

### Métricas de Cobertura (Coverlet + XPlat)

| Projeto | Line Rate | Branch Rate |
|---|---|---|
| `VianaHub.Global.Gerit.Api` | 11,12% | 2,22% |
| `VianaHub.Global.Gerit.Application` | 2,75% | 2,38% |
| `VianaHub.Global.Gerit.Domain` | 3,09% | 2,53% |
| `VianaHub.Global.Gerit.Infra.Data` | 46,41% | 2,66% |
| `VianaHub.Global.Gerit.Infra.Integration` | 0% | 0% |
| `VianaHub.Global.Gerit.Infra.IoC` | 0% | 0% |
| `VianaHub.Global.Gerit.Infra.Job` | 0% | 0% |
| **TOTAL** | **13,73%** | **2,37%** |

### Estado de Execução

```
Aprovado!  – Com falha:     0, Aprovado:    32, Ignorado:     0, Total:    32
```

- **32 testes passando** (100%)
- **0 testes falhando**
- **0 testes ignorados**
- Cobertura geral muito abaixo do mínimo aceitável de 80% para um sistema SaaS multi-tenant.

---

## 3. Inventário Completo de Testes Existentes

### 3.1. Testes por Camada

| Arquivo | Camada | Tipo | Testes (estimado) |
|---|---|---|---|
| `Api/Configuration/CorsSetupTests.cs` | Api | Unitário (config) | 1 |
| `Api/Configuration/SerilogConfigurationTests.cs` | Api | Unitário (config) | 1 |
| `Api/Configuration/SwaggerSetupTests.cs` | Api | Unitário (config) | 1 |
| `Api/Converters/GuidNoDashesConverterTests.cs` | Api | Unitário (utilitário) | 1 |
| `Api/Endpoints/EndpointsRegistrationTests.cs` | Api | Unitário (registro) | 5+ |
| `Application/Services/Business/ClientAppServiceTests.cs` | Application | Unitário (AppService) | 2 |
| `Application/Services/Business/EquipmentAppServiceTests.cs` | Application | Unitário (AppService) | 2 |
| `Application/Services/Business/VehicleAppServiceTests.cs` | Application | Unitário (AppService) | 2 |
| `Domain/Entities/Business/ClientEntityTests.cs` | Domain | Unitário (Entity) | 5+ |
| `Infra/Data/Repository/Business/ClientRepositoryTests.cs` | Infra.Data | In-Memory (EF Core) | 4+ |
| **TOTAL** | — | — | **~32** |

> Nota: Os testes atuais cobrem apenas 3 dos 40 App Services (Client, Equipment, Vehicle) e 1 dos 100+ Domain Services. Não há testes de integração HTTP ponta-a-ponta (WebApplicationFactory + TestServer).

### 3.2. Características dos Testes

- **Framework:** xUnit + Moq + NBuilder + FluentAssertions (parcial).
- **Padrão AAA:** Arrange / Act / Assert.
- **Mocks:** App Services mockam `IRepository`, `IDomainService`, `IMapper`, `INotify`, `ILocalizationService`, `ICurrentUserService`, `IFileValidationService`.
- **In-Memory DB:** Apenas `ClientRepositoryTests` usa `UseInMemoryDatabase` do EF Core. Os demais App Services usam mocks puros.
- **Testes HTTP:** Nenhum. Não há `WebApplicationFactory<Program>`, `TestServer` nem `HttpClient` para testes ponta-a-ponta dos endpoints.

---

## 4. Gap Analysis: App Services vs Testes

### 4.1. App Services com Teste ✅

| App Service | Teste | Cobertura do AppService |
|---|---|---|
| `ClientAppService` | `ClientAppServiceTests.cs` | ~2 cenários (GetById 410 + Create OK) |
| `EquipmentAppService` | `EquipmentAppServiceTests.cs` | ~2 cenários (BulkUpload sem ficheiro + CSV válido) |
| `VehicleAppService` | `VehicleAppServiceTests.cs` | ~2 cenários (BulkUpload sem ficheiro + CSV válido) |

### 4.2. App Services SEM Teste ❌ (37 pendentes)

**Identity (10):**
- `ActionAppService`
- `AuthAppService`
- `JwtKeyAppService`
- `ResourceAppService`
- `RoleAppService`
- `RolePermissionAppService`
- `UserAppService`
- `UserPreferencesAppService`
- `UserRoleAppService`

**Billing (7):**
- `PlanAppService`
- `SubscriptionAppService`
- `TenantAddressesAppService`
- `TenantAppService`
- `TenantContactAppService`
- `TenantFiscalDataAppService`

**Business (20):**
- `AcquisitionSourceTypeAppService`
- `AddressTypeAppService`
- `ClientAddressAppService`
- `ClientContactAppService`
- `ClientFiscalDataAppService`
- `DocumentTypeAppService`
- `EmployeeAddressAppService`
- `EmployeeAppService`
- `EmployeeContactAppService`
- `EmployeeTeamAppService`
- `EquipmentTypeAppService`
- `FileTypeAppService`
- `StatusDefinitionAppService`
- `TeamAppService`
- `VisitAddressAppService`
- `VisitAppService`
- `VisitAttachmentAppService`
- `VisitContactAppService`
- `VisitTeamAppService`
- `VisitTeamEmployeeAppService`
- `VisitTeamEquipmentAppService`
- `VisitTeamFunctionAppService`
- `VisitTeamVehicleAppService`

**Job (1):**
- `JobAppService`

> Total: **37 App Services** sem cobertura. A maioria cobre CRUD completo (GetAll, GetById, Create, Update, Activate, Deactivate, Delete, BulkUpload) — em média 8 cenários por AppService = **~296 cenários de teste pendentes** apenas na camada Application.

---

## 5. Gap Analysis: Endpoints vs Testes

### 5.1. Endpoints Auditados (42)

**Identity (9):** Auth, User, UserPreferences, UserRole, Role, RolePermission, Resource, Action, JwtKey.
**Billing (6):** Tenant, TenantAddress, TenantContact, TenantFiscalData, Plan, Subscription.
**Business (26):** Client, ClientAddress, ClientContact, ClientFiscalData, Employee, EmployeeAddress, EmployeeContact, EmployeeTeams, Visit, VisitAddress, VisitContact, VisitAttachment, VisitTeam, VisitTeamEmployee, VisitTeamVehicles, VisitTeamEquipments, VisitTeamFunction, Team, Vehicle, Equipment, EquipmentType, FileType, AddressType, DocumentType, StatusDefinition, AcquisitionSourceType.
**Job (1):** Job.

### 5.2. Estado Atual

- **0 endpoints** têm teste de integração HTTP ponta-a-ponta.
- O único teste da camada `Api/Endpoints` é `EndpointsRegistrationTests.cs`, que valida **apenas o registro** dos endpoints via `IEndpointRouteBuilder` mockado — não exercita o pipeline real (autenticação JWT, validação FluentValidation, RLS, INotify → códigos HTTP 409/410).

> **Gap crítico:** Não há cobertura do comportamento HTTP real (status codes, payloads, headers, autenticação).

---

## 6. Estimativa de Esforço para 80% de Cobertura

### 6.1. Premissas

- **Cobertura atual:** 13,73% (line) / 2,37% (branch).
- **Meta:** 80% (line) / 70% (branch) — meta conservadora para SaaS multi-tenant.
- **Velocidade por cenário (média):** 15 minutos para App Service mockado; 25 minutos para Repository com In-Memory; 35 minutos para endpoint HTTP ponta-a-ponta.
- **Cenários por AppService (padrão CRUD):** 8 — `GetAll_Paged`, `GetById_Ok`, `GetById_410`, `Create_Ok`, `Create_409_Duplicate`, `Update_Ok`, `Activate_Ok`, `Deactivate_Ok`, `Delete_Ok`, `Delete_410`.
- **Cenários por Repository (padrão CRUD):** 6 — `GetPaged`, `GetById_Ok`, `GetById_NotFound`, `Add`, `Update`, `Delete`.
- **Cenários por Endpoint HTTP:** 10 — happy path + 4xx (400 validação, 401 auth, 403 policy, 404, 409) + 410.

### 6.2. Estimativa por Camada

| Camada | Classes Pendentes | Cenários Pendentes | Horas Estimadas |
|---|---|---|---|
| `Application` (App Services) | 37 | ~296 | **74h** |
| `Infra.Data` (Repositories) | 44 | ~264 | **110h** |
| `Domain` (Services + Validators + Entities) | 99 | ~200 | **50h** |
| `Api` (Endpoints HTTP) | 42 | ~400 | **~210h** (com WebApplicationFactory + JWT mock) |
| `Infra.Job` (Hangfire) | 1+ | ~10 | **4h** |
| `Infra.Integration` (NoOpEmailSender) | 1 | ~3 | **1h** |
| **TOTAL para 80% de cobertura** | **~224** | **~1.173** | **~449h** |

> **~449 horas úteis = ~56 dias úteis = ~11,2 semanas (1 dev a 100%)** = aproximadamente **2,8 meses**.
> Em equipa de 2 devs: **~5,6 semanas**.

### 6.3. Roadmap Sugerido (em sprints de 2 semanas)

| Sprint | Foco | Entregáveis | Cobertura Estimada |
|---|---|---|---|
| **S1** | Fundação | Testes para Auth, Tenant, Client (App + Repo + HTTP) | ~25% |
| **S2** | Identity | User, Role, Resource, Action, JwtKey, UserPreferences, UserRole | ~40% |
| **S3** | Billing | Plan, Subscription, TenantAddress, TenantContact, TenantFiscalData | ~55% |
| **S4** | Business core | Employee, Team, Equipment, Vehicle, EquipmentType, FileType | ~70% |
| **S5** | Visit | Visit, VisitTeam, VisitAddress, VisitContact, VisitAttachment | ~80% |
| **S6** | Estabilização | Validadores, Domain Services remanescentes, cobertura branch | **~85%** |

---

## 7. Recomendações

### 7.1. Curto Prazo (Próximas 2 sprints)

1. **Criar `TestServerFactory` base** com `WebApplicationFactory<Program>` e mocks de `JWT_MASTER_KEY`, `ILocalizationService` e `IRequestTenantContext` para suportar testes HTTP ponta-a-ponta.
2. **Padronizar padrão de teste** para App Services: base class `AppServiceTestsBase<TService, TRepository, TEntity, TCreateRequest, TUpdateRequest, TResponse>` para reduzir boilerplate (template já demonstrado em `ClientAppServiceTests`).
3. **Adicionar `[Trait("Category", "Integration")]`** aos testes HTTP para permitir filtragem por categoria no CI.
4. **Configurar Coverlet threshold no CI** com gate de mínimo 60% de line coverage na próxima sprint e 80% na S5.

### 7.2. Médio Prazo (3-6 sprints)

5. **Testes de carga de fixtures** — NBuilder para gerar entidades de teste de forma fluente.
6. **Testes parametrizados** (`[Theory]`) para validadores FluentValidation cobrindo combinações de regras.
7. **Snapshot tests** para DTOs/Responses com `Verify` ou `SnapshotTesting`.
8. **Mutation testing** com Stryker.NET para validar a qualidade real dos testes (não apenas cobertura).

### 7.3. Longo Prazo

9. **Contract tests** com Pact para garantir compatibilidade API ↔ consumidores.
10. **Testes E2E** com Playwright para fluxos críticos (login → CRUD → logout).
11. **Performance tests** com k6 ou NBomber para validar SLAs dos endpoints.

---

## 8. Conclusão

| Indicador | Estado Atual | Meta | Gap |
|---|---|---|---|
| Testes passando | 32 | — | — |
| Cobertura de linhas | 13,73% | 80% | -66,27pp |
| Cobertura de branches | 2,37% | 70% | -67,63pp |
| App Services com teste | 3 / 40 | 40 / 40 | -37 |
| Endpoints com teste HTTP | 0 / 42 | 42 / 42 | -42 |
| Esforço para 80% | — | — | **~449h** |

**Veredito:** A cobertura de testes é **insuficiente** para um SaaS multi-tenant em produção. Embora os 32 testes existentes passem 100%, eles cobrem **apenas ~2,2% dos recursos** públicos. Recomenda-se o investimento de **~449 horas** distribuídas em **6 sprints de 2 semanas** (1 dev) ou **3 sprints** (2 devs) para atingir a meta de 80% de cobertura com testes de qualidade (não apenas cobertura superficial).

> ⚠️ Esta é uma **auditoria de complexidade ALTA**. Recomenda-se que a equipa priorize testes de integração HTTP ponta-a-ponta (Sprint 1) antes de aprofundar testes unitários de Domain Services, dado o maior retorno sobre risco.

---

**Anexos:**

- `tests/VianaHub.Global.Gerit.Tests/TestResults/*/coverage.cobertura.xml` — relatório bruto de cobertura.
- `docs/auditoria-unique-indexes-2026-07-17.md` — auditoria anterior (issue #250) para correlação.
