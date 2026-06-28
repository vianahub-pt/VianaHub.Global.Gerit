# Módulo 3 — Migrar `Clients` de `OriginType` para `AcquisitionSourceTypeId`

## Prompt para enviar ao agente Kanban-Coordinator

Você é o agente `kanban-coordinator` do repositório `VianaHub.Global.Gerit`.

Orquestre o Módulo 3 da migração da aplicação backend para migrar o fluxo de `Clients`.

### Objetivo do módulo

Substituir o uso de `OriginType`/enum/campo antigo no fluxo de clientes por `AcquisitionSourceTypeId`, conforme a nova tabela `dbo.Clients`.

### Contexto mínimo

A tabela `dbo.Clients` agora deve usar:

- `TenantId`
- `AcquisitionSourceTypeId`
- `ClientType`
- `UrlImage`
- `Note`
- auditoria
- controle de ativo/deletado

A origem de aquisição do cliente deve vir de `dbo.AcquisitionSourceTypes`.

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

1. Criar ou atualizar card/issue do Módulo 3.
2. Acionar `Developer-Senior` se houver muitos impactos em domínio e queries.
3. Acionar `Developer-Pleno` se o relatório do Módulo 0 mostrar impacto localizado.
4. Acionar `QA` depois da implementação.
5. Não passar ao Developer o histórico completo dos módulos anteriores; informar apenas que `AcquisitionSourceTypes` já deve existir como catálogo global.

### Handoff objetivo para Developer

```text
Migre o fluxo de Clients para substituir OriginType por AcquisitionSourceTypeId.

Domain:
- atualizar ClientEntity;
- remover/substituir propriedade OriginType no fluxo de Clients;
- adicionar AcquisitionSourceTypeId;
- adicionar navigation property para AcquisitionSourceTypeEntity se o padrão usar navegação;
- atualizar construtores;
- atualizar método Update;
- atualizar validações de domínio.

Infra.Data:
- atualizar ClientMapping;
- remover mapeamento de OriginType no Client;
- mapear AcquisitionSourceTypeId como obrigatório;
- configurar FK para AcquisitionSourceTypes;
- atualizar ClientRepository;
- ajustar includes para retornar origem quando necessário;
- ajustar busca por origem para usar AcquisitionSourceType.Code ou AcquisitionSourceType.Name.

Application:
- atualizar requests de Client;
- remover originType/originType antigo dos requests;
- adicionar acquisitionSourceTypeId;
- atualizar ClientResponse e ClientDetailResponse;
- retornar AcquisitionSourceTypeId;
- retornar AcquisitionSourceTypeCode/Name se esse for o padrão;
- atualizar MappingProfile;
- atualizar ClientAppService;
- validar AcquisitionSourceTypeId existente, ativo e não deletado.

Api:
- atualizar ClientEndpoint;
- atualizar ClientRouteValidator;
- atualizar validators de request;
- atualizar exemplos JSON.

Busca global:
- OriginType
- originType
- Origin
- origin

Não remover OriginTypeEntity/enum globalmente se ainda forem usados por outros módulos.
Neste módulo, remover apenas o uso antigo dentro de Clients.

Testes:
- não cria Client sem AcquisitionSourceTypeId;
- não cria Client com AcquisitionSourceTypeId inválido;
- cria Client com AcquisitionSourceTypeId válido;
- detalhe/listagem retornam origem de aquisição correta;
- busca por origem usa Code/Name do catálogo.

Executar:
- dotnet restore
- dotnet build
- dotnet test
```

### Handoff objetivo para QA

```text
Valide o Módulo 3.

Verifique:
- Client não depende mais de OriginType enum;
- Client exige AcquisitionSourceTypeId;
- Client rejeita AcquisitionSourceTypeId inexistente/inativo/deletado;
- criação, edição, detalhe e listagem funcionam;
- busca por origem usa Code/Name do catálogo;
- contratos HTTP não exigem originType antigo;
- não houve remoção indevida de OriginTypeEntity se ela ainda for usada fora de Clients;
- dotnet build passa;
- dotnet test passa.

Não reexecute validação completa de Tenants ou catálogos, exceto se a falha ocorrer diretamente no fluxo de Client.
```

### Critérios de aceite

- `ClientEntity` está compatível com `dbo.Clients`.
- `Client` usa `AcquisitionSourceTypeId`.
- Contratos HTTP foram atualizados.
- Não há persistência de `OriginType` no fluxo de `Clients`.
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
