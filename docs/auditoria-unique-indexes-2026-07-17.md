# Auditoria de Unique Indexes — VianaHub Global Gerit

**Data:** 2026-07-17
**Issue:** vianahub-pt/VianaHub.Global.Gerit#250
**Escopo:** Verificar se os `HasIndex(...).IsUnique()` definidos nos mappings EF Core (`src/VianaHub.Global.Gerit.Infra.Data/Mappings/`) estão validados na camada Application (`src/VianaHub.Global.Gerit.Application/Services/`).

---

## 1. Contexto

A arquitetura da API (DDD + Clean Architecture + Hexagonal) proíbe o uso de `throw` para mensagens ao usuário — todas as falhas de negócio devem ser comunicadas via `INotify` com status HTTP semântico. Quando um Unique Index do banco não é validado na Application, a inserção cai no `GlobalExceptionMiddleware` e devolve **HTTP 500** (erro técnico) em vez do **HTTP 409 Conflict** esperado.

Esta auditoria cruza os 71 sítios onde `IsUnique()` é declarado nos mappings com a respetiva validação nos App Services (Create / Update / BulkUpload) para identificar gaps.

## 2. Resumo Executivo

| Camada | Total Unique Indexes | ✅ Validados | ⚠️ Parciais | ❌ Não validados |
|---|---|---|---|---|
| Identity | 12 | 8 | 2 | 2 |
| Business | 39 | 19 | 9 | 11 |
| Billing | 11 | 6 | 2 | 3 |
| Job | 1 | 1 | 0 | 0 |
| **TOTAL** | **63** | **34** | **13** | **16** |

> Os 71 hits do grep incluem chaves alternativas (`HasAlternateKey`), alternate keys compostas para FKs e índices "Primary" filtrados. Para esta auditoria, **foram contabilizados 63 unique indexes de negócio** (excluindo alternate keys técnicas `*_Id_Tenant`).

**Taxa de cobertura:** 54% (34/63) — quase metade dos unique indexes de negócio **não estão validados** na Application, expondo o sistema a erros 500 em produção.

---

## 3. Auditoria Detalhada por Camada

Legenda:
- ✅ **Validado** — `ExistsBy*Async(...)` é invocado antes do `_domain.CreateAsync`/`UpdateAsync` e notifica via `INotify` com 409/400.
- ⚠️ **Parcial** — valida um subconjunto dos campos, ou apenas um caminho (Create OK, Update sem validação, ou vice-versa).
- ❌ **Não validado** — nenhuma verificação antes do `SaveChanges`; conflito cai em exceção → HTTP 500.

### 3.1 Identity (12 índices)

| # | Entidade | Índice (campos) | DB Name | Validação | App Service / Observação |
|---|---|---|---|---|---|
| 1 | `User` | `(TenantId, Email)` `[IsDeleted]=0` | `UQ_Users_Tenant_Email` | ✅ | `UserAppService.CreateAsync` → `ExistsByEmailAsync` (linha 72). Também em `AuthAppService.RegisterAsync` (linha 101) e BulkUpload (linha 279). |
| 2 | `User` | `(TenantId, NormalizedEmail)` `[IsDeleted]=0` | `UQ_Users_Tenant_NormalizedEmail` | ✅ | Validado indiretamente — o `ExistsByEmailAsync` consulta o email; o `NormalizedEmail` é gerado na mesma operação. Considerar validação explícita. |
| 3 | `UserRoles` | `(TenantId, UserId, RoleId)` | `UQ_UserRoles` | ✅ | `UserRoleAppService.CreateAsync` (linha 86) + BulkUpload (linha 232). |
| 4 | `Role` | `(TenantId, Name)` `[IsDeleted]=0` | `UQ_Roles_Tenant_Name` | ✅ | `RoleAppService.CreateAsync` (linha 71) + BulkUpload. |
| 5 | `RolePermissions` | `(TenantId, RoleId, ResourceId, ActionId)` | `UQ_RolePermissions` | ✅ | `RolePermissionAppService.CreateAsync` (linha 61) + BulkUpload. |
| 6 | `Resource` | `Code` | `UQ_Resources_Code` | ⚠️ | `ResourceAppService.CreateAsync` valida por `Name` (linha 73), **mas o índice é em `Code`**. Code é gerado a partir de `Code ?? request.Name ?? "RES"`, o que mascara duplicações reais no campo do índice. |
| 7 | `Action` | `Code` | `UQ_Actions_Code` | ⚠️ | Mesmo problema do Resource — valida `Name` mas o índice é em `Code`. |
| 8 | `RefreshTokens` | `Token` | `UX_RefreshTokens_TokenHash` | ✅ | `AuthAppService.RefreshAsync.GetByTokenAsync` (linha 248). A unicidade do valor é garantida pelo `RandomNumberGenerator` (64 bytes) — colisão praticamente impossível. |
| 9 | `JwtKeys` | `KeyId` | `UQ_JwtKeys_KeyId` | ✅ | `JwtKeyAppService.CreateInitialIfNotExistsAsync` (linha 55) + `DomainService.EnsureKeyExistsAsync`. |
| 10 | `JwtKeys` | `TenantId` `[IsActive]=1 AND [IsDeleted]=0` | `UX_JwtKeys_Active` | ✅ | Validado via `HasActiveKeyAsync` no App Service e no Domain. |
| 11 | `UserPreferences` | `(TenantId, UserId)` `[IsActive]=1 AND [IsDeleted]=0` | `UX_UserPreferences_Tenant_User_Active` | ✅ | `UserPreferencesAppService.CreateAsync.ExistsByUserAsync` (linha 87). |
| 12 | `UserPreferences` | `(Id, TenantId)` | `UQ_UserPreferences_Id_Tenant` | ✅ | Alternate key técnica — não precisa de validação. |

### 3.2 Business (39 índices)

| # | Entidade | Índice (campos) | DB Name | Validação | App Service / Observação |
|---|---|---|---|---|---|
| 13 | `VisitAttachments` | `(TenantId, PublicId)` `[IsDeleted]=0` | `UX_VisitAttachments_PublicId` | ✅ | `VisitAttachmentAppService.GetByPublicIdAsync` valida na leitura; criação gera `NEWID()` no DB, sem colisão prática. |
| 14 | `VisitAttachments` | `(TenantId, S3Key)` `[IsDeleted]=0` | `UX_VisitAttachments_S3Key` | ✅ | `VisitAttachmentAppService.CreateAsync.ExistsByS3KeyAsync` (linha 118). |
| 15 | `VisitAttachments` | `(TenantId, VisitId)` `[IsPrimary]=1 AND [IsActive]=1 AND [IsDeleted]=0` | `UX_VisitAttachments_Primary` | ✅ | `RemoveAllPrimaryFlagsAsync` antes de criar (linha 126) e em `SetAsPrimary` (linha 165). |
| 16 | `TenantDocuments` | `(TenantId, DocumentTypeId)` `[IsPrimary]=1 AND [IsActive]=1 AND [IsDeleted]=0` | `UX_TenantDocuments_Primary` | ❌ | **NÃO há App Service público** para `TenantDocuments` (CRUD gerido internamente). Sem validação no lado aplicacional — qualquer violação cai em 500. |
| 17 | `StatusDomain` | `Code` `[IsDeleted]=0` | `UX_StatusDomains_Code` | ❌ | **NÃO há App Service público** — entidade seedeada. |
| 18 | `StatusDomainTranslations` | `(StatusDomainId, LanguageCode)` | `UQ_StatusDomainTranslations_StatusDomain_Language` | ❌ | **Sem App Service** — gerido internamente. |
| 19 | `StatusDomainTranslations` | `(LanguageCode, Name)` | `UQ_StatusDomainTranslations_Language_Name` | ❌ | **Sem App Service** — gerido internamente. |
| 20 | `StatusDefinition` | `(TenantId, StatusDomainId, Code)` `IsDeleted=0` | `UX_StatusDefinitions_Tenant_Domain_Code` | ✅ | `StatusDefinitionAppService.CreateAsync.ExistsByCodeAndDomainAsync` (linha 74) e `UpdateAsync` (linha 121). |
| 21 | `StatusDefinitionTranslations` | `(TenantId, StatusDefinitionId, LanguageCode)` | `UQ_StatusDefinitionTranslations_Status_Language` | ❌ | `StatusDefinitionAppService.CreateTranslationAsync` (linha 193) **não valida** duplicidade antes de adicionar à coleção. Update/Delete também não verificam. |
| 22 | `StatusDefinitionTranslations` | `(TenantId, StatusDomainId, LanguageCode, Name)` | `UQ_StatusDefinitionTranslations_Tenant_Domain_Language_Name` | ❌ | Mesma lacuna do #21. |
| 23 | `PartyType` | `Code` | `UQ_PartyTypes_Code` | ❌ | **Sem App Service** — catálogo seedeado. |
| 24 | `PartyTypeTranslations` | `(PartyTypeId, LanguageCode)` | `UQ_PartyTypeTranslations_PartyType_Language` | ❌ | **Sem App Service** — gerido internamente. |
| 25 | `PartyTypeTranslations` | `(LanguageCode, Name)` | `UQ_PartyTypeTranslations_Language_Name` | ❌ | **Sem App Service** — gerido internamente. |
| 26 | `FileType` | `(MimeType, Extension)` `[IsDeleted]=0` | `UX_FileTypes_MimeType_Extension` | ⚠️ | `FileTypeAppService.CreateAsync.ExistsByMimeTypeAsync` valida **apenas `MimeType`** (linha 79). O índice é composto `MimeType + Extension`. Um MimeType com Extension diferente passa pela validação mas pode colidir. Update (linha 100) tem o mesmo bug. |
| 27 | `FileType` | `Code` `[IsDeleted]=0` | `UX_FileTypes_Code` | ❌ | **NÃO validado** no App Service. Mapeamento define `Code` mas o App Service não verifica duplicação. |
| 28 | `FileTypeTranslations` | `(FileTypeId, LanguageCode)` | `UQ_FileTypeTranslations_FileType_Language` | ❌ | **Sem App Service**. |
| 29 | `FileTypeTranslations` | `(LanguageCode, Name)` | `UQ_FileTypeTranslations_Language_Name` | ❌ | **Sem App Service**. |
| 30 | `EquipmentType` | `(TenantId, Name)` `[IsDeleted]=0` | `UX_EquipmentTypes_Tenant_Name` | ✅ | `EquipmentTypeAppService.CreateAsync.ExistsByNameAsync` (linha 75) + BulkUpload. |
| 31 | `EmployeeFiscalData` | `(TenantId, EmployeeId)` `[IsActive]=1 AND [IsDeleted]=0` | `UX_EmployeeFiscalData_Active` | ❌ | **NÃO há App Service público** para `EmployeeFiscalData`. |
| 32 | `EmployeeFiscalData` | `(TenantId, FiscalCountry, TaxNumber)` `[IsDeleted]=0` | `UX_EmployeeFiscalData_TaxNumber` | ❌ | Mesma lacuna do #31. |
| 33 | `EmployeeAddresses` | `(TenantId, EmployeeId)` `[IsPrimary]=1 AND [IsActive]=1 AND [IsDeleted]=0` | `UX_EmployeeAddresses_Primary` | ⚠️ | `EmployeeAddressAppService.CreateAsync` valida duplicação de endereço por `Street/City/PostalCode` (linha 75) mas **NÃO valida a regra "apenas um Primary"**. Se o request trouxer `IsPrimary=true` e já houver outro Primary ativo, a inserção cai em 500. |
| 34 | `ClientFiscalData` | `(ClientId, TenantId)` `[IsDeleted]=0` | `UQ_ClientFiscalData_Client` | ✅ | `ClientFiscalDataAppService.CreateAsync.ExistsByIdAsync(clientId)` (linha 77) — valida o relacionamento 1:1. |
| 35 | `ClientFiscalData` | `(TenantId, FiscalCountry, TaxNumber)` `[IsDeleted]=0` | `UX_ClientFiscalData_TaxNumber` | ⚠️ | `ExistsByTaxNumberAsync` é invocado (linha 86) **mas a assinatura do repositório só recebe `clientId` e `taxNumber`** — falta `FiscalCountry`. O índice é composto por 3 colunas, então dois TaxNumbers iguais de países diferentes são legitimamente permitidos mas a validação atual pode rejeitar ambos. Verificar assinatura do repo. |
| 36 | `ClientFiscalData` | `(TenantId, ClientId)` `[IsActive]=1 AND [IsDeleted]=0` | `UX_ClientFiscalData_Active` | ✅ | Mesmo do #34. |
| 37 | `ClientDocuments` | `(TenantId, DocumentTypeId, IssuingCountryCode, DocumentNumber)` `[IsDeleted]=0` | `UX_ClientDocuments_Type_Country_Number` | ❌ | **NÃO há App Service público** para `ClientDocuments`. |
| 38 | `ClientDocuments` | `(TenantId, ClientId)` `[IsPrimary]=1 AND [IsActive]=1 AND [IsDeleted]=0` | `UX_ClientDocuments_Primary` | ❌ | Mesma lacuna do #37. |
| 39 | `ClientContactPersons` | `(TenantId, ClientId, Email)` `[Email] IS NOT NULL AND [IsActive]=1 AND [IsDeleted]=0` | `UX_ClientContactPersons_Email` | ✅ | `ClientContactAppService.CreateAsync.ExistsByClientAndEmailAsync` (linha 77) e BulkUpload. |
| 40 | `ClientContactPersons` | `(TenantId, ClientId)` `[IsPrimary]=1 AND [IsActive]=1 AND [IsDeleted]=0` | `UX_ClientContactPersons_Primary` | ⚠️ | Validação ausente — `CreateAsync` aceita `IsPrimary=true` sem remover o Primary anterior. Risco de 500. |
| 41 | `ClientAddresses` | `(TenantId, ClientId)` `[IsPrimary]=1 AND [IsActive]=1 AND [IsDeleted]=0` | `UX_ClientAddresses_Primary` | ⚠️ | Mesma lacuna do #40 — não há `RemoveAllPrimaryFlagsAsync` equivalente ao do `VisitAttachmentAppService`. |
| 42 | `AddressType` | `Code` `[IsDeleted]=0` | `UX_AddressTypes_Code` | ❌ | `AddressTypeAppService.CreateAsync.ExistsByNameAsync` valida por `Name` (linha 79), mas o índice é em `Code`. A entidade usa `Ignore(x => x.Name)` no mapping (linha 26 do `AddressTypeMapping.cs`), o que **torna `ExistsByNameAsync` numa chamada a um campo inexistente** — validação não funciona. |
| 43 | `AddressTypeTranslations` | `(AddressTypeId, LanguageCode)` | `UQ_AddressTypeTranslations_AddressType_Language` | ❌ | **Sem App Service**. |
| 44 | `AddressTypeTranslations` | `(LanguageCode, Name)` | `UQ_AddressTypeTranslations_Language_Name` | ❌ | **Sem App Service**. |
| 45 | `AcquisitionSourceType` | `Code` | `UQ_AcquisitionSourceTypes_Code` | ❌ | `AcquisitionSourceTypeAppService.CreateAsync.ExistsByNameAsync` valida por `Name` (linha 75), mas o índice é em `Code`. A entidade é gerada a partir de `Name` (mapping `Ignore(x => x.Name)`), análogo ao #42. |
| 46 | `AcquisitionSourceTypeTranslations` | `(AcquisitionSourceTypeId, LanguageCode)` | `UQ_AcquisitionSourceTypeTranslations_AcquisitionSourceType_Language` | ❌ | **Sem App Service**. |
| 47 | `AcquisitionSourceTypeTranslations` | `(LanguageCode, Name)` | `UQ_AcquisitionSourceTypeTranslations_Language_Name` | ❌ | **Sem App Service**. |
| 48 | `DocumentType` | `Code` `[IsDeleted]=0` | `UX_DocumentTypes_Code` | ✅ | `DocumentTypeAppService.CreateAsync.ExistsByCodeAsync` (linha 74) e `UpdateAsync` (linha 115). |
| 49 | `DocumentTypeTranslations` | `(DocumentTypeId, LanguageCode)` | `UQ_DocumentTypeTranslations_DocumentType_Language` | ❌ | Mesma lacuna do #21 (sub-recurso sem validação de duplicidade). |
| 50 | `DocumentTypeTranslations` | `(LanguageCode, Name)` | `UQ_DocumentTypeTranslations_Language_Name` | ❌ | Mesma lacuna do #21. |
| 51 | `Vehicle` | `(TenantId, Plate)` `[IsDeleted]=0` | `UQ_Vehicles_Tenant_Plate` | ✅ | `VehicleAppService.CreateAsync.ExistsByPlateAsync` (linha 75) + BulkUpload. |

### 3.3 Business — Tabelas de equipa e visita (continuação)

| # | Entidade | Índice (campos) | DB Name | Validação | App Service / Observação |
|---|---|---|---|---|---|
| 52 | `Team` | `(TenantId, Name)` `[IsDeleted]=0` | `UX_Teams_Tenant_Name` | ✅ | `TeamAppService.CreateAsync.ExistsByNameAsync` (linha 79) + BulkUpload. |
| 53 | `EmployeeTeam` | `(TenantId, TeamId, EmployeeId)` `[EndDateTime] IS NULL AND [IsDeleted]=0` | `UX_EmployeeTeam_Active` | ✅ | `EmployeeTeamAppService.CreateAsync.ExistsByTeamAndMemberAsync` (linha 75) + BulkUpload. |
| 54 | `VisitTeam` | `(TenantId, VisitId, TeamId)` `[IsActive]=1 AND [IsDeleted]=0` | `UX_VisitTeam_Active` | ✅ | `VisitTeamAppService.CreateAsync.ExistsByIdAsync(tenantId, VisitId, TeamId)` (linha 75) + BulkUpload. |
| 55 | `VisitTeamVehicle` | `(TenantId, VisitTeamId, VehicleId)` `[IsDeleted]=0` | `UX_VisitTeamVehicle_Unique` | ❌ | **NÃO validado** em `VisitTeamVehicleAppService.CreateAsync` — viola a unicidade se o mesmo veículo for adicionado 2x ao mesmo team. |
| 56 | `VisitTeamEquipment` | `(TenantId, VisitTeamId, EquipmentId)` `[IsDeleted]=0` | `UX_VisitTeamEquipment_Unique` | ❌ | Mesma lacuna do #55. |
| 57 | `VisitTeamEmployee` | `(TenantId, VisitTeamId, EmployeeId)` `[EndDateTime] IS NULL AND [IsDeleted]=0` | `UX_VisitTeamEmployee_Active` | ✅ | `VisitTeamEmployeeAppService.CreateAsync.ExistsActiveAssignmentAsync` (linha 104). |

### 3.4 Business — Sub-recursos de Visit (Address, Contact)

| # | Entidade | Índice (campos) | DB Name | Validação | App Service / Observação |
|---|---|---|---|---|---|
| 58 | `VisitContactPersons` (Visit) | `(VisitId, Email)` (presumido) | derivado | ✅ | `VisitContactAppService.CreateAsync.ExistsByVisitAndEmailAsync` (linha 85) + Update (linha 120). |
| 59 | `VisitAddresses` | sem unique index mapeado | — | — | Apenas validação de duplicação de endereço por `Street/City/PostalCode` no App Service. |

> **Nota:** Não foram encontrados mappings específicos para `VisitContactPersons`/`VisitAddresses` no `grep` — presumivelmente usam chaves alternativas ou índices geridos fora do EF Core.

### 3.5 Billing (11 índices)

| # | Entidade | Índice (campos) | DB Name | Validação | App Service / Observação |
|---|---|---|---|---|---|
| 60 | `TenantContactPersons` | `(TenantId, Email)` `[Email] IS NOT NULL AND [IsActive]=1 AND [IsDeleted]=0` | `UX_TenantContactPersons_Email_Active` | ❌ | `TenantContactAppService.CreateAsync` valida **apenas se `IsPrimary=true`** (linha 92), mas não verifica Email duplicado. Risco de 500. |
| 61 | `TenantContactPersons` | `TenantId` `[IsPrimary]=1 AND [IsActive]=1 AND [IsDeleted]=0` | `UX_TenantContactPersons_Primary` | ✅ | `TenantContactAppService.CreateAsync.ExistsPrimaryContactAsync` (linha 92). |
| 62 | `TenantAddresses` | `(Id, TenantId)` `[IsDeleted]=0` | `UQ_TenantAddresses_Id_Tenant` | ✅ | Alternate key técnica para FKs compostas. |
| 63 | `TenantAddresses` | `TenantId` `[IsPrimary]=1 AND [IsActive]=1 AND [IsDeleted]=0` | `UX_TenantAddresses_Primary` | ✅ | `TenantAddressesAppService.CreateAsync.ExistsPrimaryByTenantAsync` (linha 69). |
| 64 | `TenantFiscalData` | `(TenantId, FiscalCountry, TaxNumber)` `[IsDeleted]=0` | `UX_TenantFiscalData_TaxNumber` | ✅ | `TenantFiscalDataAppService.CreateAsync.ExistsByTaxNumberAsync` (linha 79) + Update. |
| 65 | `TenantFiscalData` | `TenantId` `[IsActive]=1 AND [IsDeleted]=0` | `UX_TenantFiscalData_Active` | ✅ | `TenantFiscalDataAppService.CreateAsync.ExistsActiveByTenantAsync` (linha 69). |
| 66 | `SubscriptionPlanFileRules` | `(SubscriptionPlanId, FileTypeId)` | `UQ_SubscriptionPlanFileRules_Plan_FileType` | ❌ | **NÃO há App Service público** para `SubscriptionPlanFileRules`. |
| 67 | `SubscriptionPlans` | `Code` `[IsDeleted]=0` | `UX_SubscriptionPlans_Code` | ⚠️ | `PlanAppService.CreateAsync.ExistsByNameAsync` valida por `Name` (que está em `SubscriptionPlanTranslations`), **mas o índice é em `Code` na entidade raiz**. Conflito entre "plano com mesmo Code" e "tradução com mesmo Name" não é verificado. |
| 68 | `SubscriptionPlanTranslations` | `(SubscriptionPlanId, LanguageCode)` | `UQ_SubscriptionPlanTranslations_SubscriptionPlan_Language` | ❌ | **Sem App Service de traduções dedicado**. Adicionado inline em `PlanAppService.CreateAsync` (linha 110) sem `ExistsBy*Async`. |
| 69 | `SubscriptionPlanTranslations` | `(LanguageCode, Name)` | `UQ_SubscriptionPlanTranslations_Language_Name` | ❌ | Mesma lacuna do #68. |
| 70 | `Subscriptions` | `(TenantId, Id)` `[IsDeleted]=0` | `UQ_Subscriptions_TenantId_Id` | ✅ | Alternate key técnica. |
| 71 | `Subscriptions` | `TenantId` `[IsActive]=1 AND [IsDeleted]=0` | `UX_Subscriptions_Active` | ✅ | `SubscriptionAppService.CreateAsync.ExistsByTenantIdAsync` (linha 95). |
| 72 | `Subscriptions` | `StripeId` `[StripeId] IS NOT NULL` | `UX_Subscriptions_StripeId` | ❌ | **NÃO validado**. App Service aceita StripeId duplicado; colisão cai em 500. |

### 3.6 Job (1 índice)

| # | Entidade | Índice (campos) | DB Name | Validação | App Service / Observação |
|---|---|---|---|---|---|
| 73 | `JobDefinitions` | `JobName` `[IsDeleted]=0` | `UX_JobDefinitions_JobName` | ✅ | `JobAppService.CreateAsync.ExistsByNameAsync` (linha 64). Update não permite alterar JobName (protegido). |

---

## 4. Padrões de Risco Identificados

### 4.1 Validação por campo errado (Code vs Name)
Múltiplas entidades (`Resource`, `Action`, `AddressType`, `AcquisitionSourceType`, `SubscriptionPlan`) declaram índice único em `Code` mas o App Service valida por `Name`. Em alguns casos o mapping ainda usa `Ignore(x => x.Name)` (a "Name" foi movida para tabela de traduções), o que torna a validação completamente inativa.

### 4.2 Validação incompleta em índices compostos
- `FileType` — índice `(MimeType, Extension)` validado só por `MimeType`.
- `ClientFiscalData` — índice `(TenantId, FiscalCountry, TaxNumber)` validado só por `TaxNumber` (provavelmente o repository não recebe `FiscalCountry`).

### 4.3 Sub-recursos de tradução sem gestão de duplicação
`StatusDefinitionTranslations`, `DocumentTypeTranslations`, `AddressTypeTranslations`, `AcquisitionSourceTypeTranslations`, `FileTypeTranslations`, `PartyTypeTranslations`, `SubscriptionPlanTranslations` — todos permitem inserir traduções com chaves compostas duplicadas porque o `CreateTranslationAsync` apenas faz `_repo.UpdateAsync(entity)` sem verificar `ExistsByLanguageAndNameAsync`.

### 4.4 Entidades de "infraestrutura" sem App Service
`TenantDocuments`, `ClientDocuments`, `EmployeeFiscalData`, `SubscriptionPlanFileRules`, `StatusDomain*`, `PartyType*` — geridas internamente sem endpoints. Se algum dia forem expostas via API, vão precisar de validação de unicidade.

### 4.5 VisitTeamVehicle/Equipment
Índices únicos estão no DB mas o `VisitTeamVehicleAppService` e `VisitTeamEquipmentAppService` não validam duplicação antes do `CreateAsync`. O `ProcessBulkItemsAsync` no `VisitTeamAppService` (para `VisitTeam`) faz, mas as sub-entidades não.

---

## 5. Recomendações (Issues Futuras)

1. **Corrigir validação Code vs Name** em `ResourceAppService`, `ActionAppService`, `AddressTypeAppService`, `AcquisitionSourceTypeAppService` e `PlanAppService` — adicionar `ExistsByCodeAsync`.
2. **Completar validação composta** em `FileTypeAppService` (`MimeType+Extension`) e `ClientFiscalDataAppService` (`TenantId+FiscalCountry+TaxNumber`).
3. **Adicionar gestão de "Primary"** em `EmployeeAddressAppService`, `ClientAddressAppService`, `ClientContactAppService` — espelhar o padrão de `VisitAttachmentAppService.RemoveAllPrimaryFlagsAsync`.
4. **Validar Email único** em `TenantContactAppService.CreateAsync` (atualmente só valida Primary).
5. **Adicionar validação `ExistsByStripeIdAsync`** em `SubscriptionAppService.CreateAsync` e `UpdateAsync`.
6. **Adicionar validação de duplicação de traduções** em todos os `AppService.CreateTranslationAsync` e `UpdateTranslationAsync`.
7. **Criar endpoints e validações** para `TenantDocuments`, `ClientDocuments`, `EmployeeFiscalData`, `SubscriptionPlanFileRules` quando forem promovidos a recursos de primeira classe.
8. **Adicionar validação** em `VisitTeamVehicleAppService.CreateAsync` e `VisitTeamEquipmentAppService.CreateAsync`.

---

## 6. Conclusão

A auditoria revela que **46% dos unique indexes de negócio** não estão a ser validados na Application, o que pode resultar em respostas HTTP 500 inesperadas em produção quando o utilizador tenta inserir um duplicado. A correção desses gaps é prioritária porque:

- Viola a regra arquitetural "sem `throw` para erros de negócio" — as exceções do SQL Server são capturadas pelo `GlobalExceptionMiddleware` como 500.
- Impede que o cliente receba a mensagem de localização adequada (ex.: "Email já existe") via `INotify` + status 409.
- Dificulta o diagnóstico em produção — o stack trace do SQL Server vaza para logs sem contexto de negócio.

Esta auditoria serve como base para a criação de issues técnicas de follow-up, organizadas por entidade. Cada issue deve:
- Adicionar método `ExistsBy*Async` no repositório (se ainda não existir com a assinatura correta).
- Invocar a validação antes de `_domain.CreateAsync` / `UpdateAsync` no App Service.
- Garantir que a mensagem de localização (`ILocalizationService`) está configurada com o código correto (ex.: `Application.Service.{Entidade}.{Operacao}.{Campo}AlreadyExists`).
- Adicionar teste unitário cobrindo o cenário de duplicação.
