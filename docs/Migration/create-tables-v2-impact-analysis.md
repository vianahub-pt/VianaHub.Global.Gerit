# Relatório de Impacto — Migração Create-Tables v2

> Documento gerado em: 28/06/2026  
> Escopo: Análise completa da implementação atual vs schema esperado pelo novo Create-Tables.sql

---

## 1. Arquitetura Encontrada por Camada

### Solução (.sln)
- **Solution:** VianaHub.Global.Gerit.sln
- **Arquitetura:** DDD + Clean Architecture + Hexagonal, multi-tenant SaaS (Gerit)
- **.NET 8**, ASP.NET Core Minimal API

### Projetos da Solution

| Projeto | Função |
|---------|--------|
| src/*.Domain | Entidades, Enums, Validators (FluentValidation), Interfaces, Domain Services, Value Objects |
| src/*.Application | App Services (orquestração), DTOs (Request/Response), AutoMapper Profiles, Interfaces |
| src/*.Infra.Data | EF Core DbContext, Mappings, Repositories, Interceptors, Seeders |
| src/*.Infra.IoC | Ponto único de DI (DependencyInjection.cs) |
| src/*.Infra.Integration | Integrações externas (e-mail, etc.) |
| src/*.Infra.Job | Jobs Hangfire, Hosted Services |
| src/*.Api | Endpoints Minimal API, Route Validators, Localization (pt-PT, en-US, es-ES) |
| 	ests/*.Tests | Testes unitários (xUnit + Moq + NBuilder) |

### Estrutura de diretórios do Domain para as entidades-alvo

`
src/VianaHub.Global.Gerit.Domain/
├── Entities/
│   ├── Billing/
│   │   └── TenantEntity.cs
│   └── Business/
│       ├── ClientEntity.cs
│       ├── ClientConsentsEntity.cs
│       ├── ConsentTypeEntity.cs
│       └── OriginTypeEntity.cs
├── Enums/
│   ├── OriginType.cs (enum)
│   └── TenantType.cs (enum)
├── Interfaces/
│   ├── Billing/
│   │   ├── ITenantDomainService.cs
│   │   └── ITenantDataRepository.cs
│   └── Business/
│       ├── IOriginTypeDomainService.cs
│       ├── IOriginTypeDataRepository.cs
│       ├── IClientDomainService.cs
│       ├── IClientRepository.cs
│       ├── IClientConsentsDomainService.cs
│       └── IClientConsentsDataRepository.cs
├── Services/
│   ├── Billing/
│   │   └── TenantDomainService.cs
│   └── Business/
│       ├── OriginTypeDomainService.cs
│       ├── ClientDomainService.cs
│       └── ClientConsentsDomainService.cs
└── Validators/
    ├── Billing/Tenant/
    │   ├── TenantValidator.cs
    │   ├── CreateTenantValidator.cs
    │   ├── UpdateTenantValidator.cs
    │   ├── ActivateTenantValidator.cs
    │   ├── DeactivateTenantValidator.cs
    │   └── DeleteTenantValidator.cs
    └── Business/
        ├── Client/
        │   ├── ClientValidator.cs
        │   ├── CreateClientValidator.cs
        │   ├── UpdateClientValidator.cs
        │   ├── ActivateClientValidator.cs
        │   ├── DeactivateClientValidator.cs
        │   └── DeleteClientValidator.cs
        ├── ClientConsents/
        │   ├── ClientConsentsValidator.cs
        │   ├── CreateClientConsentsValidator.cs
        │   ├── UpdateClientConsentsValidator.cs
        │   ├── ActivateClientConsentsValidator.cs
        │   ├── DeactivateClientConsentsValidator.cs
        │   └── DeleteClientConsentsValidator.cs
        └── OriginType/
            ├── OriginTypeValidator.cs
            ├── CreateOriginTypeValidator.cs
            ├── UpdateOriginTypeValidator.cs
            ├── ActivateOriginTypeValidator.cs
            ├── DeactivateOriginTypeValidator.cs
            └── DeleteOriginTypeValidator.cs
`

---

## 2. Arquivos Impactados

### 2.1. Domain — Entidades

| Arquivo | Situação | O que precisa mudar |
|---------|----------|---------------------|
| src/.../Domain/Entities/Billing/TenantEntity.cs | **PRECISA ALTERAR** | Substituir OriginType OriginType (enum) por int AcquisitionSourceTypeId (FK para catálogo global). Manter TenantType. |
| src/.../Domain/Entities/Business/ClientEntity.cs | **PRECISA ALTERAR** | Substituir OriginType OriginType (enum) por int AcquisitionSourceTypeId (FK). |
| src/.../Domain/Entities/Business/ClientConsentsEntity.cs | **PRECISA ALTERAR** | Substituir string Origin por int ConsentOriginTypeId (FK). |
| src/.../Domain/Entities/Business/OriginTypeEntity.cs | **SERÁ SUBSTITUÍDO/REMOVER** | O catálogo OriginTypes (tenant-scoped) será substituído pelo catálogo global AcquisitionSourceTypes (sem tenant). Ou será mantido como complemento? Decisão de negócio necessária. |
| src/.../Domain/Entities/Business/ConsentTypeEntity.cs | **SEM ALTERAÇÃO ESTRUTURAL** | Permanece como catálogo global. Verificar se o Name e Description estão alinhados com o novo ConsentTypes do SQL. |
| src/.../Domain/Enums/OriginType.cs | **SERÁ REMOVIDO** | Os valores fixos do enum (Instagram, Facebook, etc.) serão migrados para registros na tabela AcquisitionSourceTypes. O enum como tipo de dado deixará de existir. |
| src/.../Domain/Enums/TenantType.cs | **MANTER** | Permanece como enum (coluna TenantType INT no SQL). |

### 2.2. Domain — Interfaces

| Arquivo | Situação | O que precisa mudar |
|---------|----------|---------------------|
| src/.../Domain/Interfaces/Billing/ITenantDomainService.cs | **MANTER** | Assinaturas não mudam (recebem TenantEntity). |
| src/.../Domain/Interfaces/Billing/ITenantDataRepository.cs | **MANTER** | Não referencia OriginType diretamente. |
| src/.../Domain/Interfaces/Business/IOriginTypeDomainService.cs | **SERÁ REMOVIDO/SUBSTITUIDO** | Servirá para AcquisitionSourceType se houver domain service. |
| src/.../Domain/Interfaces/Business/IOriginTypeDataRepository.cs | **SERÁ REMOVIDO/SUBSTITUIDO** | Idem. Referencia ExistsByNameAsync(int tenantId, ...) — AcquisitionSourceTypes é global, sem tenant. |
| src/.../Domain/Interfaces/Business/IClientRepository.cs | **MANTER** | Assinatura não referencia OriginType diretamente. |
| src/.../Domain/Interfaces/Business/IClientConsentsDataRepository.cs | **MANTER** | Assinatura não referencia Origin diretamente. |
| src/.../Domain/Interfaces/Business/IClientConsentsDomainService.cs | **MANTER** | Idem. |

### 2.3. Domain — Domain Services

| Arquivo | Situação | O que precisa mudar |
|---------|----------|---------------------|
| src/.../Domain/Services/Billing/TenantDomainService.cs | **MANTER** | Lógica não muda. |
| src/.../Domain/Services/Business/OriginTypeDomainService.cs | **SERÁ REMOVIDO/SUBSTITUIDO** | Precisa ser recriado para AcquisitionSourceTypeDomainService ou adaptado. |
| src/.../Domain/Services/Business/ClientDomainService.cs | **MANTER** | Código não referencia OriginType diretamente. |
| src/.../Domain/Services/Business/ClientConsentsDomainService.cs | **MANTER** | Código não referencia Origin diretamente. |

### 2.4. Domain — Validators (FluentValidation)

| Arquivo | Situação | O que precisa mudar |
|---------|----------|---------------------|
| src/.../Domain/Validators/Billing/Tenant/CreateTenantValidator.cs | **MANTER** | Só valida Name. |
| src/.../Domain/Validators/Billing/Tenant/UpdateTenantValidator.cs | **MANTER** | Só valida Name. |
| src/.../Domain/Validators/Business/Client/CreateClientValidator.cs | **MANTER** | Não valida OriginType. |
| src/.../Domain/Validators/Business/Client/UpdateClientValidator.cs | **MANTER** | Não valida OriginType. |
| src/.../Domain/Validators/Business/ClientConsents/CreateClientConsentsValidator.cs | **PRECISA ALTERAR** | Validava Origin (string) — precisa validar ConsentOriginTypeId (int > 0). |
| src/.../Domain/Validators/Business/ClientConsents/UpdateClientConsentsValidator.cs | **PRECISA ALTERAR** | Validava Origin (string com valores fixos "Web","Mobile","Paper","API") — precisa mudar para validar ConsentOriginTypeId. |
| src/.../Domain/Validators/Business/ClientConsents/ClientConsentsValidator.cs | **PRECISA ALTERAR** | Referencia Origin nas validações de Create/Update. |
| src/.../Domain/Validators/Business/OriginType/*.cs (6 arquivos) | **SERÃO REMOVIDOS/SUBSTITUIDOS** | Devem ser recriados para AcquisitionSourceType. |

### 2.5. Application — App Services

| Arquivo | Situação | O que precisa mudar |
|---------|----------|---------------------|
| src/.../Application/Services/Billing/TenantAppService.cs | **PRECISA ALTERAR** | Linha 78: 
ew TenantEntity((TenantType)..., (OriginType)request.OriginType, ...) — mudar para receber AcquisitionSourceTypeId. Linha 92: mesmo no Update. Linha 250: mesmo no BulkUpload. |
| src/.../Application/Services/Business/ClientAppService.cs | **PRECISA ALTERAR** | Linha 85: 
ew ClientEntity(..., (OriginType)request.OriginType, ...) — mudar para equest.AcquisitionSourceTypeId. Linha 116: client.Update(..., (OriginType)request.OriginType, ...). Linha 283: BulkUpload. |
| src/.../Application/Services/Business/ClientConsentsAppService.cs | **PRECISA ALTERAR** | Linha 76-85: construtor recebe equest.Origin (string) — mudar para equest.ConsentOriginTypeId (int). |
| src/.../Application/Services/Business/OriginTypeAppService.cs | **SERÁ SUBSTITUIDO** | Precisa ser refatorado para AcquisitionSourceTypeAppService ou adaptado. |

### 2.6. Application — DTOs (Request/Response)

| Arquivo | Situação | O que precisa mudar |
|---------|----------|---------------------|
| src/.../Application/Dtos/Request/Billing/Tenant/CreateTenantRequest.cs | **PRECISA ALTERAR** | int OriginType → int AcquisitionSourceTypeId. |
| src/.../Application/Dtos/Request/Billing/Tenant/UpdateTenantRequest.cs | **PRECISA ALTERAR** | int OriginType → int AcquisitionSourceTypeId. |
| src/.../Application/Dtos/Request/Billing/Tenant/BulkUploadTenantItem.cs | **PRECISA ALTERAR** | int OriginType → int AcquisitionSourceTypeId. |
| src/.../Application/Dtos/Request/Business/Client/CreateClientRequest.cs | **PRECISA ALTERAR** | int OriginType → int AcquisitionSourceTypeId. |
| src/.../Application/Dtos/Request/Business/Client/UpdateClientRequest.cs | **PRECISA ALTERAR** | int OriginType → int AcquisitionSourceTypeId. |
| src/.../Application/Dtos/Request/Business/Client/BulkUploadClientItem.cs | **PRECISA ALTERAR** | int OriginType + int Origin → int AcquisitionSourceTypeId. |
| src/.../Application/Dtos/Request/Business/ClientConsents/CreateClientConsentsRequest.cs | **PRECISA ALTERAR** | string Origin → int ConsentOriginTypeId. |
| src/.../Application/Dtos/Request/Business/ClientConsents/UpdateClientConsentsRequest.cs | **MANTER** | Atualmente só tem Granted e RevokedDate. |
| src/.../Application/Dtos/Response/Business/Client/ClientDetailResponse.cs | **PRECISA ALTERAR** | int OriginType + string OriginTypeDescription → int AcquisitionSourceTypeId + string AcquisitionSourceTypeDescription. |
| src/.../Application/Dtos/Response/Business/ClientConsents/ClientConsentsResponse.cs | **PRECISA ALTERAR** | string Origin → int ConsentOriginTypeId + talvez string ConsentOriginTypeDescription. |
| src/.../Application/Dtos/Response/Business/OriginType/OriginTypeResponse.cs | **SERÁ SUBSTITUIDO** | Será substituído por AcquisitionSourceTypeResponse. TenantId removido (catálogo global). |
| src/.../Application/Dtos/Request/Business/OriginType/*.cs (3 arquivos) | **SERÃO SUBSTITUIDOS** | CreateOriginTypeRequest, UpdateOriginTypeRequest, BulkUploadOriginTypeItem. |

### 2.7. Application — AutoMapper Profiles

| Arquivo | Situação | O que precisa mudar |
|---------|----------|---------------------|
| src/.../Application/Mappings/Billing/TenantMappingProfile.cs | **MANTER** | TenantEntity → TenantResponse não inclui OriginType no response atual. |
| src/.../Application/Mappings/Business/ClientMappingProfile.cs | **PRECISA ALTERAR** | Linha 26-27: mapeia OriginType (enum) para int e Description — precisa mudar para AcquisitionSourceTypeId. |
| src/.../Application/Mappings/Business/ClientConsentsMappingProfile.cs | **PRECISA ALTERAR** | Mapeia Origin (string) → Origin (string). |
| src/.../Application/Mappings/Business/OriginTypeMappingProfile.cs | **SERÁ SUBSTITUIDO** | Substituir por AcquisitionSourceTypeMappingProfile. |

### 2.8. Application — Interfaces

| Arquivo | Situação | O que precisa mudar |
|---------|----------|---------------------|
| src/.../Application/Interfaces/Billing/ITenantAppService.cs | **MANTER** | Assinaturas não mudam. |
| src/.../Application/Interfaces/Business/IClientAppService.cs | **MANTER** | Assinaturas não mudam. |
| src/.../Application/Interfaces/Business/IClientConsentsAppService.cs | **MANTER** | Assinaturas não mudam. |
| src/.../Application/Interfaces/Business/IOriginTypeAppService.cs | **SERÁ SUBSTITUIDO** | Será IAcquisitionSourceTypeAppService. |

### 2.9. Infra.Data — Mappings (EF Core)

| Arquivo | Situação | O que precisa mudar |
|---------|----------|---------------------|
| src/.../Infra.Data/Mappings/Billing/TenantMapping.cs | **PRECISA ALTERAR** | Adicionar uilder.Property(x => x.AcquisitionSourceTypeId) e FK FK_Tenants_AcquisitionSourceType. Remover mapeamento do enum OriginType. |
| src/.../Infra.Data/Mappings/Business/ClientMapping.cs | **PRECISA ALTERAR** | Linha 38-40: uilder.Property(x => x.OriginType) → uilder.Property(x => x.AcquisitionSourceTypeId) com FK. |
| src/.../Infra.Data/Mappings/Business/ClientConsentsMapping.cs | **PRECISA ALTERAR** | Linha 38-41: uilder.Property(x => x.Origin) → uilder.Property(x => x.ConsentOriginTypeId) com FK para ConsentOriginTypes. Mudar tamanho/tipo. |
| src/.../Infra.Data/Mappings/Business/OriginTypeMapping.cs | **SERÁ SUBSTITUIDO/REMOVER** | Mapeia OriginTypes (tenant-scoped) → será substituído por AcquisitionSourceTypeMapping (global). |

### 2.10. Infra.Data — Repositories

| Arquivo | Situação | O que precisa mudar |
|---------|----------|---------------------|
| src/.../Infra.Data/Repository/Billing/TenantDataRepository.cs | **MANTER** | Código não referencia OriginType diretamente. |
| src/.../Infra.Data/Repository/Business/ClientConsentRepository.cs | **PRECISA ALTERAR** | Linha 60-61: x.Origin.Contains(filter.Search) — precisa ser ajustado. |
| src/.../Infra.Data/Repository/Business/OriginTypeDataRepository.cs | **SERÁ SUBSTITUIDO** | Repositório tenant-scoped. O novo AcquisitionSourceType é global (sem tenant). |

### 2.11. Infra.Data — Contexto e Seeders

| Arquivo | Situação | O que precisa mudar |
|---------|----------|---------------------|
| src/.../Infra.Data/Context/GeritDbContext.cs | **PRECISA ALTERAR** | Adicionar DbSet<AcquisitionSourceTypeEntity> e DbSet<ConsentOriginTypeEntity> (ou entidades similares). Remover se existir DbSet<OriginTypeEntity>. |
| src/.../Infra.Data/Seeders/DatabaseSeeder.cs | **PRECISA ALTERAR** | Atualmente vazio. Precisa ser populado com seed dos catálogos globais (AcquisitionSourceTypes, ConsentOriginTypes). |

### 2.12. Infra.IoC — Dependency Injection

| Arquivo | Situação | O que precisa mudar |
|---------|----------|---------------------|
| src/.../Infra.IoC/DependencyInjection.cs | **PRECISA ALTERAR** | Trocar registros de IOriginType* por IAcquisitionSourceType*. Remover/adicionar validators. |

### 2.13. API — Endpoints

| Arquivo | Situação | O que precisa mudar |
|---------|----------|---------------------|
| src/.../Api/Endpoints/Billing/TenantEndpoint.cs | **MANTER** | Rota /v1/tenants não expõe OriginType diretamente no corpo. |
| src/.../Api/Endpoints/Business/ClientEndpoint.cs | **MANTER** | Rota /v1/clients. Contrato da API precisa ser ajustado no DTO, não no endpoint. |
| src/.../Api/Endpoints/Business/ClientConsentsEndpoint.cs | **MANTER** | Rota /v1/clients/{clientId}/consents. |
| src/.../Api/Endpoints/Business/OriginTypeEndpoint.cs | **SERÁ REMOVIDO/SUBSTITUIDO** | Rota /v1/origin-types → /v1/acquisition-source-types. |

### 2.14. API — Route Validators

| Arquivo | Situação | O que precisa mudar |
|---------|----------|---------------------|
| src/.../Api/Validators/Business/Client/CreateClientRouteValidator.cs | **PRECISA ALTERAR** | Valida OriginType — precisa mudar para AcquisitionSourceTypeId. |
| src/.../Api/Validators/Business/Client/UpdateClientRouteValidator.cs | **PRECISA ALTERAR** | Valida OriginType — precisa mudar. |
| src/.../Api/Validators/Billing/Tenant/CreateTenantRouteValidator.cs | **MANTER** | Só valida Name. |
| src/.../Api/Validators/Billing/Tenant/UpdateTenantRouteValidator.cs | **MANTER** | Só valida Name. |

### 2.15. API — Localization

| Arquivo | Situação | O que precisa mudar |
|---------|----------|---------------------|
| src/.../Api/Localization/*.{pt-PT,en-US,es-ES}.json | **PRECISA ALTERAR** | Todas as chaves de localização que referenciam OriginType, Origin, ClientConsents.Origin precisam ser revisadas e/ou substituídas. |

### 2.16. Testes

| Arquivo | Situação | O que precisa mudar |
|---------|----------|---------------------|
| 	ests/.../Domain/Entities/Business/ClientEntityTests.cs | **PRECISA ALTERAR** | Linha 50: 
ew ClientEntity(1, ..., OriginType.Outros, ...) — precisa mudar construtor. |
| 	ests/.../Application/Services/Business/ClientAppServiceTests.cs | **PRECISA ALTERAR** | Linha 93: OriginType = (int)OriginType.Outros — precisa mudar. |
| 	ests/.../Infra/Data/Repository/Business/ClientRepositoryTests.cs | **PRECISA ALTERAR** | Linhas 22, 25, 54, 57, 86, 90: OriginType.Outros — precisa mudar. |
| Não existem testes para Tenant, ClientConsents, OriginType | **CRIAR** | Cobertura de testes para essas entidades precisa ser implementada. |

---

## 3. Campos Atuais vs Esperados

### 3.1. Tenants

| Campo | Atual (Código) | Esperado (Create-Tables.sql) |
|-------|----------------|------------------------------|
| Id | int Id (Entity) | INT IDENTITY(1,1) NOT NULL |
| TenantType | TenantType TenantType (enum) | INT NOT NULL (mesmo enum) |
| OriginType / AcquisitionSourceTypeId | OriginType OriginType (enum OriginType) | INT NOT NULL FK → AcquisitionSourceTypes(Id) |
| Name | string Name | NVARCHAR(200) NOT NULL |
| Email | string Email | NVARCHAR(255) NOT NULL |
| Website | string Website | NVARCHAR(255) NULL |
| UrlImage | string UrlImage | NVARCHAR(500) NULL |
| Note | string Note | NVARCHAR(1000) NULL |
| IsActive | ool IsActive | BIT NOT NULL DEFAULT 1 |
| IsDeleted | ool IsDeleted | BIT NOT NULL DEFAULT 0 |
| CreatedBy | int CreatedBy | INT NOT NULL |
| CreatedAt | DateTime CreatedAt | DATETIME2(7) NOT NULL |
| ModifiedBy | int? ModifiedBy | INT NULL |
| ModifiedAt | DateTime? ModifiedAt | DATETIME2(7) NULL |

### 3.2. Clients

| Campo | Atual (Código) | Esperado (Create-Tables.sql) |
|-------|----------------|------------------------------|
| Id | int Id | INT IDENTITY(1,1) NOT NULL |
| TenantId | int TenantId | INT NOT NULL |
| OriginType / AcquisitionSourceTypeId | OriginType OriginType (enum) | INT NOT NULL FK → AcquisitionSourceTypes(Id) |
| ClientType | ClientType ClientType (enum) | INT NOT NULL |
| UrlImage | string UrlImage | NVARCHAR(500) NULL |
| Note | string Note | NVARCHAR(500) NULL |
| IsActive | ool IsActive | BIT NOT NULL DEFAULT 1 |
| IsDeleted | ool IsDeleted | BIT NOT NULL DEFAULT 0 |
| CreatedBy | int CreatedBy | INT NOT NULL |
| CreatedAt | DateTime CreatedAt | DATETIME2(7) NOT NULL |
| ModifiedBy | int? ModifiedBy | INT NULL |
| ModifiedAt | DateTime? ModifiedAt | DATETIME2(7) NULL |

### 3.3. ClientConsents

| Campo | Atual (Código) | Esperado (Create-Tables.sql) |
|-------|----------------|------------------------------|
| Id | int Id | INT IDENTITY(1,1) NOT NULL |
| TenantId | int TenantId | INT NOT NULL |
| ClientId | int ClientId | INT NOT NULL |
| ConsentTypeId | int ConsentTypeId | INT NOT NULL FK → ConsentTypes(Id) |
| Origin / ConsentOriginTypeId | string Origin (NVARCHAR 50) | INT NOT NULL FK → ConsentOriginTypes(Id) |
| Granted | ool Granted | BIT NOT NULL |
| GrantedDate | DateTime GrantedDate | DATETIME2(7) NOT NULL |
| RevokedDate | DateTime? RevokedDate | DATETIME2(7) NULL |
| IpAddress | string? IpAddress (NVARCHAR 50) | VARCHAR(45) NULL (mudou tipo) |
| UserAgent | string? UserAgent (NVARCHAR 500) | NVARCHAR(500) NULL |
| IsActive | ool IsActive | BIT NOT NULL DEFAULT 1 |
| IsDeleted | ool IsDeleted | BIT NOT NULL DEFAULT 0 |
| CreatedBy | int CreatedBy | INT NOT NULL |
| CreatedAt | DateTime CreatedAt | DATETIME2(7) NOT NULL |
| ModifiedBy | int? ModifiedBy | INT NULL |
| ModifiedAt | DateTime? ModifiedAt | DATETIME2(7) NULL |

> **Observação sobre IpAddress:** No código atual é NVARCHAR(50), no novo SQL é VARCHAR(45). Isso é um **breaking change** no tipo de dados.

---

## 4. Riscos de Breaking Change

### 4.1. Alto Impacto
1. **Remoção do enum OriginType** — Todo código que referencia OriginType.Outros, OriginType.Instagram, etc. quebrará. Isso afeta:
   - Construtores de TenantEntity e ClientEntity
   - Métodos Update dessas entidades
   - Testes unitários
   - Mapeamentos AutoMapper
   - DTOs de Request/Response

2. **Troca de string Origin para int ConsentOriginTypeId em ClientConsentsEntity** — Quebra:
   - Construtor e método Update da entidade
   - Validadores FluentValidation (validavam string)
   - Repositório (filtro x.Origin.Contains(...))
   - DTOs e AutoMapper
   - ClientConsentsAppService

3. **Perda de TenantId no catálogo AcquisitionSourceTypes** — O modelo atual OriginTypeEntity é por tenant (TenantId). O novo é global. Decisão de migração de dados necessária: como mapear os OriginType por tenant para valores globais?

### 4.2. Médio Impacto
4. **Mudança de IpAddress de NVARCHAR(50) para VARCHAR(45)** — Pode causar perda de dados se existirem registros com caracteres não-ASCII. Validadores precisam ser ajustados.

5. **Remoção do endpoint /v1/origin-types** — Clientes da API atual podem estar consumindo este endpoint. Precisa de depreciação ou redirecionamento.

6. **Remoção do repositório/domínio OriginType*** — Toda a stack de OriginType (Entity, DomainService, AppService, Repository, Validators) será substituída.

### 4.3. Baixo Impacto
7. **Mudança no filtro de busca do ClientConsentRepository** — Antes filtrava por x.Origin.Contains(...), agora não fará mais sentido filtrar por texto (será FK).

---

## 5. Ordem Recomendada de Implementação

### Módulo 1 — Catálogos Globais (Foundation)
1. Criar entidades AcquisitionSourceTypeEntity e ConsentOriginTypeEntity (catálogos globais, sem TenantId)
2. Criar interfaces IAcquisitionSourceTypeDataRepository, IAcquisitionSourceTypeDomainService
3. Criar repositórios e mapeamentos EF Core para as novas tabelas
4. Criar AcquisitionSourceTypeMapping.cs e ConsentOriginTypeMapping.cs
5. Atualizar GeritDbContext com os novos DbSet
6. Popular DatabaseSeeder.cs com os registros padrão dos catálogos
7. Registrar no DependencyInjection.cs
8. Criar endpoints AcquisitionSourceTypes e ConsentOriginTypes (CRUD global)

### Módulo 2 — Tenant (AcquisitionSourceTypeId)
1. Alterar TenantEntity.cs: substituir OriginType por int AcquisitionSourceTypeId
2. Atualizar TenantMapping.cs: adicionar FK para AcquisitionSourceTypes
3. Atualizar TenantAppService.cs: usar AcquisitionSourceTypeId
4. Atualizar DTOs de Tenant (CreateTenantRequest, UpdateTenantRequest, BulkUploadTenantItem)
5. Atualizar testes de Tenant

### Módulo 3 — Client (AcquisitionSourceTypeId)
1. Alterar ClientEntity.cs: substituir OriginType por int AcquisitionSourceTypeId
2. Atualizar ClientMapping.cs: adicionar FK
3. Atualizar ClientAppService.cs, ClientMappingProfile.cs
4. Atualizar DTOs de Client
5. Atualizar Route Validators
6. Atualizar testes

### Módulo 4 — ClientConsents (ConsentOriginTypeId)
1. Alterar ClientConsentsEntity.cs: substituir string Origin por int ConsentOriginTypeId
2. Atualizar ClientConsentsMapping.cs: mudar coluna e adicionar FK
3. Atualizar validadores (CreateClientConsentsValidator, UpdateClientConsentsValidator)
4. Atualizar ClientConsentsAppService.cs e DTOs
5. Atualizar ClientConsentsMappingProfile.cs (AutoMapper)
6. Ajustar ClientConsentRepository.cs (filtro de busca)
7. Atualizar testes

### Módulo 5 — Limpeza (Cleanup)
1. Remover OriginTypeEntity.cs, OriginType.cs (enum)
2. Remover OriginTypeMapping.cs
3. Remover OriginTypeDataRepository.cs e interfaces relacionadas
4. Remover OriginTypeDomainService.cs e interface
5. Remover OriginTypeAppService.cs e interface
6. Remover OriginTypeEndpoint.cs
7. Remover validadores de OriginType
8. Remover OriginTypeMappingProfile.cs (AutoMapper)
9. Remover DTOs de OriginType
10. Atualizar chaves de localização obsoletas

---

## 6. Testes Necessários

### 6.1. Testes a Criar
| Teste | Prioridade | Descrição |
|-------|-----------|-----------|
| AcquisitionSourceTypeEntityTests | Alta | Testar criação, ativação, desativação, exclusão |
| ConsentOriginTypeEntityTests | Alta | Testar criação, ativação, desativação, exclusão |
| TenantAppServiceTests | Média | Testar criação/atualização com AcquisitionSourceTypeId |
| ClientConsentsAppServiceTests | Alta | Testar criação com ConsentOriginTypeId |
| ClientConsentsRepositoryTests | Alta | Testar busca paginada sem filtro por Origin texto |

### 6.2. Testes a Ajustar
| Teste | Arquivo | Ajuste Necessário |
|-------|---------|-------------------|
| ClientEntityTests | 	ests/.../Domain/Entities/Business/ClientEntityTests.cs | Substituir OriginType.Outros por cquisitionSourceTypeId: 1 |
| ClientAppServiceTests | 	ests/.../Application/Services/Business/ClientAppServiceTests.cs | Substituir OriginType = (int)OriginType.Outros por AcquisitionSourceTypeId = 1 |
| ClientRepositoryTests | 	ests/.../Infra/Data/Repository/Business/ClientRepositoryTests.cs | Substituir OriginType.Outros nos 3 testes |

### 6.3. Cobertura de Testes por Entidade (Recomendação)
| Entidade | Testes de Domínio | Testes de Aplicação | Testes de Repositório |
|----------|-------------------|---------------------|----------------------|
| TenantEntity | **CRIAR** | **CRIAR** | **CRIAR** |
| ClientEntity | Ajustar existentes | Ajustar existentes | Ajustar existentes |
| ClientConsentsEntity | **CRIAR** | **CRIAR** | **CRIAR** |
| AcquisitionSourceTypeEntity | **CRIAR** | **CRIAR** | **CRIAR** |
| ConsentOriginTypeEntity | **CRIAR** | **CRIAR** | **CRIAR** |

---

## Apêndice A — Lista Completa de Arquivos Impactados

### Domain (22 arquivos)
- src/.../Domain/Entities/Billing/TenantEntity.cs — ALTERAR
- src/.../Domain/Entities/Business/ClientEntity.cs — ALTERAR
- src/.../Domain/Entities/Business/ClientConsentsEntity.cs — ALTERAR
- src/.../Domain/Entities/Business/OriginTypeEntity.cs — REMOVER
- src/.../Domain/Entities/Business/ConsentTypeEntity.cs — VERIFICAR
- src/.../Domain/Enums/OriginType.cs — REMOVER
- src/.../Domain/Enums/TenantType.cs — MANTER
- src/.../Domain/Interfaces/Business/IOriginTypeDataRepository.cs — REMOVER
- src/.../Domain/Interfaces/Business/IOriginTypeDomainService.cs — REMOVER
- src/.../Domain/Services/Business/OriginTypeDomainService.cs — REMOVER
- src/.../Domain/Validators/Business/ClientConsents/CreateClientConsentsValidator.cs — ALTERAR
- src/.../Domain/Validators/Business/ClientConsents/UpdateClientConsentsValidator.cs — ALTERAR
- src/.../Domain/Validators/Business/ClientConsents/ClientConsentsValidator.cs — ALTERAR
- src/.../Domain/Validators/Business/OriginType/OriginTypeValidator.cs — REMOVER
- src/.../Domain/Validators/Business/OriginType/CreateOriginTypeValidator.cs — REMOVER
- src/.../Domain/Validators/Business/OriginType/UpdateOriginTypeValidator.cs — REMOVER
- src/.../Domain/Validators/Business/OriginType/ActivateOriginTypeValidator.cs — REMOVER
- src/.../Domain/Validators/Business/OriginType/DeactivateOriginTypeValidator.cs — REMOVER
- src/.../Domain/Validators/Business/OriginType/DeleteOriginTypeValidator.cs — REMOVER

### Application (17 arquivos)
- src/.../Application/Services/Billing/TenantAppService.cs — ALTERAR
- src/.../Application/Services/Business/ClientAppService.cs — ALTERAR
- src/.../Application/Services/Business/ClientConsentsAppService.cs — ALTERAR
- src/.../Application/Services/Business/OriginTypeAppService.cs — REMOVER
- src/.../Application/Interfaces/Business/IOriginTypeAppService.cs — REMOVER
- src/.../Application/Mappings/Business/OriginTypeMappingProfile.cs — REMOVER
- src/.../Application/Mappings/Business/ClientMappingProfile.cs — ALTERAR
- src/.../Application/Mappings/Business/ClientConsentsMappingProfile.cs — ALTERAR
- src/.../Application/Dtos/Request/Billing/Tenant/CreateTenantRequest.cs — ALTERAR
- src/.../Application/Dtos/Request/Billing/Tenant/UpdateTenantRequest.cs — ALTERAR
- src/.../Application/Dtos/Request/Billing/Tenant/BulkUploadTenantItem.cs — ALTERAR
- src/.../Application/Dtos/Request/Business/Client/CreateClientRequest.cs — ALTERAR
- src/.../Application/Dtos/Request/Business/Client/UpdateClientRequest.cs — ALTERAR
- src/.../Application/Dtos/Request/Business/Client/BulkUploadClientItem.cs — ALTERAR
- src/.../Application/Dtos/Request/Business/ClientConsents/CreateClientConsentsRequest.cs — ALTERAR
- src/.../Application/Dtos/Response/Business/Client/ClientDetailResponse.cs — ALTERAR
- src/.../Application/Dtos/Response/Business/ClientConsents/ClientConsentsResponse.cs — ALTERAR
- src/.../Application/Dtos/Request/Business/OriginType/*.cs (3 arquivos) — REMOVER
- src/.../Application/Dtos/Response/Business/OriginType/*.cs (1 arquivo) — REMOVER

### Infra.Data (6 arquivos)
- src/.../Infra.Data/Context/GeritDbContext.cs — ALTERAR
- src/.../Infra.Data/Mappings/Billing/TenantMapping.cs — ALTERAR
- src/.../Infra.Data/Mappings/Business/ClientMapping.cs — ALTERAR
- src/.../Infra.Data/Mappings/Business/ClientConsentsMapping.cs — ALTERAR
- src/.../Infra.Data/Mappings/Business/OriginTypeMapping.cs — REMOVER
- src/.../Infra.Data/Repository/Business/ClientConsentRepository.cs — ALTERAR
- src/.../Infra.Data/Repository/Business/OriginTypeDataRepository.cs — REMOVER
- src/.../Infra.Data/Seeders/DatabaseSeeder.cs — ALTERAR

### Infra.IoC (1 arquivo)
- src/.../Infra.IoC/DependencyInjection.cs — ALTERAR

### Api (4 arquivos)
- src/.../Api/Endpoints/Business/OriginTypeEndpoint.cs — REMOVER/SUBSTITUIR
- src/.../Api/Validators/Business/Client/CreateClientRouteValidator.cs — ALTERAR
- src/.../Api/Validators/Business/Client/UpdateClientRouteValidator.cs — ALTERAR
- src/.../Api/Localization/*.json (todos os idiomas) — REVISAR

### Testes (3 arquivos + criação)
- 	ests/.../Domain/Entities/Business/ClientEntityTests.cs — ALTERAR
- 	ests/.../Application/Services/Business/ClientAppServiceTests.cs — ALTERAR
- 	ests/.../Infra/Data/Repository/Business/ClientRepositoryTests.cs — ALTERAR

---

## Apêndice B — SQLs de Referência

Os arquivos SQL que definem o schema esperado estão em:
- docs/sql/Create-Tables.sql — O schema alvo completo (1320 linhas)
- docs/sql/Initial_BackOffice_Idempotent.sql — Script de seed com dados iniciais
- docs/sql/Exclude-Tebles.sql — Script de exclusão (referencia as tabelas antigas)
- docs/sql/trabalhos.sql — Script de consulta para verificação

---

*Fim do relatório.*
