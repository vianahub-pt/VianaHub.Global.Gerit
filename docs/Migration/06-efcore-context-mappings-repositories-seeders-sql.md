# Módulo 6 — EF Core, Context, Mappings, Repositories, Seeders e SQL

## Prompt para enviar ao agente Kanban-Coordinator

Você é o agente `kanban-coordinator` do repositório `VianaHub.Global.Gerit`.

Orquestre o Módulo 6 da migração da aplicação backend para consolidar a compatibilidade física com o banco de dados.

### Objetivo do módulo

Revisar e ajustar a camada `Infra.Data` para garantir compatibilidade com o novo `Create-Tables.sql`.

### Contexto mínimo

As principais FKs novas/alteradas são:

- `Tenants.AcquisitionSourceTypeId -> AcquisitionSourceTypes.Id`
- `Clients.AcquisitionSourceTypeId -> AcquisitionSourceTypes.Id`
- `ClientConsents.ConsentOriginTypeId -> ConsentOriginTypes.Id`
- `ClientConsents.ConsentTypeId -> ConsentTypes.Id`
- `ClientConsents.ClientId/TenantId -> Clients.Id/TenantId`, conforme padrão real do banco/projeto

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

1. Criar ou atualizar card/issue do Módulo 6.
2. Acionar `Developer-Senior`, porque este módulo envolve EF Core, mappings, repositories, seeders e compatibilidade com SQL.
3. Acionar `QA` após implementação.
4. Não pedir validação humana antes do PR.
5. Se houver falha em migration/seeder, devolver ao Developer com erro objetivo.

### Handoff objetivo para Developer-Senior

```text
Revise e ajuste a camada Infra.Data para compatibilidade com o Create-Tables.sql atualizado.

GeritDbContext:
- adicionar DbSet para AcquisitionSourceTypeEntity;
- adicionar DbSet para ConsentOriginTypeEntity;
- confirmar DbSets de TenantEntity, ClientEntity, ClientConsentsEntity e ConsentTypeEntity;
- garantir que catálogos globais não tenham filtro por TenantId;
- garantir que entidades tenant-scoped continuam filtradas corretamente, se houver global query filter.

Mappings:
- revisar TenantMapping;
- revisar ClientMapping;
- revisar ClientConsentsMapping;
- revisar ConsentTypeMapping;
- criar/revisar AcquisitionSourceTypeMapping;
- criar/revisar ConsentOriginTypeMapping;
- conferir table names, max lengths, required, FKs, indexes e constraints.

Repositories:
- atualizar includes e joins necessários;
- substituir busca por OriginType enum em Client;
- substituir busca por Origin string em ClientConsents;
- usar Code/Name dos catálogos quando houver busca por origem.

Seeders:
- criar seeder idempotente para AcquisitionSourceTypes;
- criar seeder idempotente para ConsentOriginTypes;
- ajustar seeders de Tenant para preencher AcquisitionSourceTypeId;
- ajustar seeders de Client para preencher AcquisitionSourceTypeId;
- ajustar seeders de ClientConsents para preencher ConsentOriginTypeId;
- garantir ordem correta:
  1. catálogos globais;
  2. tenants;
  3. clients;
  4. consents.

SQL/migrations:
- se o projeto usa migrations EF, criar migration alinhada;
- se o projeto usa scripts SQL versionados, criar script incremental no padrão do projeto;
- não duplicar o Create-Tables.sql inteiro se o padrão for incremental;
- garantir que RLS continue apenas nas tabelas tenant-scoped.

Executar:
- dotnet restore
- dotnet build
- dotnet test
```

### Handoff objetivo para QA

```text
Valide o Módulo 6.

Verifique:
- mappings EF refletem o banco;
- DbSets novos existem;
- catálogos globais não recebem filtro por TenantId;
- entidades tenant-scoped continuam seguras por TenantId;
- seeders são idempotentes;
- seeders respeitam ordem correta;
- não ocorre erro de FK em seed/test;
- queries de Client não usam OriginType enum;
- queries de ClientConsents não usam Origin string;
- dotnet build passa;
- dotnet test passa.

Não reexecute validação funcional de endpoints, exceto se necessário para comprovar integração com Infra.Data.
```

### Critérios de aceite

- Mappings EF Core estão alinhados ao banco.
- Contexto possui DbSets corretos.
- Seeders não geram duplicidade nem conflito de FK.
- Queries foram atualizadas.
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
