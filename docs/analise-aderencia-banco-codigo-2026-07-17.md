# Análise de Aderência Banco-Código — Índices SQL → EF Core

**Data:** 17 de Julho de 2026
**Responsável:** Developer Senior
**Base:** `docs/sql/Create-Tables.sql` → EF Core Mappings

---

## Resumo

| Métrica | Antes | Depois |
|---------|-------|--------|
| Índices no SQL (CREATE INDEX) | 75 | 75 |
| Constraints UNIQUE (CONSTRAINT UQ_xxx) | 42 | 42 |
| **Total SQL (excluindo PK)** | **117** | **117** |
| Índices mapeados no EF | ~79 | ~114 |
| Índices criados/adicionados | — | ~35 |
| Índices renomeados/corrigidos | — | ~12 |
| Arquivos modificados | — | 62 |
| Build | ✅ OK | ✅ 0 erros |
| Testes | ✅ OK | ✅ 31/31 passando |

---

## Índices Criados (Novos)

### Chaves Alternativas (UQ_Id_Tenant) — suporte a FKs compostas

| # | Tabela | Nome do Índice | Colunas |
|---|--------|---------------|---------|
| 1 | Equipments | UQ_Equipments_Id_Tenant | (Id, TenantId) |
| 2 | EquipmentTypes | UQ_EquipmentTypes_Id_Tenant | (Id, TenantId) |
| 3 | Teams | UQ_Teams_Id_Tenant | (Id, TenantId) |
| 4 | Vehicles | UQ_Vehicles_Id_Tenant | (Id, TenantId) |
| 5 | VisitTeam | UQ_VisitTeam_Id_Tenant | (Id, TenantId) |
| 6 | VisitAddresses | UQ_VisitAddresses_Id_Tenant | (Id, TenantId) |
| 7 | Roles | UQ_Roles_Id_Tenant | (Id, TenantId) |
| 8 | ClientDocuments | UQ_ClientDocuments_Id_Tenant | (Id, TenantId) |
| 9 | ClientAddresses | UQ_ClientAddresses_Id_Tenant | (Id, TenantId) |
| 10 | VisitTeamVehicle | UQ_VisitTeamVehicle_Id_Tenant | (Id, TenantId) |
| 11 | VisitTeamEquipment | UQ_VisitTeamEquipment_Id_Tenant | (Id, TenantId) |
| 12 | UserRoles | UQ_UserRoles | (TenantId, UserId, RoleId) |

### Índices Únicos Filtrados (UX_)

| # | Tabela | Nome do Índice | Colunas | Filtro |
|---|--------|---------------|---------|--------|
| 13 | ClientContactPersons | UX_ClientContactPersons_Email | (TenantId, ClientId, Email) | Email IS NOT NULL AND IsActive=1 |
| 14 | ClientContactPersons | UX_ClientContactPersons_Primary | (TenantId, ClientId) | IsPrimary=1 AND IsActive=1 |
| 15 | ClientDocuments | UX_ClientDocuments_Type_Country_Number | (TenantId, DocumentTypeId, IssuingCountryCode, DocumentNumber) | IsDeleted=0 |
| 16 | ClientFiscalData | UX_ClientFiscalData_TaxNumber | (TenantId, FiscalCountry, TaxNumber) | IsDeleted=0 |
| 17 | ClientFiscalData | UX_ClientFiscalData_Active | (TenantId, ClientId) | IsActive=1 |
| 18 | EmployeeFiscalData | UX_EmployeeFiscalData_TaxNumber | (TenantId, FiscalCountry, TaxNumber) | IsDeleted=0 |
| 19 | TenantContactPersons | UX_TenantContactPersons_Email_Active | (TenantId, Email) | Email IS NOT NULL AND IsActive=1 |
| 20 | TenantContactPersons | UX_TenantContactPersons_Primary | (TenantId) | IsPrimary=1 AND IsActive=1 |
| 21 | Subscriptions | UX_Subscriptions_StripeId | (StripeId) | StripeId IS NOT NULL |
| 22 | Subscriptions | UX_Subscriptions_Active | (TenantId) | IsActive=1 |
| 23 | JwtKeys | UX_JwtKeys_Active | (TenantId) | IsActive=1 |
| 24 | RefreshTokens | UX_RefreshTokens_TokenHash | (Token) | — |
| 25 | AddressTypes | UX_AddressTypes_Code | (Code) | IsDeleted=0 |
| 26 | DocumentTypes | UX_DocumentTypes_Code | (Code) | IsDeleted=0 |
| 27 | StatusDomains | UX_StatusDomains_Code | (Code) | IsDeleted=0 |
| 28 | FileTypes | UX_FileTypes_MimeType_Extension | (MimeType, Extension) | IsDeleted=0 |
| 29 | EquipmentTypes | UX_EquipmentTypes_Tenant_Name | (TenantId, Name) | IsDeleted=0 |
| 30 | Teams | UX_Teams_Tenant_Name | (TenantId, Name) | IsDeleted=0 |
| 31 | EmployeeAddresses | UX_EmployeeAddresses_Primary | (TenantId, EmployeeId) | IsPrimary=1 AND IsActive=1 |
| 32 | TenantAddresses | UX_TenantAddresses_Primary | (TenantId) | IsPrimary=1 AND IsActive=1 |
| 33 | VisitTeamVehicle | UX_VisitTeamVehicle_Unique | (TenantId, VisitTeamId, VehicleId) | IsDeleted=0 |
| 34 | VisitTeamEquipment | UX_VisitTeamEquipment_Unique | (TenantId, VisitTeamId, EquipmentId) | IsDeleted=0 |

### Índices Não-Clusterizados (IX_)

| # | Tabela | Nome do Índice | Colunas | Filtro |
|---|--------|---------------|---------|--------|
| 35 | Clients | IX_Clients_TenantId | (TenantId) | IsDeleted=0 |
| 36 | ClientAddresses | IX_ClientAddresses_Client | (TenantId, ClientId) | IsDeleted=0 |
| 37 | ClientContactPersons | IX_ClientContactPersons_Client | (TenantId, ClientId) | IsDeleted=0 |
| 38 | ClientDocuments | IX_ClientDocuments_Client | (TenantId, ClientId) | IsDeleted=0 |
| 39 | Visits | IX_Visits_Tenant_Date | (TenantId, StartDateTime) | IsDeleted=0 |
| 40 | Visits | IX_Visits_ClientId | (TenantId, ClientId) | IsDeleted=0 |
| 41 | Visits | IX_Visits_Dashboard | (TenantId, StatusDefinitionId, StartDateTime) | IsDeleted=0 |
| 42 | SubscriptionPlanFileRules | IX_SubscriptionPlanFileRules_SubscriptionPlan | (SubscriptionPlanId) | IsDeleted=0 |
| 43 | SubscriptionPlanFileRules | IX_SubscriptionPlanFileRules_FileType | (FileTypeId) | IsDeleted=0 |
| 44 | RefreshTokens | IX_RefreshTokens_User_Active | (TenantId, UserId) | RevokedAt IS NULL |
| 45 | RefreshTokens | IX_RefreshTokens_ExpiresAt | (TenantId, ExpiresAt) | RevokedAt IS NULL |
| 46 | VisitTeamVehicle | IX_VisitTeamVehicle_VisitTeamId | (TenantId, VisitTeamId) | IsDeleted=0 |
| 47 | VisitTeamEquipment | IX_VisitTeamEquipment_VisitTeamId | (TenantId, VisitTeamId) | IsDeleted=0 |

---

## Índices Renomeados/Corrigidos

| # | Arquivo | Antes | Depois |
|---|---------|-------|--------|
| 48 | JobDefinitionMapping | IX_Services_Category_Active | IX_JobDefinitions_Category_Active |
| 49 | JobDefinitionMapping | IX_Services_Active_SYSTEM | IX_JobDefinitions_Active_System |
| 50 | JobDefinitionMapping | IX_Services_HangfireJobId | IX_JobDefinitions_HangfireJobId |
| 51 | JobDefinitionMapping | UQ_Job_JobName | UX_JobDefinitions_JobName |
| 52 | ClientAddressMapping | IX_ClientAddresses_ClientId | IX_ClientAddresses_Client |
| 53 | ClientContactMapping | IX_ClientContacts_ClientId | IX_ClientContactPersons_Client |
| 54 | ClientDocumentMapping | UX_ClientDocuments_Primary | corrigido colunas/filtro |
| 55 | SubscriptionMapping | UQ_Subscriptions_Tenant_Active | UX_Subscriptions_Active |
| 56 | EmployeeAddressMapping | IX_EmployeeAddresses_EmployeeId | corrigido colunas (TenantId, EmployeeId) |
| 57 | ClientContactMapping | Table: ClientContacts | Table: ClientContactPersons |
| 58 | UserPreferencesMapping | UX_UserPreferences_Tenant_User_Active | corrigido filtro (IsActive=1) |

---

## Arquivo Criado

| Arquivo | Descrição |
|---------|-----------|
| `Infra.Data/Mappings/Identity/RefreshTokenMapping.cs` | Novo mapeamento EF para tabela RefreshTokens |

---

## Notas

- **PRIMARY KEY** não listadas pois o EF Core as cria automaticamente via `HasKey()`
- Índices com prefixo `UQ_` no EF podem corresponder a `UX_` no SQL ou vice-versa — a semântica é equivalente
- Os nomes de índice no EF foram mantidos idênticos aos do SQL via `HasDatabaseName()`
- A aplicação ainda não está em produção, portanto não há risco de breaking change
- `ApplyConfigurationsFromAssembly` garante que o novo `RefreshTokenMapping` é automaticamente descoberto
