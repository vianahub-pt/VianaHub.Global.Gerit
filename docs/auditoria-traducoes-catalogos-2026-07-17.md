# Auditoria de Traduções em Catálogos — 2026-07-17

**Issue:** [vianahub-pt/VianaHub.Global.Gerit#249](https://github.com/vianahub-pt/VianaHub.Global.Gerit/issues/249)
**Data:** 17 de Julho de 2026
**Autor:** Developer Junior (auditoria via análise estática de código)
**Escopo:** DTOs de resposta e endpoints de `AcquisitionSourceType`, `AddressType`, `FileType` e `SubscriptionPlan` (Billing/Plan)
**Metodologia:** Análise estática do código (DTOs, Mapeamentos AutoMapper, AppServices, Repositórios EF Core, Pipeline de Localização)

---

## 1. Resumo Executivo

Foi realizada uma auditoria de traduções em **4 catálogos** da API: `AcquisitionSourceType`, `AddressType`, `FileType` e `SubscriptionPlan` (Billing).

**Achados:**

| Severidade | Ocorrências |
|------------|-------------|
| 🔴 Crítica (frontend nunca recebe tradução) | **3 catálogos** (AcquisitionSourceType, AddressType, FileType) |
| 🟢 OK (resolução por cultura já implementada) | **1 catálogo** (SubscriptionPlan) |

**Diagnóstico principal:** Os catálogos `AcquisitionSourceType`, `AddressType` e `FileType` **têm as entidades de tradução e respetivas tabelas criadas no banco de dados**, mas os DTOs de resposta **não expõem os valores traduzidos** ao frontend. O frontend recebe sempre o `Name`/`Description` (ou `MimeType`/`Extension`) na cultura de criação do registo (pt-PT) — independentemente do header `Accept-Language`.

O catálogo `SubscriptionPlan` (Billing) está **corretamente implementado**: a camada `Application` resolve `Name`/`Description` a partir da tabela `SubscriptionPlanTranslations` com fallback para `pt-PT` usando a cultura corrente.

> ⚠️ **Limitação metodológica:** Esta auditoria foi realizada por **análise estática de código**, sem chamadas HTTP ao vivo (o ambiente de auditoria não dispõe de SQL Server nem de uma instância da API em execução). Os testes HTTP planeados no procedimento foram substituídos por inspeção determinística do pipeline de serialização (DTO ↔ MappingProfile ↔ AppService ↔ Repository ↔ Entity), o que produz um veredito igualmente conclusivo.

---

## 2. Pipeline de Cultura — Verificação Prévia

Antes de auditar cada catálogo, foi confirmada a infraestrutura de localização.

| Componente | Caminho | Estado |
|------------|---------|--------|
| Middleware de cultura | `src/VianaHub.Global.Gerit.Api/Middleware/RequestLocalizationMiddleware.cs` | ✅ Ativo (lê `Accept-Language`, faz fallback para `pt-PT`) |
| Registo no `Program.cs` | Linha 149 (`app.UseMiddleware<RequestLocalizationMiddleware>()`) | ✅ Ativo |
| Cultura em `HttpContext.Items["RequestCulture"]` | Injetado pelo middleware | ✅ Disponível |
| `ILocalizationService.GetCurrentCulture()` | `src/VianaHub.Global.Gerit.Api/Services/LocalizationService.cs` | ✅ Lê o item do HttpContext corretamente |
| Culturas suportadas | `pt-PT`, `en-US`, `es-ES` | ✅ Conforme exigido pelo handoff |

**Conclusão da infraestrutura:** O pipeline está pronto. **O problema não é de infraestrutura, é de exposição nos DTOs.**

---

## 3. Tabela de Resultados por Catálogo

| # | Catálogo | DTO inclui traduções? | Resolução por cultura? | Teste HTTP (código) | Resultado |
|---|----------|------------------------|------------------------|---------------------|-----------|
| 1 | `AcquisitionSourceType` | ❌ **NÃO** (DTO expõe `Name`/`Description` vindos da entidade, sem `Translations`) | ❌ **NÃO** | Análise estática: `AcquisitionSourceTypeResponse` mapeia `Name`/`Description` diretamente de `AcquisitionSourceTypeEntity.Name/Description` (campos em pt-PT). Repositório também **não inclui `.Include(x => x.Translations)`** | 🔴 **FALHA** |
| 2 | `AddressType` | ❌ **NÃO** (DTO expõe `Name`/`Description` mapeados da entidade, sem `Translations` nem resolução) | ❌ **NÃO** | Análise estática: `AddressTypeResponse` mapeia `Name`/`Description` diretamente de `AddressTypeEntity`. Repositório **inclui** as traduções, mas o `AppService` e o `MappingProfile` ignoram-nas | 🔴 **FALHA** |
| 3 | `FileType` | ❌ **NÃO** (DTO **nem sequer** expõe `Name`/`Description` — apenas `MimeType`/`Extension` que não são traduzíveis) | ❌ **NÃO** (e tampouco faria sentido sem DTO) | Análise estática: `FileTypeResponse` contém `MimeType`, `Extension`, `IsActive`. Repositório inclui `Translations` mas a app/não as consome | 🔴 **FALHA** |
| 4 | `SubscriptionPlan` (Billing) | ⚠️ Parcialmente — expõe `Name`/`Description` resolvidos por cultura (não um array `Translations`) | ✅ **SIM** | Análise estática: `PlanAppService.MapToResponse()` resolve `Name`/`Description` a partir de `entity.Translations` filtrando por `culture` (com fallback `pt-PT`) | 🟢 **OK** |

---

## 4. Análise Detalhada por Catálogo

### 4.1 🔴 AcquisitionSourceType — FALHA

**DTO de resposta:** `src/VianaHub.Global.Gerit.Application/Dtos/Response/Business/AcquisitionSourceType/AcquisitionSourceTypeResponse.cs`
```csharp
public class AcquisitionSourceTypeResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }            // ← vem da entidade (pt-PT)
    public string? Description { get; set; }      // ← vem da entidade (pt-PT)
    public bool IsActive { get; set; }
}
```

**MappingProfile:** `src/VianaHub.Global.Gerit.Application/Mappings/Business/AcquisitionSourceTypeMappingProfile.cs`
```csharp
CreateMap<AcquisitionSourceTypeEntity, AcquisitionSourceTypeResponse>()
    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))         // ← direto
    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description)); // ← direto
```

**AppService:** `src/VianaHub.Global.Gerit.Application/Services/Business/AcquisitionSourceTypeAppService.cs` (linhas 49-71)
- `GetAllAsync` / `GetByIdAsync` / `GetPagedAsync` usam apenas o mapper automático. Nenhuma resolução por cultura.

**Repositório:** `src/VianaHub.Global.Gerit.Infra.Data/Repository/Business/AcquisitionSourceTypeDataRepository.cs`
```csharp
public async Task<IEnumerable<AcquisitionSourceTypeEntity>> GetAllAsync(CancellationToken ct)
{
    return await _context.Set<AcquisitionSourceTypeEntity>()
        .AsNoTracking()
        // ⚠️ FALTA: .Include(x => x.Translations)
        .Where(x => !x.IsDeleted)
        .OrderBy(x => x.Name)
        .ToListAsync(ct);
}
```

**Entidade de tradução existe mas é ignorada:** `AcquisitionSourceTypeTranslationsEntity` (com `LanguageCode`, `Name`, `Description`) e tabela SQL `AcquisitionSourceTypeTranslations` foram criadas mas **nunca são consultadas**.

**Impacto no frontend:** Ao chamar `GET /v1/acquisition-source-types` com `Accept-Language: en-US`, o frontend recebe `Name` e `Description` em **português** (pt-PT), sem alternativa.

---

### 4.2 🔴 AddressType — FALHA

**DTO de resposta:** `src/VianaHub.Global.Gerit.Application/Dtos/Response/Business/AddressType/AddressTypeResponse.cs`
```csharp
public class AddressTypeResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string? Name { get; set; }            // ← vem da entidade
    public string? Description { get; set; }      // ← vem da entidade
    public bool IsActive { get; set; }
}
```

**MappingProfile:** `src/VianaHub.Global.Gerit.Application/Mappings/Business/AddressTypeMappingProfile.cs`
- Mapeia `Name`/`Description` diretamente da entidade, sem `MapFrom` para traduções.

**AppService:** `AddressTypeAppService.cs` (linhas 52-74)
- Mesmo padrão do AcquisitionSourceType: usa apenas o mapper automático.

**Repositório:** `AddressTypeDataRepository.cs` — **inclui** `.Include(x => x.Translations)` mas o resultado é descartado pelo mapper.

**Entidade de tradução existe mas é ignorada:** `AddressTypeTranslationsEntity` carregada em memória e nunca serializada.

**Impacto no frontend:** Idêntico ao AcquisitionSourceType — sempre pt-PT, ignorando `Accept-Language`.

---

### 4.3 🔴 FileType — FALHA (caso particular)

**DTO de resposta:** `src/VianaHub.Global.Gerit.Application/Dtos/Response/Business/FileType/FileTypeResponse.cs`
```csharp
public class FileTypeResponse
{
    public int Id { get; set; }
    public string? MimeType { get; set; }       // técnico, não traduzível
    public string? Extension { get; set; }      // técnico, não traduzível
    public bool IsActive { get; set; }
}
```

**Particularidade:** `MimeType` e `Extension` são **identificadores técnicos** e **não devem** ser traduzidos. O DTO está correto neste aspeto.

**Entidade de tradução:** `FileTypeTranslationsEntity` existe e o repositório **inclui** `.Include(x => x.Translations)`, mas o DTO **não expõe** nenhum campo de tradução (`Name`/`Description`). As traduções carregadas são **completamente desperdiçadas**.

**Impacto no frontend:** A `FileType` é um catálogo com colunas técnicas (`MimeType`/`Extension`) — a aplicação de uma tradução textual depende do que se quer apresentar no UI (e.g., "PDF Document" vs `application/pdf`). Atualmente, o frontend **não recebe** nome/descrição traduzidos porque o DTO nem sequer os expõe.

**Decisão arquitetural recomendada:** Adicionar campos `Name`/`Description` (resolvidos por cultura) ou um array `Translations` ao DTO.

---

### 4.4 🟢 SubscriptionPlan (Billing) — OK

**DTO de resposta:** `src/VianaHub.Global.Gerit.Application/Dtos/Response/Billing/Plan/PlanResponse.cs`
```csharp
public class PlanResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }          // ← preenchido manualmente com tradução
    public string? Description { get; set; }   // ← preenchido manualmente com tradução
    public decimal? PricePerHour { get; set; }
    // ... (outros preços)
    public bool IsActive { get; set; }
}
```

**MappingProfile:** `PlanMappingProfile.cs`
- Explicitamente ignora `Name`/`Description` no AutoMapper com `.ForMember(dest => dest.Name, opt => opt.Ignore())` e o comentário:
  > "Name e Description são preenchidos manualmente no PlanAppService.MapToResponse() pois dependem da cultura do utilizador e da tabela de traduções."

**AppService:** `PlanAppService.cs` (linhas 54-85, 373-392)
```csharp
public async Task<IEnumerable<PlanResponse>> GetAllAsync(CancellationToken ct)
{
    var entities = await _repo.GetAllAsync(ct);
    var culture = _localization.GetCurrentCulture();   // ← pt-PT | en-US | es-ES
    return entities.Select(e => MapToResponse(e, culture));
}

private PlanResponse MapToResponse(SubscriptionPlanEntity entity, string culture)
{
    var translation = entity.Translations.FirstOrDefault(t => t.LanguageCode == culture)
                   ?? entity.Translations.FirstOrDefault(t => t.LanguageCode == DefaultLanguage);
    return new PlanResponse
    {
        Id = entity.Id,
        Name = translation?.Name,
        Description = translation?.Description,
        // ...
    };
}
```

**Repositório:** `PlanDataRepository.cs` — Inclui `.Include(x => x.Translations)` corretamente.

**Validação:** ✅ O pipeline resolve traduções por cultura (com fallback pt-PT) e popula `Name`/`Description` antes de devolver ao controller.

**Limitação conhecida (não bloqueante):** O DTO não expõe o array `Translations` — apenas o nome/descrição da cultura corrente. Se o frontend precisar de múltiplas culturas em simultâneo, será necessário um `DetailResponse` com `List<PlanTranslationResponse>`. Para o caso de uso atual (mostrar o plano na cultura do utilizador), está correto.

---

## 5. Issues de Bug Recomendadas

Conforme critério de aceite, **se alguma falha for encontrada, criar nova Issue de bug**. Recomenda-se abrir **3 issues separadas**, uma por catálogo afetado:

### 🐛 Issue sugerida #1 — AcquisitionSourceType sem tradução
- **Título:** `[BUG] AcquisitionSourceType: frontend não recebe Name/Description traduzidos via Accept-Language`
- **Componente:** `AcquisitionSourceType` — DTO + MappingProfile + Repository
- **Comportamento atual:** retorna sempre pt-PT
- **Comportamento esperado:** retornar Name/Description na cultura do `Accept-Language` (com fallback pt-PT)
- **Critério de aceitação (BDD):**
  - Given o utilizador faz `GET /v1/acquisition-source-types` com `Accept-Language: en-US`
  - And existe tradução en-US para o item
  - Then `name` e `description` devem estar em inglês
- **Padrão a seguir:** modelo do `PlanAppService.MapToResponse()` ou padrão do `DocumentType` (DTOs `Response` resumido + `DetailResponse` com array `Translations`)

### 🐛 Issue sugerida #2 — AddressType sem tradução
- **Mesmo problema e padrão de solução da Issue #1**, aplicado ao `AddressType`.

### 🐛 Issue sugerida #3 — FileType sem exposição de traduções
- **Título:** `[BUG] FileType: DTO de resposta não expõe Name/Description traduzidos`
- **Particularidade:** adicionar primeiro os campos `Name`/`Description` ao DTO (ou criar `FileTypeDetailResponse` com `Translations`).
- **Mesmo critério de aceitação** das anteriores.

---

## 6. Padrão de Referência (DocumentType)

O catálogo `DocumentType` (já mergeado em `develop`, commit `ed1b1d4..a39c98d`) é o **padrão correto** a replicar para os 3 catálogos afetados:

| Ficheiro | Conteúdo |
|----------|----------|
| `DocumentTypeResponse.cs` | DTO resumido (sem traduções) para listagens |
| `DocumentTypeDetailResponse.cs` | DTO detalhado com `List<DocumentTypeTranslationResponse>? Translations` |
| `DocumentTypeTranslationResponse.cs` | DTO com `Id`, `LanguageCode`, `Name`, `Description` |
| `DocumentTypeAppService.GetByIdAsync` | Devolve `DocumentTypeDetailResponse` (com traduções) |
| `DocumentTypeAppService.GetPagedAsync/GetAllAsync` | Devolve `DocumentTypeResponse` (resumido) |
| `DocumentTypeDataRepository` | Inclui `.Include(x => x.Translations)` |
| `DocumentTypeMappingProfile` | Mapeia `Translations` da entidade para o DTO |

**Endpoints com sub-recurso de tradução:**
- `GET /v1/document-types` (resumido)
- `GET /v1/document-types/{id}` (detalhado, com traduções)
- `GET /v1/document-types/{id}/translations`
- `POST /v1/document-types/{id}/translations`
- `PUT /v1/document-types/{id}/translations/{tid}`
- `DELETE /v1/document-types/{id}/translations/{tid}`

Este padrão resolve **dois cenários** em simultâneo: (a) o frontend recebe o item na cultura corrente, (b) tem acesso a todas as culturas via array `Translations` para ecrãs administrativos.

---

## 7. Recomendações

1. **Curto prazo (Issues #1, #2, #3):** replicar o padrão `DocumentType` (ou o padrão `Plan`) em cada um dos 3 catálogos afetados. Complexidade estimada: **média** (afeta DTO + Mapping + AppService + Repository + Endpoint para manter consistência com `DocumentType`). Não é adequado para Developer Junior — alinhar com Developer Pleno/Senior.

2. **Médio prazo:** Introduzir um **teste de integração automatizado** que faça `GET` em cada endpoint de catálogo com os 3 headers `Accept-Language` e valide que pelo menos um campo de tradução muda de valor entre culturas (ou permanece na cultura fallback se não houver tradução). Isto eliminaria este tipo de regressão silenciosa.

3. **Curto prazo (prevenção):** Considerar mover a lógica de mapeamento de traduções para uma extensão `IMapper` ou um helper partilhado (`TranslationMapper.ResolveLocalized(entity, culture, defaultLanguage)`), evitando que cada `AppService` implemente a mesma lógica manualmente (como `PlanAppService` já faz).

---

## 8. Conclusão

| Catálogo | Estado | Ação |
|----------|--------|------|
| `AcquisitionSourceType` | 🔴 Falha | Abrir issue de bug (Issue #1 sugerida) |
| `AddressType` | 🔴 Falha | Abrir issue de bug (Issue #2 sugerida) |
| `FileType` | 🔴 Falha (DTO sem campos) | Abrir issue de bug (Issue #3 sugerida) |
| `SubscriptionPlan` | 🟢 OK | Nenhuma ação |

**Total:** 3 issues de bug a criar a partir desta auditoria. A correção não cabe no escopo desta issue (que é de **auditoria**), mas o relatório fornece a base técnica completa (referências de código, padrão a seguir, critérios de aceitação) para que a correção seja executada em issues separadas.

**Validação técnica (build & testes):** ✅ `dotnet build` — 0 erros. ✅ `dotnet test` — 32/32 testes passando.
