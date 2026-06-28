# Módulo 4 — Migrar `ClientConsents` para `ConsentOriginTypeId`

## Prompt para enviar ao agente Kanban-Coordinator

Você é o agente `kanban-coordinator` do repositório `VianaHub.Global.Gerit`.

Orquestre o Módulo 4 da migração da aplicação backend para migrar o fluxo de consentimentos de cliente.

### Objetivo do módulo

Substituir a origem livre `Origin` em `ClientConsents` por `ConsentOriginTypeId`, conforme a nova tabela `dbo.ClientConsents`.

### Contexto mínimo

A tabela `dbo.ClientConsents` agora deve usar:

- `Id`
- `TenantId`
- `ClientId`
- `ConsentTypeId`
- `ConsentOriginTypeId`
- `Granted`
- `GrantedDate`
- `RevokedDate`
- `IpAddress`
- `UserAgent`
- auditoria
- controle de ativo/deletado

A origem do consentimento deve vir de `dbo.ConsentOriginTypes`.

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

1. Criar ou atualizar card/issue do Módulo 4.
2. Acionar `Developer-Pleno` ou `Developer-Senior`.
3. Acionar `QA` após a implementação.
4. Usar `PO` apenas se houver dúvida sobre regra de negócio de revogação.
5. Não pedir validação humana durante o fluxo.

### Handoff objetivo para Developer

```text
Migre ClientConsents para substituir Origin string por ConsentOriginTypeId.

Domain:
- atualizar ClientConsentsEntity;
- remover/substituir propriedade Origin string;
- adicionar ConsentOriginTypeId;
- adicionar navigation property ConsentOriginType se o padrão usar navegação;
- atualizar construtor;
- atualizar método Update;
- manter RevokedDate opcional;
- manter Granted e GrantedDate obrigatórios;
- não inventar nova regra de revogação se já existir regra no domínio.

Infra.Data:
- atualizar ClientConsentsMapping;
- remover mapeamento de Origin string;
- mapear ConsentOriginTypeId como obrigatório;
- configurar FK para ConsentOriginTypes;
- mapear IpAddress como varchar(45);
- mapear UserAgent como nvarchar(500);
- garantir que ClientConsents continua tenant-scoped.

Application:
- atualizar requests de ClientConsents;
- remover Origin/origin dos requests;
- adicionar ConsentOriginTypeId;
- atualizar responses;
- retornar ConsentOriginTypeId;
- retornar ConsentOriginTypeCode/Name se esse for o padrão;
- atualizar MappingProfile;
- atualizar ClientConsentsAppService;
- validar ClientId dentro do Tenant;
- validar ConsentTypeId existente, ativo e não deletado;
- validar ConsentOriginTypeId existente, ativo e não deletado.

Api:
- atualizar ClientConsentsEndpoint;
- atualizar ClientConsentsRouteValidator;
- atualizar validators de request;
- atualizar exemplos Swagger/Postman.

Testes:
- não cria consentimento sem ConsentOriginTypeId;
- não cria consentimento com ConsentOriginTypeId inválido;
- cria consentimento com ConsentOriginTypeId válido;
- response retorna origem do consentimento;
- não persiste Origin string.

Executar:
- dotnet restore
- dotnet build
- dotnet test
```

### Handoff objetivo para QA

```text
Valide o Módulo 4.

Verifique:
- ClientConsents não persiste Origin string;
- ClientConsents exige ConsentOriginTypeId;
- ConsentOriginTypeId inexistente/inativo/deletado é rejeitado;
- ClientId é validado dentro do Tenant;
- ConsentTypeId continua validado;
- IpAddress suporta IPv4/IPv6 com tamanho correto;
- UserAgent respeita limite de 500;
- criação, edição, listagem e detalhe funcionam;
- dotnet build passa;
- dotnet test passa.

Não reexecute validação completa de Clients ou catálogos, exceto se necessário para evidenciar falha direta neste módulo.
```

### Critérios de aceite

- `ClientConsentsEntity` usa `ConsentOriginTypeId`.
- `Origin` string não é mais persistida.
- Contratos HTTP foram atualizados.
- Validações de FK foram implementadas.
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
