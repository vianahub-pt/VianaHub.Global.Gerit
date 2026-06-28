# Módulo 1 — Catálogos globais `AcquisitionSourceTypes` e `ConsentOriginTypes`

## Prompt para enviar ao agente Kanban-Coordinator

Você é o agente `kanban-coordinator` do repositório `VianaHub.Global.Gerit`.

Orquestre o Módulo 1 da migração da aplicação backend para implementar suporte aos novos catálogos globais:

- `dbo.AcquisitionSourceTypes`
- `dbo.ConsentOriginTypes`

### Objetivo do módulo

Criar suporte completo no backend para os dois novos catálogos globais, incluindo domínio, EF Core, application services, DTOs, mappings, endpoints, validators, seeders e testes.

### Contexto mínimo

Essas tabelas são globais, não possuem `TenantId` e não devem receber filtro tenant-scoped.

Campos esperados:

- `Id`
- `Code`
- `Name`
- `Description`
- `IsActive`
- `IsDeleted`
- `CreatedBy`
- `CreatedAt`
- `ModifiedBy`
- `ModifiedAt`

Tamanhos esperados:

- `Code`: 50
- `Name`: 100
- `Description`: 300

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

1. Criar ou atualizar card/issue para este módulo.
2. Acionar `PO` apenas para validar o contrato funcional dos catálogos.
3. Acionar `Developer-Pleno` ou `Developer-Senior` para implementação.
4. Acionar `QA` somente depois da implementação estar em `For Tests`.
5. Em caso de reprovação do QA, devolver para `In Progress` com instruções objetivas e sem repetir contexto desnecessário.

### Handoff objetivo para PO

```text
Valide o escopo funcional dos novos catálogos globais:
- AcquisitionSourceTypes
- ConsentOriginTypes

Defina, de forma objetiva:
- operações esperadas na API;
- campos obrigatórios;
- mensagens de erro esperadas para Code/Name duplicados;
- comportamento de ativo/inativo;
- comportamento de soft delete;
- se a API deve expor CRUD completo ou apenas listagem/consulta.

Não reavalie arquitetura técnica.
Não escreva implementação.
Entregue apenas critérios funcionais de aceite.
```

### Handoff objetivo para Developer

```text
Implemente suporte backend completo aos catálogos globais:
- AcquisitionSourceTypes
- ConsentOriginTypes

Criar no Domain:
- AcquisitionSourceTypeEntity
- ConsentOriginTypeEntity

Criar na Infra.Data:
- AcquisitionSourceTypeMapping
- ConsentOriginTypeMapping
- DbSet no GeritDbContext

Regras:
- entidades globais;
- sem TenantId;
- sem filtro global por TenantId;
- Code obrigatório, max 50, único;
- Name obrigatório, max 100, único;
- Description max 300;
- IsActive e IsDeleted conforme padrão do projeto;
- auditoria conforme padrão do projeto.

Criar na Application:
- Requests;
- Responses;
- MappingProfiles;
- Interfaces;
- AppServices;
seguindo o padrão de catálogos já existente.

Criar na Api:
- Endpoints;
- RouteValidators;
- Validators;
seguindo o padrão da aplicação.

Criar seeders idempotentes:
AcquisitionSourceTypes:
- INSTAGRAM
- FACEBOOK
- LINKEDIN
- GOOGLE
- WHATSAPP
- FRIENDS
- EVENTS
- TV
- RADIO
- NEWSPAPER
- MAGAZINE
- OTHER

ConsentOriginTypes:
- WEB
- MOBILE
- PAPER
- API
- BACKOFFICE
- EMAIL
- SMS
- WHATSAPP
- CALLCENTER

Atualizar testes.

Executar:
- dotnet restore
- dotnet build
- dotnet test
```

### Handoff objetivo para QA

```text
Valide o Módulo 1.

Verifique:
- endpoints dos catálogos aparecem no Swagger, se aplicável;
- é possível criar/listar/consultar/atualizar/desativar/remover conforme escopo definido;
- Code duplicado é rejeitado;
- Name duplicado é rejeitado;
- entidades não exigem TenantId;
- entidades não recebem filtro tenant-scoped;
- seeders são idempotentes;
- dotnet build passa;
- dotnet test passa.

Não reexecute análise arquitetural já feita.
Reporte apenas aprovado ou reprovado com evidências objetivas.
```

### Critérios de aceite

- Existem entidades, mappings, DbSets, DTOs, services, endpoints e validators para os dois catálogos.
- Seeders são idempotentes.
- Catálogos não usam `TenantId`.
- Catálogos não são afetados por RLS/filtro tenant-scoped da aplicação.
- Build e testes passam.

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
