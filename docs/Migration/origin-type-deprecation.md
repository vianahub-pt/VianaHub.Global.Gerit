# Deprecação do OriginType Legado

## O que foi removido

### 1. `OriginType` enum (`Domain/Enums/OriginType.cs`)
- Enum com valores: `Outros=1, Instagram=2, Facebook=3, Google=4, ..., Eventos=14`
- **Nenhuma referência no código fonte** — dead code confirmado por grep
- Substituído por:
  - `AcquisitionSourceTypeId` (FK → `AcquisitionSourceTypeEntity`) para `Tenant` e `Client`
  - `ConsentOriginTypeId` (FK → `ConsentOriginTypeEntity`) para `ClientConsents`

### 2. `OriginTypeEntity` CRUD completo
Entidade que armazenava tipos de origem gerais (Instagram, Facebook, etc.). Removido porque:
- **Não registrado em `DependencyInjection.cs`** — nenhuma dependência registrada, endpoint `/v1/origin-types` quebraria em runtime
- **Nenhum outro módulo ou entidade referenciava `OriginTypeEntity`**
- **Nenhum DbSet no `GeritDbContext`**

Arquivos removidos:

| Camada | Arquivos |
|--------|----------|
| **Domain** | `OriginTypeEntity.cs`, `IOriginTypeDataRepository.cs`, `IOriginTypeDomainService.cs`, `OriginTypeDomainService.cs`, `OriginTypeValidator.cs`, `CreateOriginTypeValidator.cs`, `UpdateOriginTypeValidator.cs`, `ActivateOriginTypeValidator.cs`, `DeactivateOriginTypeValidator.cs`, `DeleteOriginTypeValidator.cs` |
| **Application** | `OriginTypeAppService.cs`, `IOriginTypeAppService.cs`, `OriginTypeMappingProfile.cs`, `CreateOriginTypeRequest.cs`, `UpdateOriginTypeRequest.cs`, `BulkUploadOriginTypeItem.cs`, `OriginTypeResponse.cs` |
| **Infra.Data** | `OriginTypeDataRepository.cs`, `OriginTypeMapping.cs` |
| **Api** | `OriginTypeEndpoint.cs`, `CreateOriginTypeRouteValidator.cs`, `UpdateOriginTypeRouteValidator.cs` |

### 3. Localizações legadas
- `Application.Service.OriginType.*` (15 chaves em 3 idiomas)
- `Swagger.Tag.OriginTypes` + `Swagger.Endpoint.OriginType.*` (9 chaves em 3 idiomas)
- `Api.Validator.Client.Create.OriginType` (3 chaves)

## O que foi mantido

| Item | Motivo |
|------|--------|
| `ConsentOriginTypeEntity` + CRUD completo | Usado por `ClientConsentsEntity` como FK `ConsentOriginTypeId`. Registrado em DI. |
| `AcquisitionSourceTypeEntity` + CRUD completo | Usado por `TenantEntity` e `ClientEntity` como FK `AcquisitionSourceTypeId`. Registrado em DI. |

## Regras futuras

- **`ClientEntity`**: usa exclusivamente `AcquisitionSourceTypeId` (FK → `AcquisitionSourceTypeEntity`)
- **`ClientConsentsEntity`**: usa exclusivamente `ConsentOriginTypeId` (FK → `ConsentOriginTypeEntity`)
- **`TenantEntity`**: usa exclusivamente `AcquisitionSourceTypeId` (FK → `AcquisitionSourceTypeEntity`)
- **Não reintroduzir** o enum `OriginType` ou a entidade `OriginTypeEntity`
