# Análise Final (PO + Dev Senior): Camadas Api + Application vs Banco de Dados

**Data:** 2026-07-17  
**Tipo:** Documento Final Consolidado (PO + Dev Senior)  
**Escopo:** Camadas VianaHub.Global.Gerit.Api e VianaHub.Global.Gerit.Application  
**Arquivo SQL de referência:** docs/sql/Create-Tables.sql  

**Documentos de origem:**
1. docs/analise-api-application-vs-banco-2026-07-17.md (Developer Senior — análise técnica original)
2. docs/analise-po-api-application-vs-banco-2026-07-17.md (Product Owner — análise de negócio original)
3. docs/analise-alinhada-api-application-vs-banco-2026-07-17.md (Developer Senior — análise alinhada)

---

## 1. Resumo Executivo

### 1.1 Visão Final Consolidada

O projeto **VianaHub.Global.Gerit** está **80% coberto** nas camadas Api + Application. A arquitetura DDD + Clean Architecture + Hexagonal é sólida e consistente. Os gaps estão **concentrados no domínio Billing (41.7%)**, que é o domínio mais crítico para o negócio.

Esta análise final representa a **fonte única da verdade**, combinando:
- A **precisão técnica** do Developer Senior (mapeamento exato, dependências de FK, domínios existentes)
- A **perspectiva de negócio** do Product Owner (priorização por valor entregue, impacto no cliente)
- As **correções finais** do PO sobre a análise alinhada (4 ajustes identificados na comparação)

### 1.2 Score de Cobertura Final

| Métrica | Valor |
|---------|-------|
| **Total de tabelas no SQL** | 56 |
| **Recursos de negócio** (excluindo catálogos fixos, traduções e RefreshTokens) | **43** |
| **Recursos com CRUD completo** (App Service + Endpoint) | 36 |
| **Recursos com CRUD parcial** (Service sem Endpoint) | 1 (TenantContactPersons) |
| **Recursos SEM implementação** | 6 + 2 catalogáveis = **8** |
| **Score de cobertura de negócio** | **36/43 = 83.7%** |
| **Score ajustado (incluindo parciais)** | **37/43 = 86.0%** |

> **Nota metodológica:** Mantivemos a base de **43 recursos de negócio** (como nos dois documentos originais) para consistência. As 8 tabelas de tradução são geridas como sub-recursos dos pais e não contam como recursos independentes. PartyTypes e StatusDomains são catálogos seed fixos. RefreshTokens é gerido internamente. **DocumentTypes e SubscriptionPlanFileRules** foram RECLASSIFICADOS como recursos pendentes (não como catálogos), totalizando 8 recursos sem implementação.

### 1.3 Cobertura por Domínio

| Domínio | Cobertura | Avaliação |
|---------|-----------|-----------|
| **Identity** | 9/9 (100%) | ✅ Perfeito. Nenhum gap. |
| **Business** | 21/23 (91.3%) | ✅ Excelente. 2 gaps médios. |
| **Job** | 1/1 (100%) | ✅ Perfeito. |
| **Billing** | 5/12 (41.7%) | ⚠️ **Crítico.** 7 gaps, 4 de alta prioridade. |
| **Geral** | 36/43 (83.7%) | 🟡 Bom. Billing precisa de atenção urgente. |

### 1.4 Por que este documento é a fonte da verdade

1. **Três fontes comparadas:** Análise original do Dev Senior + análise original do PO + análise alinhada
2. **Divergências resolvidas:** Todos os 4 pontos de discordância entre PO e Dev Senior foram tratados
3. **Correções finais do PO:** 4 ajustes aplicados sobre a análise alinhada (documentados na Seção 4)
4. **Validação de negócio:** Cada prioridade foi justificada com valor de negócio, não apenas viabilidade técnica
5. **Plano de ação completo:** Roadmap com 4 fases, esforços, dependências e critérios de Go/No-Go

---

## 2. Metodologia

### 2.1 Como as Três Análises Foram Comparadas

1. **Leitura completa** dos três relatórios (618 + 551 + 444 linhas)
2. **Comparação ponto a ponto** para cada um dos 56 recursos (tabelas SQL)
3. **Identificação de concordâncias, divergências e achados exclusivos**
4. **Avaliação crítica** da análise alinhada do Dev Senior contra a posição original do PO
5. **Resolução final** de cada divergência com justificativa explícita
6. **Produção deste documento final** como fonte única da verdade

### 2.2 Critérios de Decisão

| Critério | Peso | Aplicação |
|----------|------|-----------|
| **Valor de negócio** | Alto | O recurso desbloqueia funcionalidades core (ex: faturação)? |
| **Bloqueante operacional** | Alto | Sem o recurso, o tenant consegue operar? |
| **Esforço técnico** | Médio | Qual o esforço estimado? Existem padrões reutilizáveis? |
| **Dependências** | Médio | Quantas entidades dependem do recurso? |
| **Risco arquitetural** | Baixo | Altera DI, interceptors, RLS ou camadas core? |
| **Roadmap do frontend** | Médio | O frontend precisa deste endpoint para alguma tela planejada? |

### 2.3 Quem Contribuiu com o Quê

| Contribuição | Responsável |
|-------------|-------------|
| Mapeamento técnico tabela → implementação | Developer Senior |
| Identificação de Domain Services existentes | Developer Senior |
| Namespaces e localizações de código | Developer Senior |
| Priorização por valor de negócio | Product Owner |
| Identificação de gaps de Domain Services | Product Owner |
| Questões de unique indexes e validações | Product Owner |
| Análise de riscos e métricas de sucesso | Dev Senior (alinhado) |
| Correções finais sobre a análise alinhada | **Product Owner (este documento)** |

---

## 3. Divergências Resolvidas (Análise Alinhada vs PO)

### 3.1 Divergências Onde o PO Concorda com a Correção do Dev Senior

| # | Recurso | Posição Original Dev Senior | Posição Final (Alinhada) | PO Concorda? |
|---|---------|----------------------------|--------------------------|--------------|
| 1 | **TenantFiscalData prioridade** | 🟡 Média | 🔴 Alta | ✅ Sim — prioridade correta |
| 2 | **StatusDefinitions prioridade** | 🟡 Média (Backlog) | 🔴 Alta | ✅ Sim — prioridade correta |
| 3 | **SubscriptionPlanFileRules** | 🏷️ Catálogo | 🟡 Recurso pendente (Média) | ✅ Sim — classificação correta |
| 4 | **DocumentTypes** | 🏷️ Catálogo sem CRUD | 🟡 Precisa de CRUD (Média) | ✅ Sim — classificação correta |

### 3.2 Divergências Onde o PO DISCORDA do Dev Senior Alinhado

| # | Ponto | Posição do Dev Senior Alinhado | Posição Final (PO) | Decisão Final | Justificativa |
|---|-------|-------------------------------|-------------------|---------------|---------------|
| 1 | **TenantFiscalData — conclusão** | Dividido em Fase 1 (iniciar) + Fase 2 (completar) | **Completar integralmente na Fase 1** | **Completo na Fase 1** | Recurso bloqueia faturação (consenso). Esforço total é 8h — cabe numa sprint. Dividir significa faturação bloqueada por +1 sprint. Inaceitável. |
| 2 | **Base de recursos** | 45 recursos de negócio | 43 recursos de negócio | **43 recursos** | Os dois documentos originais concordavam em 43. Mudar para 45 sem validação gera confusão e muda o score sem motivo real. |
| 3 | **Itens "ninguém viu" sem ação** | Listados mas sem tarefas no roadmap | Adicionar tarefas de investigação | **Adicionado na Fase 2** | Testes, frontend, migrações e performance são riscos reais. Precisam de dono. |
| 4 | **Dependência de frontend** | Ausente da tabela de priorização | Adicionar coluna "Frontend Dependency" | **Adicionado** | O roadmap do frontend deve influenciar a ordem. O próprio Dev Senior disse isso mas não aplicou. |

---

## 4. Correções Finais do PO sobre a Análise Alinhada

Este documento aplica **4 correções** sobre a análise alinhada do Dev Senior:

### Correção 1: TenantFiscalData completo na Fase 1

**Antes (alinhado):**
- Fase 1: Iniciar (Domain Service + validators — 4h)
- Fase 2: Completar (App Service + Endpoint + DTOs — 4h)

**Depois (final):**
- Fase 1: **Completar integralmente** (Domain Service + App Service + Endpoint + DTOs + Mapping + Route Validators — 8h)

**Impacto:** Fase 1 passa de 4-6h para 8-12h, mas **desbloqueia faturação** uma sprint inteira antes.

### Correção 2: Base de recursos mantida em 43

**Antes (alinhado):** 45 recursos de negócio → Score 80.0%
**Depois (final):** 43 recursos de negócio → Score 83.7%

**Justificativa:** As 8 tabelas de tradução são sub-recursos, não recursos independentes. PartyTypes e StatusDomains são catálogos seed. RefreshTokens é interno. DocumentTypes e SubscriptionPlanFileRules são agora classificados como "recursos pendentes" (não entram na base de 43, mas estão no plano de ação).

### Correção 3: Tarefas de investigação adicionadas

Adicionados na Fase 2 como itens de auditoria:
- Auditoria de testes de integração existentes
- Alinhamento com frontend sobre dependências
- Verificação de migrações EF Core vs schema SQL
- Auditoria de performance (N+1 queries)

### Correção 4: Coluna "Frontend Dependency" na priorização

Adicionada à tabela final de priorização para visibilidade do roadmap do frontend.

---

## 5. Análise Final por Domínio

### 5.1 Billing (Domínio Mais Crítico)

**Cobertura atual:** 5 de 12 recursos = **41.7%**

| # | Tabela SQL | Status | Prioridade Final | Esforço | Domain Service? | Frontend Dependency? |
|---|---|---|---|---|---|---|
| 1 | **TenantContactPersons** | ⚠️ Parcial | 🔴 **Crítica** | 🟢 1-2h | ✅ Sim | Alta — tela de dados do tenant |
| 2 | **TenantFiscalData** | ❌ 0% | 🔴 **Alta** | 🟡 8h | ❌ Não | Crítica — tela de faturação |
| 3 | **TenantAddresses** | ❌ 0% | 🔴 **Alta** | 🟡 4-8h | ❌ Não | Alta — tela de dados do tenant |
| 4 | **StatusDefinitions** | ❌ 0% | 🔴 **Alta** | 🔴 16-24h | ✅ Sim | Média — tela de configurações |
| 5 | **TenantDocuments** | ❌ 0% | 🟡 Média | 🟡 4-8h | ❌ Não | Média — tela de documentos |
| 6 | **SubscriptionPlanFileRules** | ❌ 0% | 🟡 Média | 🟡 4-8h | ❌ Não | Baixa — interno |
| 7 | **DocumentTypes** | ❌ 0% | 🟡 Média | 🟢 2-4h | ✅ Sim | Média — tela de catálogos |

### 5.2 Identity (100% Coberto)

**Cobertura atual:** 9 de 9 recursos = **100%**

Nenhuma ação necessária. ✅

### 5.3 Business (91.3% Coberto)

**Cobertura atual:** 21 de 23 recursos = **91.3%**

| # | Tabela SQL | Status | Prioridade Final | Esforço | Domain Service? |
|---|---|---|---|---|---|
| 1 | **ClientDocuments** | ❌ 0% | 🟡 Média | 🟡 4-8h | ❌ Não |
| 2 | **EmployeeFiscalData** | ❌ 0% | 🟡 Média | 🟡 4-8h | ❌ Não |

### 5.4 Job (100% Coberto)

**Cobertura atual:** 1 de 1 recurso = **100%**

Nenhuma ação necessária. ✅

### 5.5 Catálogos (Sem CRUD Independente)

| Tabela | Ação Recomendada |
|--------|-----------------|
| PartyTypes | 🟢 GET /v1/party-types (somente leitura) — Baixa prioridade |
| PartyTypeTranslations | 🏷️ Gerida com PartyTypes |
| AcquisitionSourceTypeTranslations | 🏷️ **AUDITORIA:** Confirmar se o CRUD do AcquisitionSourceType inclui traduções |
| AddressTypeTranslations | 🏷️ **AUDITORIA:** Confirmar se o CRUD do AddressType inclui traduções |
| FileTypeTranslations | 🏷️ **AUDITORIA:** Confirmar se o CRUD do FileType inclui traduções |
| StatusDomains | 🟢 GET /v1/status-domains (somente leitura) — Baixa prioridade |
| StatusDomainTranslations | 🏷️ Gerida com StatusDomains |
| StatusDefinitionTranslations | 🏷️ Gerida com StatusDefinitions (quando implementado) |
| SubscriptionPlanTranslations | 🏷️ Gerida com SubscriptionPlan (✅ já implementado) |
| DocumentTypeTranslations | 🏷️ Gerida com DocumentTypes (quando implementado) |

---

## 6. Tabela Final de Priorização

| # | Recurso | Prioridade | Esforço | Domain Service? | Dependências | Frontend Dependency | Justificativa |
|---|---------|-----------|---------|-----------------|--------------|---------------------|---------------|
| 1 | **TenantContactPersons Endpoint** | 🔴 **Crítica** | 🟢 1-2h | ✅ Sim | Nenhuma | Alta | Service + DTOs prontos. Maior impacto com menor esforço. |
| 2 | **TenantContact Route Validators** | 🔴 **Crítica** | 🟢 1h | — | #1 | Alta | Necessário junto com o endpoint. |
| 3 | **TenantFiscalData** (COMPLETO) | 🔴 **Alta** | 🟡 8h | ❌ Não | Nenhuma | **Crítica** | **Bloqueia faturação.** Core business. **Completo na Fase 1.** |
| 4 | **TenantAddresses** | 🔴 **Alta** | 🟡 4-8h | ❌ Não | AddressTypes (✅) | Alta | Informação base do tenant. Padrão EmployeeAddress/ClientAddress. |
| 5 | **StatusDefinitions** | 🔴 **Alta** | 🔴 16-24h | ✅ Sim | StatusDomains (🏷️), 6 entidades filhas | Média | Customização de workflows. Alta complexidade, alto valor. |
| 6 | **DocumentTypes** | 🟡 Média | 🟢 2-4h | ✅ Sim | Nenhuma | Média | Catálogo editável. Padrão AcquisitionSourceType/AddressType. |
| 7 | **TenantDocuments** | 🟡 Média | 🟡 4-8h | ❌ Não | DocumentTypes (#6), Tenants (✅) | Média | Documentos legais do tenant. |
| 8 | **ClientDocuments** | 🟡 Média | 🟡 4-8h | ❌ Não | DocumentTypes (#6), Clients (✅) | Média | Documentos de identificação de clientes. |
| 9 | **EmployeeFiscalData** | 🟡 Média | 🟡 4-8h | ❌ Não | Employees (✅) | Média | Dados fiscais para RH/processamento salarial. |
| 10 | **SubscriptionPlanFileRules** | 🟡 Média | 🟡 4-8h | ❌ Não | SubscriptionPlans (✅), FileTypes (✅) | Baixa | Regras de upload por plano. |
| 11 | **Auditoria: Traduções em catálogos** | 🟡 Média | 🟢 2h | — | AcquisitionSourceType, AddressType, FileType, Plan | — | Confirmar se DTOs incluem campos de tradução. |
| 12 | **Auditoria: Unique Indexes** | 🟡 Média | 🟢 2h | — | Todos os App Services | — | Verificar validação de indexes antes do insert. |
| 13 | **GET /v1/party-types** | 🟢 Baixa | 🟢 1h | ✅ Sim | Nenhuma | Baixa | Endpoint somente leitura. 2 registros fixos. |
| 14 | **GET /v1/status-domains** | 🟢 Baixa | 🟢 1h | ✅ Sim | Nenhuma | Baixa | Endpoint somente leitura. |
| 15 | **Corrigir naming Intervention → Visit** | 🟢 Baixa | 🟡 4-8h | — | Vários | Nenhuma | Refatoração de namespaces/DTOs. Consistência. |
| 16 | **Auditoria: Testes de Integração** | 🟡 Média | 🟡 4h | — | Vários | — | Verificar cobertura de testes para recursos existentes. |
| 17 | **Auditoria: Performance (N+1)** | 🟡 Média | 🟡 4h | — | App Services existentes | — | Verificar queries N+1 nos services atuais. |

---

## 7. Roadmap de Implementação

### Fase 1 — Crítica (Sprint Atual — ~10-14h)

**Objetivo:** Desbloquear funcionalidades core do tenant + faturação.

| # | Tarefa | Esforço | Critério de Go/No-Go |
|---|--------|---------|---------------------|
| 1 | Criar TenantContactEndpoint.cs | 1h | Endpoint responde GET/POST/PUT em /v1/tenants/{tenantId}/contacts |
| 2 | Criar CreateTenantContactRouteValidator.cs + UpdateTenantContactRouteValidator.cs | 1h | Validadores aprovam/rejeitam rotas corretamente |
| 3 | **Implementar TenantFiscalData COMPLETO**: Domain Service + Entity Validators + App Service + Endpoint + DTOs + Mapping + Route Validators | 8h | CRUD completo funcional em /v1/tenants/{tenantId}/fiscal-data. Unique indexes validados. |

**Entregável:** TenantContact + TenantFiscalData acessíveis via API. **Faturação desbloqueada.** 🎯

### Fase 2 — Alta Prioridade (Próximo Sprint — ~32-44h)

**Objetivo:** Elevar cobertura do Billing de 41.7% para ~75%.

| # | Tarefa | Esforço | Critério de Go/No-Go |
|---|--------|---------|---------------------|
| 4 | Implementar TenantAddresses: Domain Service + App Service + Endpoint | 8h | CRUD completo em /v1/tenants/{tenantId}/addresses |
| 5 | Implementar StatusDefinitions: App Service + Endpoint + DTOs | 20h | CRUD completo para status customizáveis. FK composta validada. |
| 6 | Implementar DocumentTypes: App Service + Endpoint | 4h | CRUD completo em /v1/document-types |
| 7 | **Auditoria:** Traduções em catálogos existentes | 2h | Relatório: DTOs incluem ou não traduções |
| 8 | **Auditoria:** Unique indexes na Application | 2h | Relatório: indexes cobertos vs descobertos |
| 9 | **Auditoria:** Testes de integração existentes | 4h | Relatório: cobertura de testes atual |
| 10 | **Auditoria:** Performance (N+1 queries) | 4h | Relatório: queries problemáticas encontradas |

**Entregável:** Billing com 9 de 12 recursos implementados. Relatórios de auditoria para decisões da Fase 3.

### Fase 3 — Média Prioridade (Backlog — ~20-28h)

**Objetivo:** Elevar cobertura geral para ~93%.

| # | Tarefa | Esforço |
|---|--------|---------|
| 11 | Implementar TenantDocuments | 8h |
| 12 | Implementar ClientDocuments | 8h |
| 13 | Implementar EmployeeFiscalData | 8h |
| 14 | Implementar SubscriptionPlanFileRules | 4h |

**Entregável:** Cobertura geral de 36/43 (83.7%) para ~40/43 (93.0%).

### Fase 4 — Baixa Prioridade (Oportunidade — ~8-14h)

**Objetivo:** Completar itens de baixo esforço e qualidade de código.

| # | Tarefa | Esforço |
|---|--------|---------|
| 15 | Criar GET /v1/party-types | 1h |
| 16 | Criar GET /v1/status-domains | 1h |
| 17 | Refatorar naming Intervention → Visit | 6h |
| 18 | Aplicar correções das auditorias (se necessário) | 4-8h |

**Entregável:** Cobertura máxima teórica de 42/43 (97.7%).

### Mapa de Progresso Esperado

| Fase | Cobertura Billing | Cobertura Geral | Tempo Estimado |
|------|-------------------|-----------------|----------------|
| **Atual** | 41.7% (5/12) | 83.7% (36/43) | — |
| **Após Fase 1** | 58.3% (7/12) | 88.4% (38/43) | 1 sprint |
| **Após Fase 2** | 83.3% (10/12) | 93.0% (40/43) | 2 sprints |
| **Após Fase 3** | 91.7% (11/12) | 95.3% (41/43) | 3 sprints |
| **Após Fase 4** | 91.7% (11/12) | 97.7% (42/43) | ~4 sprints |

> **Nota:** O máximo teórico é 42/43 (97.7%). PartyTypes (1 tabela) não requer CRUD — apenas endpoint GET opcional.

---

## 8. Decisões Pendentes

| # | Questão | Impacto | Recomendação | Responsável |
|---|---------|---------|--------------|-------------|
| 1 | **Os DTOs de catálogos (AcquisitionSourceType, AddressType, FileType, Plan) incluem campos de tradução?** | Alto — se não incluem, traduções nunca são populadas via API. | **Auditoria na Fase 2.** Se não incluem, adicionar. | Dev Senior |
| 2 | **Os unique indexes do banco estão todos validados na Application?** | Alto — risco de erros 500 em produção. | **Auditoria na Fase 2.** Priorizar indexes críticos. | Dev Senior |
| 3 | **Qual o propósito do TenantContactAppService_NEW.cs vazio?** | Baixo — não afeta funcionalidade. | **Investigação rápida.** Se abandonado, remover. | Dev Junior |
| 4 | **Qual a prioridade do frontend para estes recursos?** | Alto — influencia ordem da Fase 2. | **Alinhamento com frontend antes da Fase 2.** | PO + Frontend |
| 5 | **StatusDefinitions: CRUD completo ou só GET?** | Alto — impacta escopo da Fase 2. | **CRUD completo.** É diferencial competitivo. Se time sobrecarregado, começar com GET. | PO + Dev Senior |
| 6 | **Migrações EF Core refletem schema SQL mais recente?** | Médio — risco de divergência Code-First vs Database-First. | **Verificar na auditoria da Fase 2.** | Dev Senior |

---

## 9. Riscos e Mitigações

| # | Risco | Probabilidade | Impacto | Mitigação |
|---|-------|--------------|---------|-----------|
| 1 | **Domain Services ausentes** aumentam esforço (5 recursos: TenantFiscalData, TenantAddresses, TenantDocuments, ClientDocuments, EmployeeFiscalData) | Alta | Médio | Criar templates de Domain Service. Priorizar recursos com Domain Service existente (StatusDefinitions, DocumentTypes). |
| 2 | **Unique indexes não validados** causam erros 500 em produção | Média | Alto | Auditoria dedicada na Fase 2. |
| 3 | **StatusDefinitions** complexidade subestimada (FK composta multi-tenant) | Média | Alto | Alocar Dev Senior. Fazer spike técnico antes de comprometer prazo. |
| 4 | **Falta de alinhamento com frontend** causa retrabalho | Média | Médio | Sincronizar roadmap com frontend ANTES da Fase 2. |
| 5 | **TenantFiscalData Domain Service pode ser subestimado** (unique indexes complexos) | Média | Alto | Alocar Dev Pleno ou Senior. Validar indexes antes do merge. |
| 6 | **Traduções não suportadas nos DTOs existentes** requerem refatoração de endpoints em produção | Média | Alto | Auditoria na Fase 2. Se necessário, adicionar nas Fases 3/4. |

---

## 10. Glossário

| Termo | Significado |
|-------|-------------|
| **App Service** | Classe na camada Application que orquestra use-cases (ex: TenantContactAppService) |
| **Endpoint** | Arquivo na camada Api que expõe rotas HTTP (ex: TenantContactEndpoint.cs) |
| **DTO** | Data Transfer Object — Request/Response models |
| **Domain Service** | Classe na camada Domain com lógica de negócio pura |
| **CRUD completo** | App Service + Endpoint implementados para Create, Read, Update, Delete |
| **CRUD parcial** | App Service implementado, mas sem Endpoint (sem rota HTTP) |
| **RLS** | Row-Level Security — isolamento multi-tenant no banco |
| **Billing** | Domínio de faturação: Tenants, Subscriptions, Plans |
| **Identity** | Domínio de autenticação/autorização: Users, Roles, Permissions |
| **Business** | Domínio de negócio core: Clients, Employees, Visits |
| **Catálogo** | Tabela de lookup com valores pré-definidos (ex: PartyTypes) |
| **Tradução** | Tabela *Translations com valores localizados (pt-PT, en-US, es-ES) |
| **Unique Index** | Constraint do banco que impede valores duplicados (ex: UX_Vehicles_Tenant_Plate) |
| **CHECK Constraint** | Regra de validação no banco (ex: CK_Document_ExpiresAt) |

---

## 11. Conclusão

### 11.1 Estado Atual

O projeto **VianaHub.Global.Gerit** está **83.7% coberto** (36/43 recursos de negócio). A arquitetura é madura, os padrões são consistentes e a base de domínio é rica.

### 11.2 O Que Fazer Primeiro

1. **Fase 1 (esta sprint):** Criar TenantContact Endpoint + **Completar TenantFiscalData** → desbloqueia dados do tenant e faturação
2. **Fase 2 (próxima sprint):** TenantAddresses + StatusDefinitions + DocumentTypes + 4 auditorias técnicas
3. **Fase 3 (backlog):** Documentos (Tenant + Client) + Dados fiscais de funcionários + File Rules
4. **Fase 4 (oportunidade):** Endpoints lookup + Refatoração Intervention→Visit + Correções de auditoria

### 11.3 O Que NÃO Fazer

- ❌ **PartyTypes CRUD completo** — apenas 2 valores fixos (Individual, Organization)
- ❌ **StatusDomains CRUD completo** — conceitos fixos do sistema
- ❌ **RefreshTokens CRUD exposto** — gestão interna via Auth é a abordagem correta
- ❌ **Traduções como recursos independentes** — sempre geridas como sub-recursos dos pais

### 11.4 Validação Final

Este documento foi produzido pelo **Product Owner** após:
1. Leitura completa da análise original do Developer Senior
2. Produção de análise independente com perspectiva de negócio
3. Leitura completa da análise alinhada do Developer Senior
4. Comparação ponto a ponto entre as três fontes
5. Identificação e correção de 4 divergências residuais

**Este documento é a fonte da verdade para priorização de implementação.**

---

*Documento Final consolidado em 2026-07-17.*
*Product Owner — análise de negócio, correções finais e validação.*
*Developer Senior — análise técnica, riscos e métricas.*
*Nenhum código foi alterado durante esta análise.*
