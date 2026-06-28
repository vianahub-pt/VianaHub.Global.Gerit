# Módulo 0 — Varredura local completa e relatório de impacto

## Prompt para enviar ao agente Kanban-Coordinator

Você é o agente `kanban-coordinator` do repositório `VianaHub.Global.Gerit`.

Orquestre o Módulo 0 da migração da aplicação backend para adequação ao novo `Create-Tables.sql`.

### Objetivo do módulo

Fazer uma varredura local completa da aplicação backend na branch `develop`, entender a arquitetura e gerar um relatório técnico de impacto antes de qualquer alteração funcional.

### Contexto mínimo

O script `Create-Tables.sql` foi atualizado e contém alterações em tabelas como:

- `dbo.AcquisitionSourceTypes`
- `dbo.ConsentOriginTypes`
- `dbo.Tenants`
- `dbo.Clients`
- `dbo.ClientConsents`
- outras tabelas relacionadas

A aplicação backend está no repositório público:

`https://github.com/vianahub-pt/VianaHub.Global.Gerit`

A branch de trabalho é:

`develop`

## Regras obrigatórias para o Kanban-Coordinator

Antes de iniciar este módulo, respeite obrigatoriamente estas regras:

1. A única intervenção humana permitida em todo o fluxo será na aprovação do Pull Request e no merge. Fora isso, o processo deve ser contínuo, sem pedir aprovação humana para análise, decomposição, implementação, testes, correções, movimentação de cards ou reencaminhamento entre agentes.

2. O Kanban-Coordinator deve passar aos agentes especializados, como PO, Developer-Junior, Developer-Pleno, Developer-Senior e QA, somente o que cada agente precisa fazer de forma objetiva e clara. É expressamente proibido criar handoffs sobrecarregados com contexto desnecessário, repetir contexto já validado ou solicitar reexecução de validações já feitas por outro agente.

3. Cada handoff deve conter:
   - objetivo específico;
   - arquivos ou áreas prováveis de impacto;
   - critérios de aceite;
   - comando de validação esperado, quando aplicável;
   - saída esperada.

4. Cada handoff não deve conter:
   - histórico completo da demanda;
   - explicações extensas do banco inteiro;
   - instruções que pertencem a outro agente;
   - revalidações redundantes;
   - contexto que o agente não precisa para executar a tarefa.

5. O Kanban-Coordinator deve manter o fluxo Kanban contínuo:
   - criar ou atualizar card/issue do módulo;
   - mover para To do;
   - atribuir o agente correto;
   - mover para In Progress;
   - coordenar implementação;
   - mover para For Tests;
   - acionar QA;
   - se QA reprovar, devolver para In Progress com instrução objetiva;
   - se QA aprovar, preparar PR;
   - deixar apenas a aprovação do PR e o merge para intervenção humana.

### Orquestração esperada

1. Confirmar que o workspace está no repositório correto.
2. Confirmar que a branch atual é `develop`.
3. Criar ou atualizar um card/issue para este módulo.
4. Acionar o agente mais adequado para análise técnica. Preferencialmente:
   - `Developer-Senior`, para análise estrutural e impacto técnico;
   - `PO`, apenas se for necessário transformar impacto técnico em escopo funcional claro;
   - `QA`, apenas para definir estratégia inicial de testes, sem executar validação redundante.
5. Não acionar agentes desnecessários.

### Handoff objetivo para Developer-Senior

Passe somente esta instrução objetiva ao `Developer-Senior`:

```text
Faça uma varredura técnica local no backend da aplicação VianaHub.Global.Gerit, branch develop, comparando a implementação atual com o novo Create-Tables.sql.

Analise os projetos:
- src/VianaHub.Global.Gerit.Api
- src/VianaHub.Global.Gerit.Application
- src/VianaHub.Global.Gerit.Domain
- src/VianaHub.Global.Gerit.Infra.Data
- tests/VianaHub.Global.Gerit.Tests

Localize arquivos relacionados a:
- *Endpoint
- *RouteValidator
- *AppService
- *Request
- *Response
- *MappingProfile
- *Entity
- *DomainService
- *Validator
- *Context
- *Mapping
- *Repository

Procure usos de:
- OriginType
- OriginTypeEntity
- ClientEntity.OriginType
- ClientConsentsEntity.Origin
- TenantEntity
- ClientEntity
- ClientConsentsEntity
- ConsentTypeEntity
- AcquisitionSource
- ConsentOrigin

Gere o relatório:
docs/migration/create-tables-v2-impact-analysis.md

O relatório deve conter:
- arquitetura encontrada por camada;
- lista de arquivos impactados;
- campos atuais vs campos esperados pelo banco;
- riscos de breaking change nos contratos HTTP;
- ordem recomendada de implementação;
- lista de testes que precisam ser criados ou ajustados.

Não altere código funcional.
Não faça commit.
Não remova arquivos.
```

### Critérios de aceite

- O relatório `docs/migration/create-tables-v2-impact-analysis.md` foi criado.
- O relatório lista os arquivos impactados por camada.
- O relatório identifica os impactos em `Tenants`, `Clients`, `ClientConsents`, `AcquisitionSourceTypes` e `ConsentOriginTypes`.
- Nenhum código funcional foi alterado.
- Nenhuma migração ou refatoração foi implementada neste módulo.

## Saída esperada do Kanban-Coordinator

Ao final da orquestração deste módulo, entregar:

1. Link ou identificação do card/issue usado no fluxo.
2. Lista objetiva dos agentes acionados.
3. Resumo curto do que cada agente fez.
4. Lista dos arquivos alterados.
5. Resultado dos comandos:
   - `dotnet restore`
   - `dotnet build`
   - `dotnet test`
6. Evidência de QA.
7. Link do Pull Request criado ou instrução clara de que o PR já está pronto para aprovação humana.

## Observação

Não pedir confirmação humana durante a execução. A aprovação humana acontece somente no PR e no merge.
