# Auditoria de Performance — Padrões N+1

**Data:** 2026-07-17  
**Issue:** [#252](https://github.com/vianahub-pt/VianaHub.Global.Gerit/issues/252)  
**Versão analisada:** develop @ cd63d1b  
**Escopo:** Todos os repositórios em `src/VianaHub.Global.Gerit.Infra.Data/Repository/` + AppServices em `src/VianaHub.Global.Gerit.Application/Services/`

---

## Metodologia

Análise estática de todos os repositórios EF Core e AppServices, verificando:

1. Uso de `.Include()` com múltiplos relacionamentos
2. Presença/ausência de `.AsSplitQuery()` em queries com múltiplos Includes
3. Uso de `.AsNoTracking()` em queries de leitura
4. Loops com chamadas a repositórios nos AppServices (N+1 na camada de aplicação)

---

## 1. Resultado Geral

| Métrica | Valor |
|---|---|
| Total de repositórios analisados | 37 |
| Total de AppServices analisados | ~20 |
| Queries com `.Include()` | 225 ocorrências |
| Queries com `.AsSplitQuery()` | 23 ocorrências |
| Queries com `.AsNoTracking()` | 266 ocorrências (bom coverage) |
| N+1 identificados na camada de aplicação | 1 caso ALTO |
| Repositórios sem `AsSplitQuery` quando necessário | 13 |

---

## 2. Achados — Risco ALTO 🔴

### 2.1 AuthAppService — Loop N+1 nas Permissões (JWT)

**Arquivo:** `src/VianaHub.Global.Gerit.Application/Services/Identity/AuthAppService.cs`  
**Método:** `GenerateAccessTokenAsync()` (linhas 373–396)

```csharp
foreach (var role in userRoles ?? Enumerable.Empty<dynamic>())
{
    if (role?.Role == null) continue;
    var roleId = role.Role.Id;
    var rolePerms = await _rolePermissionRepo.GetByRoleAsync(roleId, user.TenantId, ct);
    // ...
}
```

**Problema:** Para cada role do utilizador, é feita uma query separada ao repositório de permissões. Um utilizador com 5 roles dispara 5 queries extras.

**Impacto:** A cada login/refresh, o tempo de geração do token aumenta linearmente com o número de roles.

**Solução recomendada:** Adicionar um método `GetByRoleIdsAsync` no `IRolePermissionDataRepository` que receba uma lista de roleIds e faça uma única query.

---

### 2.2 SubscriptionDataRepository — Múltiplos Includes sem Split

**Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Repository/Billing/SubscriptionDataRepository.cs`  
**Métodos afetados:** `GetByIdAsync`, `GetAllAsync`, `GetPagedAsync`, `GetByTenantIdAsync`, `GetBySubscriptionPlanIdAsync`, `GetActiveSubscriptionsAsync`, `GetExpiringSubscriptionsAsync`

**Padrão repetido (6×):**
```csharp
.Include(x => x.SubscriptionPlan)
    .ThenInclude(x => x.Translations)
.Include(x => x.Tenant)
.Include(x => x.StatusDefinition)
    .ThenInclude(x => x.Translations)
```

**Problema:** 3 joins laterais sem `.AsSplitQuery()`. O EF Core gera uma única query com JOINs que produzem um produto cartesiano. Com `SubscriptionPlan.Translations` (1:N) e `StatusDefinition.Translations` (1:N), o resultado pode ter `N × M` linhas por subscription.

**Solução recomendada:** Adicionar `.AsSplitQuery()` antes de `.FirstOrDefaultAsync()` ou `.ToListAsync()` em todos os 6 métodos.

---

### 2.3 UserDataRepository — Include de Coleção sem Split

**Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Repository/Identity/UserDataRepository.cs`  
**Métodos afetados:** `GetByIdAsync`, `GetByEmailAsync`, `GetAllAsync`, `GetPagedAsync`

**Padrão repetido (4×):**
```csharp
.Include(x => x.Tenant)
.Include(x => x.UserRoles)
    .ThenInclude(ur => ur.Role)
```

**Problema:** `UserRoles` é uma coleção (1:N). Sem `.AsSplitQuery()`, o EF Core gera uma única query com LEFT JOINs que multiplica as linhas.

**Solução recomendada:** Adicionar `.AsSplitQuery()` em todos os 4 métodos.

---

### 2.4 VisitTeamEmployeeDataRepository — Includes de Coleção sem Split

**Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Repository/Business/VisitTeamEmployeeDataRepository.cs`  
**Métodos afetados:** `GetByIdAsync`, `GetAllAsync`, `GetByVisitTeamIdAsync`, `GetByEmployeeIdAsync`, `GetActiveByVisitTeamIdAsync`, `GetPagedAsync`

**Padrões:**
- `GetByIdAsync`: 3 Includes (Employee, Function, VisitTeam)
- `GetAllAsync`: 2 Includes (Employee, Function)
- `GetByEmployeeIdAsync`: 3 Includes (VisitTeam→Visit, Function)

**Problema:** Múltiplos Includes aninhados sem `.AsSplitQuery()`. A query pode produzir produto cartesiano.

**Solução recomendada:** Adicionar `.AsSplitQuery()` em todos os métodos.

---

## 3. Achados — Risco MÉDIO 🟡

### 3.1 VisitTeamVehicleDataRepository — Sem AsSplitQuery

**Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Repository/Business/VisitTeamVehicleDataRepository.cs`  
**Métodos:** `GetByIdAsync`, `GetAllAsync`, `GetPagedAsync`  

3 Includes (Vehicle, VisitTeam, VisitTeam→Visit) sem `.AsSplitQuery()`.

---

### 3.2 VisitTeamEquipmentDataRepository — Sem AsSplitQuery

**Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Repository/Business/VisitTeamEquipmentDataRepository.cs`  
**Métodos:** `GetByIdAsync`, `GetAllAsync`, `GetPagedAsync`

3 Includes (Equipment, VisitTeam, VisitTeam→Visit) sem `.AsSplitQuery()`.

---

### 3.3 UserRoleDataRepository — Sem AsSplitQuery

**Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Repository/Identity/UserRoleDataRepository.cs`  
**Métodos:** `GetAllAsync`, `GetByIdAsync`, `GetByUserAsync`, `GetByRoleAsync`

2-3 Includes (Tenant, User, Role) sem `.AsSplitQuery()`. Volume de dados tipicamente baixo, mas o padrão deve ser consistente.

---

### 3.4 RolePermissionDataRepository — Sem AsSplitQuery

**Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Repository/Identity/RolePermissionDataRepository.cs`  
**Métodos:** `GetAllAsync`, `GetByIdAsync`, `GetByRoleAsync`, `GetByResourceAsync`

3-4 Includes (Tenant, Role, Resource, Action) sem `.AsSplitQuery()`.

---

### 3.5 VisitDataRepository — Include com StatusDefinition sem Split

**Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Repository/Business/VisitDataRepository.cs`  
**Métodos:** `GetByIdAsync`, `GetAllAsync`, `GetPagedAsync`

2 Includes (Client, StatusDefinition→Translations). `Translations` é uma coleção (1:N).

---

### 3.6 EquipmentDataRepository — Sem AsSplitQuery

**Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Repository/Business/EquipmentDataRepository.cs`  
**Métodos:** `GetByIdAsync`, `GetAllAsync`, `GetPagedAsync`

2 Includes (EquipmentType, StatusDefinition→Translations).

---

### 3.7 SubscriptionPlanFileRuleDataRepository — Sem AsSplitQuery

**Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Repository/Billing/SubscriptionPlanFileRuleDataRepository.cs`  
**Métodos:** `GetAllAsync`, `GetByIdAsync`, `GetPagedAsync`

2-3 Includes (SubscriptionPlan→Translations, FileType). `GetPagedAsync` usa `.Include(x => x.SubscriptionPlan)` sem `.ThenInclude(x => x.Translations)` — possível inconsistência.

---

### 3.8 AuthAppService.LoginAsync — Múltiplos Checks de Subscription

**Arquivo:** `src/VianaHub.Global.Gerit.Application/Services/Identity/AuthAppService.cs`  
**Método:** `LoginAsync()` (linhas 164–193)

6 chamadas sequenciais ao `ISubscriptionDomainService`: `IsActiveAsync`, `IsCanceledAsync`, `IsDeletedAsync`, `IsTrialAsync`, `IsTrialPeriodExpiredAsync`, `IsSubscriptionPeriodExpiredAsync`. Cada chamada dispara uma query `AnyAsync()` separada.

**Solução recomendada:** Consolidar num único método `GetSubscriptionStatusAsync(tenantId)` que retorne um DTO com todos os flags.

---

### 3.9 EmployeeTeamDataRepository — Sem AsSplitQuery

**Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Repository/Business/EmployeeTeamDataRepository.cs`  
**Métodos:** `GetByIdAsync`, `GetAllAsync`, `GetPagedAsync`

2 Includes (Team, Employee) sem `.AsSplitQuery()`.

---

### 3.10 VisitAttachmentDataRepository — Sem AsSplitQuery

**Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Repository/Business/VisitAttachmentDataRepository.cs`  
**Métodos:** Todos com Include

1-2 Includes (FileType, Visit) sem `.AsSplitQuery()`.

---

### 3.11 VisitAddressDataRepository — Sem AsSplitQuery

**Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Repository/Business/VisitAddressDataRepository.cs`  
**Métodos:** Todos com Include

2 Includes (Visit, AddressType) sem `.AsSplitQuery()`.

---

### 3.12 EmployeeAddressDataRepository — Sem AsSplitQuery

**Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Repository/Business/EmployeeAddressDataRepository.cs`  
**Métodos:** Todos com Include

2 Includes (AddressType, Employee) sem `.AsSplitQuery()`.

---

### 3.13 ClientAddressRepository — Sem AsSplitQuery

**Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Repository/Business/ClientAddressRepository.cs`  
**Métodos:** Todos com Include

1-2 Includes (Client, AddressType) sem `.AsSplitQuery()`.

---

### 3.14 VisitContactDataRepository — Sem AsSplitQuery

**Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Repository/Business/VisitContactDataRepository.cs`  
**Métodos:** Todos com Include

1 Include (Visit) sem `.AsSplitQuery()`. (Risco baixo, apenas 1 Include.)

---

## 4. Achados — Risco BAIXO 🟢

### 4.1 Repositórios com .AsSplitQuery() correto ✅

| Repositório | Métodos com Split |
|---|---|
| TenantAddressesDataRepository | 3 | 
| TenantFiscalDataDataRepository | 3 |
| TenantContactDataRepository | 6 |
| ClientContactRepository | 3 |
| ClientDataRepository | 2 (GetPaged) |
| ClientRepository | 3 |
| ClientFiscalDataDataRepository | 3 |

---

### 4.2 Repositórios com Include único — OK sem Split ✅

| Repositório | Inclui |
|---|---|
| EmployeeContactDataRepository | 1 (Employee) |
| VehicleDataRepository | 1 (StatusDefinition→Translations) |
| EmployeeDataRepository | 1 (StatusDefinition→Translations) |
| PlanDataRepository | 1 (Translations) |

---

### 4.3 .AsNoTracking() Coverage ✅

266 ocorrências em todas as queries de leitura. Nenhuma query de leitura encontrada **sem** `.AsNoTracking()`. Cobertura de 100%.

**Exceções corretas:** Métodos `AddAsync`, `UpdateAsync`, `DeleteAsync` não usam `.AsNoTracking()` (esperado, pois precisam de tracking para `SaveChangesAsync`).

---

## 5. Resumo por Gravidade

| Risco | Quantidade | Itens |
|---|---|---|
| 🔴 ALTO | 4 | AuthAppService (N+1 loop), SubscriptionDataRepository, UserDataRepository, VisitTeamEmployeeDataRepository |
| 🟡 MÉDIO | 14 | VisitTeamVehicle, VisitTeamEquipment, UserRole, RolePermission, Visit, Equipment, SubscriptionPlanFileRule, AuthAppService Login (subscription checks), EmployeeTeam, VisitAttachment, VisitAddress, EmployeeAddress, ClientAddress, VisitContact |
| 🟢 BAIXO / OK | 19+ | Restantes |

---

## 6. Recomendações

### Imediatas (neste sprint)

1. **AuthAppService.GenerateAccessTokenAsync** — Substituir loop N+1 por query com `roleIds IN (...)`. Adicionar método `GetByRoleIdsAsync(IEnumerable<int> roleIds, int tenantId, CancellationToken ct)` no `RolePermissionDataRepository`.

2. **SubscriptionDataRepository** — Adicionar `.AsSplitQuery()` nos 6 métodos com múltiplos Includes.

3. **UserDataRepository** — Adicionar `.AsSplitQuery()` nos 4 métodos com Include de `UserRoles` (coleção).

4. **VisitTeamEmployeeDataRepository** — Adicionar `.AsSplitQuery()` nos 6 métodos.

### Curto prazo (próximo sprint)

5. Padronizar `.AsSplitQuery()` em todos os repositórios com 2+ Includes aninhados (visit team, equipment, attachments, addresses).

6. **AuthAppService.LoginAsync** — Consolidar 6 verificações de subscription num único método `GetSubscriptionStatusAsync`.

### Médio prazo

7. Criar analisador estático (Roslyn analyzer) ou teste de arquitetura que obrigue `.AsSplitQuery()` quando houver 2+ `.Include()` na mesma query.

8. Considerar `.AsSplitQuery()` como padrão global do `GeritDbContext` via `OnConfiguring` para queries de leitura, com opt-out explícito.

---

## 7. Métricas de Risco

| Query | Multiplicação Cartesiana Potencial | Pior Caso (10 registos cada) |
|---|---|---|
| SubscriptionDataRepository (3 Includes, 2 coleções) | N × M (Translations) | 10 × 3 × 10 × 3 = ~900 linhas/registro |
| UserDataRepository (UserRoles + Role) | UserRoles × count | 10 roles por user × users = 10× linhas |
| AuthAppService (loop permissões) | N queries extras | 5 roles → 5 queries extras |

---

## 8. Conclusão

A aplicação tem uma cobertura excelente de `.AsNoTracking()` (100%), o que evita overhead de change tracking em queries de leitura. No entanto, **apenas 7 dos 37 repositórios (19%)** utilizam `.AsSplitQuery()`, criando risco de cartesian explosion em queries com múltiplos Includes, especialmente quando há coleções aninhadas (`.ThenInclude`).

O achado mais crítico está no `AuthAppService`, onde um loop N+1 nas permissões impacta diretamente o tempo de login de cada utilizador.

**Recomendação global:** Adotar `.AsSplitQuery()` como padrão para qualquer query com 2+ `.Include()` ou com `.ThenInclude()` em coleções (1:N).
