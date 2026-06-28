# Módulo 5 — Endpoints, RouteValidators e contratos HTTP

## Prompt para enviar ao agente Kanban-Coordinator

Você é o agente `kanban-coordinator` do repositório `VianaHub.Global.Gerit`.

Orquestre o Módulo 5 da migração da aplicação backend para revisar os contratos HTTP afetados.

### Objetivo do módulo

Revisar e ajustar endpoints, route validators, request validators, requests, responses e documentação Swagger relacionados a:

- `AcquisitionSourceTypes`
- `ConsentOriginTypes`
- `Tenants`
- `Clients`
- `ClientConsents`

### Contexto mínimo

Os novos campos principais são:

- `acquisitionSourceTypeId`
- `consentOriginTypeId`

Os campos antigos não devem ser usados como campos persistidos:

- `originType`
- `origin`

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

1. Criar ou atualizar card/issue do Módulo 5.
2. Acionar `Developer-Pleno` para padronização dos contratos.
3. Acionar `QA` para validação de API/Swagger.
4. Acionar `PO` somente se houver dúvida sobre backward compatibility.
5. Não reexecutar implementação de domínio já feita em módulos anteriores, salvo correção pontual.

### Handoff objetivo para Developer

```text
Revise e ajuste os contratos HTTP impactados pela migração.

Verifique:
- src/VianaHub.Global.Gerit.Api/Endpoints
- src/VianaHub.Global.Gerit.Api/Validators
- src/VianaHub.Global.Gerit.Application/Dtos/Request
- src/VianaHub.Global.Gerit.Application/Dtos/Response

Atualizar endpoints de:
- Tenants
- Clients
- ClientConsents
- AcquisitionSourceTypes
- ConsentOriginTypes

Ajustar:
- Create
- Update
- GetById
- GetPaged/List
- Delete/SoftDelete
- Revoke, se existir para consentimentos
- GetByClient, se existir para consentimentos

Regras:
- usar acquisitionSourceTypeId em Tenants e Clients;
- usar consentOriginTypeId em ClientConsents;
- não aceitar origin/originType como campos persistidos;
- se houver compatibilidade temporária, retornar apenas campos derivados em response, como acquisitionSourceTypeName ou consentOriginTypeName;
- validar ids de rota;
- validar payloads obrigatórios;
- validar IpAddress e UserAgent nos limites corretos.

Atualizar Swagger/examples se existirem.

Executar:
- dotnet restore
- dotnet build
- dotnet test
```

### Handoff objetivo para QA

```text
Valide o Módulo 5 pelo contrato HTTP.

Verifique no Swagger ou testes de endpoint:
- Tenants expõe acquisitionSourceTypeId;
- Clients expõe acquisitionSourceTypeId;
- ClientConsents expõe consentOriginTypeId;
- originType antigo não é obrigatório;
- origin string antigo não é obrigatório;
- payload inválido retorna erro claro;
- ids de rota inválidos são rejeitados;
- endpoints dos catálogos funcionam conforme escopo;
- dotnet build passa;
- dotnet test passa.

Não reexecute testes internos de domínio se os testes de endpoint já cobrirem o contrato.
```

### Critérios de aceite

- Swagger reflete os novos contratos.
- Requests antigos não são aceitos como fonte persistida.
- Responses retornam os novos campos.
- Validators estão coerentes.
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
