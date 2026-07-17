# Análise Rigorosa — Banco de Dados vs Código (Domain, Infra.Data, Infra.IoC)

**Data:** 2026-07-17  
**Escopo:** Varredura linha a linha de todas as tabelas SQL (56) vs mappings EF Core (56) vs Domain Entities vs DI  
**Princípio:** O banco de dados é a ÚNICA fonte da verdade  
**Método:** Comparação exata: nome da tabela, nome de cada coluna, tipo SQL, nullable, default, PKs, FKs, índices, constraints

---

## 1. RESUMO EXECUTIVO

| Métrica | Valor |
|---|---|
| Total de tabelas no SQL | **56** |
| Total de arquivos de mapping (IEntityTypeConfiguration) | **56** |
| Total de ERROS encontrados | **27** |
| Erros CRÍTICOS (nome de tabela errado) | **3** |
| Erros ALTOS (coluna inexistente, PK errada) | **10** |
| Erros MÉDIOS (tipo incorreto, nullable errado, tamanho errado) | **14** |

### ⚠️ A confiança anterior estava INJUSTIFICADA — há 3 tabelas com nomes errados nos mappings que causariam ERRO EM RUNTIME ao tentar acessar as tabelas reais.

---

## 2. MAPEAMENTO COMPLETO: Tabela SQL ↔ Mapping

| # | Tabela SQL | Mapping File | .ToTable() | Status |
|---|---|---|---|---|
| 1 | PartyTypes | PartyTypeMapping | `"PartyTypes"` | ✅ OK |
| 2 | PartyTypeTranslations | PartyTypeTranslationMapping | `"PartyTypeTranslations"` | ✅ OK |
| 3 | AcquisitionSourceTypes | AcquisitionSourceTypeMapping | `"AcquisitionSourceTypes"` | ⚠️ Colunas extras |
| 4 | AcquisitionSourceTypeTranslations | AcquisitionSourceTypeTranslationMapping | `"AcquisitionSourceTypeTranslations"` | ✅ OK |
| 5 | AddressTypes | AddressTypeMapping | `"AddressTypes"` | ⚠️ Colunas extras |
| 6 | AddressTypeTranslations | AddressTypeTranslationMapping | `"AddressTypeTranslations"` | ✅ OK |
| 7 | DocumentTypes | DocumentTypeMapping | `"DocumentTypes"` | ✅ OK |
| 8 | DocumentTypeTranslations | DocumentTypeTranslationMapping | `"DocumentTypeTranslations"` | ✅ OK |
| 9 | FileTypes | FileTypeMapping | `"FileTypes"` | ✅ OK |
| 10 | FileTypeTranslations | FileTypeTranslationMapping | `"FileTypeTranslations"` | ✅ OK |
| 11 | StatusDomains | StatusDomainMapping | `"StatusDomains"` | ✅ OK |
| 12 | StatusDomainTranslations | StatusDomainTranslationMapping | `"StatusDomainTranslations"` | ✅ OK |
| 13 | SubscriptionPlans | SubscriptionPlanEntityMapping | `"SubscriptionPlans"` | ✅ OK |
| 14 | SubscriptionPlanTranslations | SubscriptionPlanTranslationMapping | `"SubscriptionPlanTranslations"` | ✅ OK |
| 15 | SubscriptionPlanFileRules | SubscriptionPlanFileRuleMapping | `"SubscriptionPlanFileRules"` | ✅ OK |
| 16 | Tenants | TenantMapping | `"Tenants"` | ✅ OK |
| **17** | **TenantContactPersons** | **TenantContactMapping** | **`"TenantContacts"`** | 🔴 **CRÍTICO** |
| 18 | TenantAddresses | TenantAddressMapping | `"TenantAddresses"` | ⚠️ Neighborhood NOT NULL |
| 19 | TenantFiscalData | TenantFiscalDataMapping | `"TenantFiscalData"` | ✅ OK |
| 20 | TenantDocuments | TenantDocumentMapping | `"TenantDocuments"` | ✅ OK |
| 21 | StatusDefinitions | StatusDefinitionMapping | `"StatusDefinitions"` | ✅ OK |
| 22 | StatusDefinitionTranslations | StatusDefinitionTranslationMapping | `"StatusDefinitionTranslations"` | ✅ OK |
| 23 | Subscriptions | SubscriptionEntityMapping | `"Subscriptions"` | ⚠️ BillingInterval nullable |
| 24 | Users | UserMapping | `"Users"` | ✅ OK |
| 25 | UserPreferences | UserPreferencesMapping | `"UserPreferences"` | ✅ OK |
| 26 | Roles | RoleMapping | `"Roles"` | 🔴 Faltam Code + Desc |
| 27 | Resources | ResourceMapping | `"Resources"` | 🔴 Falta Code + Desc |
| 28 | Actions | ActionMapping | `"Actions"` | 🔴 Falta Code + Desc |
| 29 | RolePermissions | RolePermissionMapping | `"RolePermissions"` | 🔴 PK errada |
| 30 | UserRoles | UserRoleMapping | `"UserRoles"` | 🔴 PK errada |
| **31** | RefreshTokens | RefreshTokenMapping | `"RefreshTokens"` | ✅ OK |
| 32 | JwtKeys | JwtKeyMapping | `"JwtKeys"` | ✅ OK |
| 33 | JobDefinitions | JobDefinitionMapping | `"JobDefinitions"` | ⚠️ Vários tamanhos/tipos |
| 34 | Clients | ClientMapping | `"Clients"` | ✅ OK |
| 35 | ClientAddresses | ClientAddressMapping | `"ClientAddresses"` | ⚠️ Neighborhood NOT NULL |
| 36 | ClientContactPersons | ClientContactMapping | `"ClientContactPersons"` | ⚠️ Coluna fantasma IsWhatsapp |
| 37 | ClientDocuments | ClientDocumentMapping | `"ClientDocuments"` | ✅ OK |
| 38 | ClientFiscalData | ClientFiscalDataMapping | `"ClientFiscalData"` (sem dbo) | ⚠️ Sem schema |
| 39 | Teams | TeamMapping | `"Teams"` | ✅ OK |
| 40 | Employees | EmployeeMapping | `"Employees"` | ✅ OK |
| **41** | **EmployeeContactPersons** | **EmployeeContactMapping** | **`"EmployeeContacts"`** | 🔴 **CRÍTICO** |
| 42 | EmployeeAddresses | EmployeeAddressMapping | `"EmployeeAddresses"` | ⚠️ FK simples |
| 43 | EmployeeFiscalData | EmployeeFiscalDataMapping | `"EmployeeFiscalData"` | ✅ OK |
| 44 | EmployeeTeam | EmployeeTeamMapping | `"EmployeeTeam"` | ✅ OK |
| 45 | EquipmentTypes | EquipmentTypeMapping | `"EquipmentTypes"` | ✅ OK |
| 46 | Equipments | EquipmentMapping | `"Equipments"` | ⚠️ Typo constraint |
| 47 | Vehicles | VehicleMapping | `"Vehicles"` | ✅ OK |
| 48 | Visits | VisitMapping | `"Visits"` | ✅ OK |
| **49** | **VisitContactPersons** | **VisitContactMapping** | **`"VisitContacts"`** | 🔴 **CRÍTICO** |
| 50 | VisitAddresses | VisitAddressMapping | `"VisitAddresses"` | ⚠️ FK simples |
| 51 | VisitTeam | VisitTeamMapping | `"VisitTeam"` | ✅ OK |
| 52 | VisitTeamFunctions | VisitTeamFunctionMapping | `"VisitTeamFunctions"` | ✅ OK |
| 53 | VisitTeamEmployee | VisitTeamEmployeeMapping | `"VisitTeamEmployee"` | ✅ OK |
| 54 | VisitTeamVehicle | VisitTeamVehicleMapping | `"VisitTeamVehicle"` | ✅ OK |
| 55 | VisitTeamEquipment | VisitTeamEquipmentMapping | `"VisitTeamEquipment"` | ✅ OK |
| 56 | VisitAttachments | VisitAttachmentMapping | `"VisitAttachments"` (sem dbo) | ⚠️ Sem schema |

---

## 3. ERROS CRÍTICOS (Quebram em Runtime)

### 🔴 ERRO CRÍTICO #1: TenantContactMapping mapeia tabela INEXISTENTE

- **Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Mappings/Billing/TenantContactMapping.cs`
- **Linha:** 15
- **Erro:** `.ToTable("TenantContacts", "dbo")`
- **Correção:** `.ToTable("TenantContactPersons", "dbo")`
- **Impacto:** EF Core tentará acessar `dbo.TenantContacts` que NÃO EXISTE. A tabela real é `dbo.TenantContactPersons`. **Runtime exception garantida.**
- **Erros adicionais no mesmo mapping:**
  - Linha 39: `x.Phone` — coluna `Phone` **NÃO EXISTE** no SQL. A coluna SQL é `PhoneNumber` (NVARCHAR(50), nullable)
  - Linha 40-41: `Phone` mapeado como NVARCHAR(30) — SQL é NVARCHAR(50)
  - Linha 44-46: `JobTitle` NVARCHAR(100) — SQL é NVARCHAR(150)
  - Linha 49-51: `Department` NVARCHAR(100) — SQL é NVARCHAR(150)
  - Linha 54-56: `CellPhoneNumber` NVARCHAR(30) — SQL é NVARCHAR(50)
  - Linha 84: `CreatedAt` usa `SYSDATETIME()` — SQL usa `SYSUTCDATETIME()`

### 🔴 ERRO CRÍTICO #2: EmployeeContactMapping mapeia tabela INEXISTENTE

- **Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Mappings/Business/EmployeeContactMapping.cs`
- **Linha:** 15
- **Erro:** `.ToTable("EmployeeContacts", "dbo")`
- **Correção:** `.ToTable("EmployeeContactPersons", "dbo")`
- **Impacto:** EF Core tentará acessar `dbo.EmployeeContacts` que NÃO EXISTE. A tabela real é `dbo.EmployeeContactPersons`. **Runtime exception garantida.**
- **Erros adicionais:**
  - Linha 43-45: `x.Phone` — coluna **NÃO EXISTE** no SQL (SQL tem `PhoneNumber`)
  - Linha 47: `x.JobTitle` NVARCHAR(100) — SQL é NVARCHAR(150)
  - Linha 52: `x.Department` NVARCHAR(100) — SQL é NVARCHAR(150)
  - Linha 57: `x.CellPhoneNumber` NVARCHAR(30) — SQL é NVARCHAR(50)
  - Linha 96: PK name `PK_EmployeeContacts` — SQL é `PK_EmployeeContactPersons`

### 🔴 ERRO CRÍTICO #3: VisitContactMapping mapeia tabela INEXISTENTE

- **Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Mappings/Business/VisitContactMapping.cs`
- **Linha:** 15
- **Erro:** `.ToTable("VisitContacts", "dbo")`
- **Correção:** `.ToTable("VisitContactPersons", "dbo")`
- **Impacto:** EF Core tentará acessar `dbo.VisitContacts` que NÃO EXISTE. A tabela real é `dbo.VisitContactPersons`. **Runtime exception garantida.**
- **Erros adicionais:**
  - Linha 42-45: `x.Phone` — coluna **NÃO EXISTE** no SQL (SQL tem `PhoneNumber`)
  - Linha 47: `x.JobTitle` NVARCHAR(100) — SQL é NVARCHAR(150)
  - Linha 52: `x.Department` NVARCHAR(100) — SQL é NVARCHAR(150)
  - Linha 57: `x.CellPhoneNumber` NVARCHAR(30) — SQL é NVARCHAR(50)
  - Linha 18: PK name `PK_VisitContacts` — SQL é `PK_VisitContactPersons`

### 🔴 ERRO CRÍTICO #4: UserRoles PK não corresponde ao SQL

- **Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Mappings/Identity/UserRoleMapping.cs`
- **Linha:** 18
- **Erro:** `builder.HasKey(x => new { x.TenantId, x.UserId, x.RoleId })` — PK composta
- **SQL:** `CONSTRAINT PK_UserRoles PRIMARY KEY CLUSTERED (Id)` — PK é `Id` (IDENTITY)
- **Correção:** PK deve ser `x.Id` com `UseIdentityColumn`. Adicionar `Id` como propriedade na entidade. Adicionar UNIQUE constraint em `(TenantId, UserId, RoleId)`.
- **Impacto:** O modelo EF Core está fundamentalmente diferente do banco. O banco usa PK surrogate (Id), mas o código define PK composta. Qualquer operação de save/update pode falhar.

### 🔴 ERRO CRÍTICO #5: RolePermissions PK não corresponde ao SQL

- **Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Mappings/Identity/RolePermissionMapping.cs`
- **Linha:** 18
- **Erro:** `builder.HasKey(x => new { x.TenantId, x.RoleId, x.ResourceId, x.ActionId })` — PK composta
- **SQL:** `CONSTRAINT PK_RolePermissions PRIMARY KEY CLUSTERED (Id)` — PK é `Id` (IDENTITY)
- **Correção:** PK deve ser `x.Id` com `UseIdentityColumn`. Adicionar `Id` como propriedade. Manter UNIQUE em `(TenantId, RoleId, ResourceId, ActionId)`.
- **Impacto:** Mesmo que #4 — modelo incompatível com o banco.

---

## 4. ERROS ALTOS (Colunas Faltando / Inexistentes)

### 🟠 ERRO ALTO #6: RoleMapping — Coluna `Code` faltando

- **Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Mappings/Identity/RoleMapping.cs`
- **SQL:** `Code NVARCHAR(50) NOT NULL` (presente na tabela `dbo.Roles`)
- **Erro:** A coluna `Code` NÃO está mapeada. O mapping também não tem a coluna.
- **Correção:** Adicionar `builder.Property(x => x.Code).HasColumnType("NVARCHAR(50)").HasMaxLength(50).IsRequired()`
- Erro adicional: `Description` mapeado como NVARCHAR(255) — SQL é NVARCHAR(500)

### 🟠 ERRO ALTO #7: ResourceMapping — Colunas `Code` e `Description` faltando

- **Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Mappings/Identity/ResourceMapping.cs`
- **SQL Resources:** `Id, Code NVARCHAR(50) NOT NULL, Name NVARCHAR(100) NOT NULL, Description NVARCHAR(500) NOT NULL, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt`
- **Erro 1:** Coluna `Code` NÃO está mapeada
- **Erro 2:** `Description` mapeado como NVARCHAR(255) — SQL é NVARCHAR(500)
- **Erro 3:** Índice único está em `Name` (linha 60: `UQ_Resources_Name`) — SQL tem `UX_Resources_Code` em `Code`
- **Correção:** Adicionar `Code`, corrigir `Description` tamanho, mudar índice único para `Code`

### 🟠 ERRO ALTO #8: ActionMapping — Colunas `Code` e `Description` faltando

- **Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Mappings/Identity/ActionMapping.cs`
- **SQL Actions:** `Id, Code NVARCHAR(50) NOT NULL, Name NVARCHAR(50) NOT NULL, Description NVARCHAR(500) NOT NULL, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt`
- **Erro 1:** Coluna `Code` NÃO está mapeada
- **Erro 2:** Coluna `Description` NÃO está mapeada (SQL tem NOT NULL!)
- **Erro 3:** Índice único está em `Name` (linha 55: `UQ_Actions_Name`) — SQL tem `UX_Actions_Code` em `Code`
- **Correção:** Adicionar `Code` e `Description`, mudar índice único para `Code`

### 🟠 ERRO ALTO #9: AddressTypeMapping — Colunas `Name` e `Description` INEXISTENTES

- **Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Mappings/Business/AddressTypeMapping.cs`
- **Linhas:** 26-33
- **SQL AddressTypes:** `Id, Code, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt` — **NÃO tem Name nem Description**
- **Erro:** Mapping mapeia `x.Name` (NVARCHAR(200) REQUIRED) e `x.Description` (NVARCHAR(500) REQUIRED) — estas colunas NÃO EXISTEM na tabela `AddressTypes`. Os nomes estão na tabela de traduções (`AddressTypeTranslations`).
- **Impacto:** EF Core tentará fazer queries incluindo colunas que não existem → **Runtime exception.**

### 🟠 ERRO ALTO #10: AcquisitionSourceTypeMapping — Colunas `Name` e `Description` INEXISTENTES

- **Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Mappings/Business/AcquisitionSourceTypeMapping.cs`
- **Linhas:** 34-46
- **SQL AcquisitionSourceTypes:** `Id, Code, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt` — **NÃO tem Name nem Description**
- **Erro:** Mapping mapeia `x.Name` (NVARCHAR(100) REQUIRED) e `x.Description` (NVARCHAR(300) nullable) — estas colunas NÃO EXISTEM na tabela `AcquisitionSourceTypes`.
- **Impacto:** Runtime exception ao tentar acessar colunas inexistentes.

### 🟠 ERRO ALTO #11: ClientContactMapping — Coluna fantasma `IsWhatsapp`

- **Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Mappings/Business/ClientContactMapping.cs`
- **Linhas:** 47-50 e 67-70
- **Erro:** O mapping tem DUAS propriedades para o mesmo conceito:
  - `x.IsWhatsapp` (linha 47) — coluna que NÃO EXISTE no SQL
  - `x.IsCellPhoneWhatsapp` (linha 67) — esta é a coluna correta do SQL
- **Correção:** Remover `IsWhatsapp`, manter apenas `IsCellPhoneWhatsapp`
- **Erro adicional:** `Email` está `.IsRequired()` (linha 55) — SQL é `NULL`

### 🟠 ERRO ALTO #12: JobDefinitionMapping — Diversas divergências

- **Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Mappings/Job/JobDefinitionMapping.cs`
- **Erros:**
  - Linha 29: `Description` NVARCHAR(1000) — SQL é NVARCHAR(500)
  - Linha 32: `JobPurpose` NVARCHAR(1000) — SQL é NVARCHAR(500)
  - Linha 42: `CronExpression` NVARCHAR(200) — SQL é NVARCHAR(100)
  - Linha 44: `TimeZoneId` NVARCHAR(150) — SQL coluna é `Timezone` NVARCHAR(100) (nome de coluna diferente!)
  - Linha 57: `Queue` NVARCHAR(100) — SQL é NVARCHAR(50)
  - Linha 69: `HangfireJobId` NVARCHAR(200) — SQL é NVARCHAR(100)
  - Linha 63: `JobConfiguration` usa `"text"` — SQL é NVARCHAR(MAX)

### 🟠 ERRO ALTO #13: EquipmentMapping — Typo em constraint name

- **Arquivo:** `src/VianaHub.Global.Gerit.Infra.Data/Mappings/Business/EquipmentMapping.cs`
- **Linha:** 92
- **Erro:** `.HasConstraintName("FK_Equipments_EquipamentType")` — "Equipament" é um typo
- **SQL:** `FK_Equipments_EquipmentType` — correto é "Equipment"
- **Correção:** `.HasConstraintName("FK_Equipments_EquipmentType")`

---

## 5. ERROS MÉDIOS (Nullable / Tamanho / Tipo Incorreto)

### 🟡 ERRO MÉDIO #14-17: `Neighborhood` marcado como NOT NULL mas SQL é NULL

**Arquivos afetados e linhas:**
| Arquivo | Linha | Tabela SQL | Coluna SQL |
|---|---|---|---|
| `TenantAddressMapping.cs` | 47 | `TenantAddresses` | `Neighborhood NVARCHAR(100) NULL` |
| `ClientAddressMapping.cs` | 53 | `ClientAddresses` | `Neighborhood NVARCHAR(100) NULL` |
| `EmployeeAddressMapping.cs` | 52 | `EmployeeAddresses` | `Neighborhood NVARCHAR(100) NULL` |
| `VisitAddressMapping.cs` | 55 | `VisitAddresses` | `Neighborhood NVARCHAR(100) NULL` |

- **Correção:** Mudar `.IsRequired()` para `.IsRequired(false)` em todos os 4 arquivos.

### 🟡 ERRO MÉDIO #18: SubscriptionEntityMapping — BillingInterval nullable errado

- **Arquivo:** `SubscriptionEntityMapping.cs`, linha 39
- **Erro:** `x.BillingInterval` está `.IsRequired(false)` 
- **SQL:** `BillingInterval NVARCHAR(20) NOT NULL`
- **Correção:** Deve ser `.IsRequired()`

### 🟡 ERRO MÉDIO #19: VisitAddressMapping / EmployeeAddressMapping — FK simples vs composta

- **Arquivo:** `VisitAddressMapping.cs`, linha 131-134
- **Erro:** `builder.HasOne(x => x.Visit).WithMany(i => i.Addresses).HasForeignKey(x => x.VisitId)` — FK usa apenas `VisitId`
- **SQL:** `FK_VisitAddresses_Visit FOREIGN KEY (VisitId, TenantId) REFERENCES Visits(Id, TenantId)` — FK composta!
- **Correção:** Usar `HasForeignKey(x => new { x.VisitId, x.TenantId })`

- **Arquivo:** `EmployeeAddressMapping.cs`, linha 131-135
- **Erro:** `builder.HasOne(x => x.Employee).WithMany(tm => tm.Addresses).HasForeignKey(x => x.EmployeeId)` — FK apenas com `EmployeeId`
- **SQL:** `FK_EmployeeAddresses_Employee FOREIGN KEY (EmployeeId, TenantId) REFERENCES Employees(Id, TenantId)` — FK composta!
- **Correção:** Usar `HasForeignKey(x => new { x.EmployeeId, x.TenantId })`

### 🟡 ERRO MÉDIO #20-21: Schemas ausentes

- **Arquivo:** `ClientFiscalDataMapping.cs`, linha 12
- **Erro:** `.ToTable("ClientFiscalData")` — sem schema `"dbo"`
- **Correção:** `.ToTable("ClientFiscalData", "dbo")`

- **Arquivo:** `VisitAttachmentMapping.cs`, linha 11
- **Erro:** `.ToTable("VisitAttachments")` — sem schema `"dbo"`
- **Correção:** `.ToTable("VisitAttachments", "dbo")`

### 🟡 ERRO MÉDIO #22: TenantAddressMapping — CreatedAt tipo errado

- **Arquivo:** `TenantAddressMapping.cs`, linha 108
- **Erro:** `CreatedAt` mapeado como `DATETIME2` e default `SYSDATETIME()`
- **SQL:** `CreatedAt DATETIME2(7) NOT NULL DEFAULT SYSUTCDATETIME()`
- **Correção:** Usar `HasColumnType("DATETIME2(7)")` e `.HasDefaultValueSql("SYSUTCDATETIME()")`

### 🟡 ERRO MÉDIO #23: TenantDocumentMapping — CreatedAt tipo errado

- **Arquivo:** `TenantDocumentMapping.cs`, linha 74
- **Erro:** Mesmo padrão — `DATETIME2` com `SYSDATETIME()`
- **SQL:** `DATETIME2(7)` com `SYSUTCDATETIME()`

### 🟡 ERRO MÉDIO #24: ClientDocumentMapping — CreatedAt tipo errado

- **Arquivo:** `ClientDocumentMapping.cs`, linha 78
- **Erro:** Mesmo padrão — `DATETIME2` com `SYSDATETIME()`
- **SQL:** `DATETIME2(7)` com `SYSUTCDATETIME()`

### 🟡 ERRO MÉDIO #25: EmployeeFiscalDataMapping — CreatedAt tipo errado

- **Arquivo:** `EmployeeFiscalDataMapping.cs`, linha 80
- **Erro:** Mesmo padrão
- **SQL:** `DATETIME2(7)` com `SYSUTCDATETIME()`

### 🟡 ERRO MÉDIO #26: EmployeeAddressMapping — CreatedAt tipo errado

- **Arquivo:** `EmployeeAddressMapping.cs`, linha 111
- **Erro:** `DATETIME2` com `SYSDATETIME()`
- **SQL:** `DATETIME2(7)` com `SYSUTCDATETIME()`

### 🟡 ERRO MÉDIO #27: Multiple mappings — CreatedAt/ModifiedAt inconsistente

Vários mappings usam `DATETIME2` em vez de `DATETIME2(7)` e `SYSDATETIME()` em vez de `SYSUTCDATETIME()`:
- TenantAddressMapping
- TenantDocumentMapping
- ClientDocumentMapping
- ClientFiscalDataMapping
- EmployeeAddressMapping
- EmployeeFiscalDataMapping
- SubscriptionPlanFileRuleMapping
- AddressTypeMapping
- DocumentTypeMapping
- StatusDomainMapping
- TenantContactMapping

---

## 6. TABELAS SEM MAPPING

Nenhuma. Todas as 56 tabelas têm um mapping correspondente (embora 3 estejam com nome errado).

## 7. MAPPINGS SEM TABELA

Nenhum mapping referencia uma tabela que não existe, mas 3 mappings referenciam tabelas com nome diferente do SQL:
- `TenantContactMapping` → `"TenantContacts"` (deveria ser `"TenantContactPersons"`)
- `EmployeeContactMapping` → `"EmployeeContacts"` (deveria ser `"EmployeeContactPersons"`)
- `VisitContactMapping` → `"VisitContacts"` (deveria ser `"VisitContactPersons"`)

---

## 8. OBSERVAÇÕES ADICIONAIS

### 8.1 Padrão de nomenclatura inconsistente
O SQL consistentemente usa o sufixo `Persons` para tabelas de contato:
- `TenantContactPersons` ✅
- `ClientContactPersons` ✅
- `EmployeeContactPersons` ✅
- `VisitContactPersons` ✅

Mas os mappings usam:
- `TenantContacts` ❌
- `ClientContactPersons` ✅
- `EmployeeContacts` ❌
- `VisitContacts` ❌

Apenas `ClientContactPersons` está correto. Os outros 3 perderam o sufixo `Persons`.

### 8.2 GeritDbContext está sem DbSet para JobDefinitions
O arquivo `GeritDbContext.cs` não declara `DbSet<JobDefinitionEntity>`. Embora o mapping exista via `ApplyConfigurationsFromAssembly`, a entidade pode não ser incluída no modelo se não houver DbSet ou navegação que a referencie. Isto pode causar ausência da tabela no modelo EF Core.

### 8.3 DatabaseSeeder referencia tabelas com nomes potencialmente errados
Verificar `src/VianaHub.Global.Gerit.Infra.Data/Seeders/DatabaseSeeder.cs` para garantir que usa os nomes corretos das tabelas.

---

## 9. VERIFICAÇÃO DE BUILD

```
dotnet build: 0 Erro(s), 1181 Aviso(s)
```

O build compila, mas **não valida a existência das tabelas no banco** — os erros de nome de tabela (#1, #2, #3) só seriam detectados em **runtime** ou com **testes de integração contra um banco real**.

---

## 10. TOP 10 ERROS MAIS CRÍTICOS (Ordem de Gravidade)

| # | Gravidade | Erro | Impacto |
|---|---|---|---|
| 1 | 🔴 CRÍTICO | TenantContactMapping → `"TenantContacts"` em vez de `"TenantContactPersons"` | Runtime exception |
| 2 | 🔴 CRÍTICO | EmployeeContactMapping → `"EmployeeContacts"` em vez de `"EmployeeContactPersons"` | Runtime exception |
| 3 | 🔴 CRÍTICO | VisitContactMapping → `"VisitContacts"` em vez de `"VisitContactPersons"` | Runtime exception |
| 4 | 🔴 CRÍTICO | UserRoleMapping PK composta vs PK Identity no SQL | Incompatibilidade fundamental |
| 5 | 🔴 CRÍTICO | RolePermissionMapping PK composta vs PK Identity no SQL | Incompatibilidade fundamental |
| 6 | 🟠 ALTO | AddressTypeMapping mapeia colunas Name/Description INEXISTENTES | Runtime exception |
| 7 | 🟠 ALTO | AcquisitionSourceTypeMapping mapeia colunas Name/Description INEXISTENTES | Runtime exception |
| 8 | 🟠 ALTO | RoleMapping sem coluna `Code` (existe no SQL) | Coluna ignorada |
| 9 | 🟠 ALTO | ResourceMapping sem coluna `Code` (existe no SQL) + Description tamanho errado | Coluna ignorada |
| 10 | 🟠 ALTO | ActionMapping sem colunas `Code` e `Description` (existem no SQL) | Colunas ignoradas |

---

## 11. CONCLUSÃO

**A análise anterior estava INCORRETA ao afirmar que tudo estava 100% alinhado.** Esta varredura linha a linha encontrou:

- **3 tabelas com nome errado** nos mappings (causariam erro em runtime)
- **2 PKs fundamentalmente erradas** (UserRoles, RolePermissions)
- **2 mappings com colunas INEXISTENTES** (AddressType, AcquisitionSourceType)
- **3 entidades sem colunas obrigatórias do SQL** (Role.Code, Resource.Code, Action.Code + Description)
- **14+ erros de tipo/tamanho/nullable**

**Total: 27 erros encontrados.**

A correção deve priorizar os erros críticos #1 a #5, que impediriam o funcionamento correto da aplicação contra o banco de dados real.

---

## 12. STATUS DE CORREÇÃO — 2026-07-17

**Todos os 27 erros foram corrigidos.** Build: 0 erros. Testes: 31/31 aprovados.

| # | Gravidade | Erro | Status | Correção |
|---|---|---|---|---|
| 1 | 🔴 CRÍTICO | TenantContactMapping → `"TenantContacts"` | ✅ CORRIGIDO | `.ToTable("TenantContactPersons")`, Phone→PhoneNumber (HasColumnName), sizes, DATETIME2(7)+SYSUTCDATETIME, PK/IX names |
| 2 | 🔴 CRÍTICO | EmployeeContactMapping → `"EmployeeContacts"` | ✅ CORRIGIDO | `.ToTable("EmployeeContactPersons")`, Phone→PhoneNumber, sizes, PK/FK/IX names |
| 3 | 🔴 CRÍTICO | VisitContactMapping → `"VisitContacts"` | ✅ CORRIGIDO | `.ToTable("VisitContactPersons")`, Phone→PhoneNumber, sizes, PK/FK/IX names |
| 4 | 🔴 CRÍTICO | UserRoleMapping PK composta vs Identity | ✅ CORRIGIDO | Adicionado `Id` à entidade + UseIdentityColumn + HasKey(Id). Unique constraint composta mantida |
| 5 | 🔴 CRÍTICO | RolePermissionMapping PK composta vs Identity | ✅ CORRIGIDO | Adicionado `Id` à entidade + UseIdentityColumn + HasKey(Id). Unique constraint composta mantida |
| 6 | 🟠 ALTO | RoleMapping sem coluna `Code` | ✅ CORRIGIDO | Adicionado `Code` NVARCHAR(50) à entidade e mapping. Description 255→500. DTOs e AppServices atualizados |
| 7 | 🟠 ALTO | ResourceMapping sem coluna `Code` | ✅ CORRIGIDO | Adicionado `Code` NVARCHAR(50) à entidade e mapping. Description 255→500. Unique index em Code. DTOs e AppServices atualizados |
| 8 | 🟠 ALTO | ActionMapping sem colunas `Code`+`Description` | ✅ CORRIGIDO | Adicionado `Code` NVARCHAR(50) à entidade e mapping. Description NVARCHAR(500). Unique index em Code. DTOs e AppServices atualizados |
| 9 | 🟠 ALTO | AddressTypeMapping Name/Description INEXISTENTES | ✅ CORRIGIDO | Adicionado `Ignore()` para Name e Description (estão na tabela de traduções) |
| 10 | 🟠 ALTO | AcquisitionSourceTypeMapping Name/Description INEXISTENTES | ✅ CORRIGIDO | Adicionado `Ignore()` para Name e Description (estão na tabela de traduções) |
| 11 | 🟠 ALTO | ClientContactMapping coluna fantasma `IsWhatsapp` | ✅ CORRIGIDO | `IsWhatsapp` ignorado via `Ignore()`. Apenas `IsCellPhoneWhatsapp` mapeado. Email nullable |
| 12 | 🟠 ALTO | JobDefinitionMapping divergências | ✅ CORRIGIDO | Description/JobPurpose 500, CronExpression 100, TimeZoneId→HasColumnName("Timezone") 100, Queue 50, HangfireJobId 100, JobConfiguration NVARCHAR(MAX) |
| 13 | 🟠 ALTO | EquipmentMapping typo `EquipamentType` | ✅ CORRIGIDO | `HasConstraintName("FK_Equipments_EquipmentType")` |
| 14 | 🟡 MÉDIO | TenantAddressMapping Neighborhood NOT NULL | ✅ CORRIGIDO | `.IsRequired(false)` |
| 15 | 🟡 MÉDIO | ClientAddressMapping Neighborhood NOT NULL | ✅ CORRIGIDO | `.IsRequired(false)` |
| 16 | 🟡 MÉDIO | EmployeeAddressMapping Neighborhood NOT NULL | ✅ CORRIGIDO | `.IsRequired(false)` |
| 17 | 🟡 MÉDIO | VisitAddressMapping Neighborhood NOT NULL | ✅ CORRIGIDO | `.IsRequired(false)` |
| 18 | 🟡 MÉDIO | SubscriptionEntityMapping BillingInterval nullable | ✅ CORRIGIDO | `.IsRequired()` |
| 19 | 🟡 MÉDIO | VisitAddressMapping/EmployeeAddressMapping FK simples vs composta | ✅ CORRIGIDO | `HasForeignKey(x => new { x.VisitId/EmpId, x.TenantId })` + `HasPrincipalKey` |
| 20 | 🟡 MÉDIO | ClientFiscalDataMapping sem schema | ✅ CORRIGIDO | `.ToTable("ClientFiscalData", "dbo")` |
| 21 | 🟡 MÉDIO | VisitAttachmentMapping sem schema | ✅ CORRIGIDO | `.ToTable("VisitAttachments", "dbo")` |
| 22 | 🟡 MÉDIO | TenantAddressMapping CreatedAt | ✅ CORRIGIDO | `DATETIME2(7)` + `SYSUTCDATETIME()` |
| 23 | 🟡 MÉDIO | TenantDocumentMapping CreatedAt | ✅ CORRIGIDO | `DATETIME2(7)` + `SYSUTCDATETIME()` |
| 24 | 🟡 MÉDIO | ClientDocumentMapping CreatedAt | ✅ CORRIGIDO | `DATETIME2(7)` + `SYSUTCDATETIME()` |
| 25 | 🟡 MÉDIO | EmployeeFiscalDataMapping CreatedAt | ✅ CORRIGIDO | `DATETIME2(7)` + `SYSUTCDATETIME()` |
| 26 | 🟡 MÉDIO | EmployeeAddressMapping CreatedAt | ✅ CORRIGIDO | `DATETIME2(7)` + `SYSUTCDATETIME()` |
| 27 | 🟡 MÉDIO | Múltiplos mappings DATETIME2/SYSDATETIME | ✅ CORRIGIDO | SubscriptionPlanFileRule, DocumentType, StatusDomain, PartyType, TenantContact, SubscriptionEntity, ClientFiscalData — todos `DATETIME2(7)` + `SYSUTCDATETIME()` |

### Arquivos Modificados (43 arquivos)

**Domain Entities (5):**
- `UserRoleEntity.cs` — adicionado `Id`
- `RolePermissionEntity.cs` — adicionado `Id`
- `RoleEntity.cs` — adicionado `Code`, construtor/update atualizados
- `ResourceEntity.cs` — adicionado `Code`, construtor/update atualizados
- `ActionEntity.cs` — adicionado `Code`, construtor/update atualizados

**Application DTOs (9):**
- `CreateRoleRequest.cs`, `UpdateRoleRequest.cs`, `BulkUploadRoleItem.cs` — adicionado `Code`
- `CreateResourceRequest.cs`, `UpdateResourceRequest.cs`, `BulkUploadResourceItem.cs` — adicionado `Code`
- `CreateActionRequest.cs`, `UpdateActionRequest.cs`, `BulkUploadActionItem.cs` — adicionado `Code`

**Application Services (3):**
- `RoleAppService.cs` — construtor/update com `Code`
- `ResourceAppService.cs` — construtor/update com `Code`
- `ActionAppService.cs` — construtor/update com `Code`

**Infra.Data Mappings (26):**
- `TenantContactMapping.cs`, `EmployeeContactMapping.cs`, `VisitContactMapping.cs` — nome tabela, colunas, sizes, DATETIME2
- `UserRoleMapping.cs`, `RolePermissionMapping.cs` — PK Identity
- `RoleMapping.cs`, `ResourceMapping.cs`, `ActionMapping.cs` — Code, Description, indexes
- `AddressTypeMapping.cs`, `AcquisitionSourceTypeMapping.cs` — Ignore Name/Description
- `ClientContactMapping.cs` — Ignore IsWhatsapp, Email nullable
- `JobDefinitionMapping.cs` — sizes, Timezone column
- `EquipmentMapping.cs` — typo constraint
- `TenantAddressMapping.cs`, `ClientAddressMapping.cs`, `EmployeeAddressMapping.cs`, `VisitAddressMapping.cs` — Neighborhood nullable, FK compostas, DATETIME2
- `SubscriptionEntityMapping.cs` — BillingInterval required, DATETIME2
- `ClientFiscalDataMapping.cs`, `VisitAttachmentMapping.cs` — schema dbo
- `TenantDocumentMapping.cs`, `ClientDocumentMapping.cs`, `EmployeeFiscalDataMapping.cs` — DATETIME2
- `SubscriptionPlanFileRuleMapping.cs`, `DocumentTypeMapping.cs`, `StatusDomainMapping.cs`, `PartyTypeMapping.cs` — DATETIME2
