# Módulo 7 — Testes, Swagger, build e regressão

## Prompt para enviar ao agente Kanban-Coordinator

Você é o agente `kanban-coordinator` do repositório `VianaHub.Global.Gerit`.

Orquestre o Módulo 7 da migração da aplicação backend para validação final automatizada e regressão.

### Objetivo do módulo

Criar, ajustar e executar testes automatizados para validar a migração completa dos fluxos:

- `AcquisitionSourceTypes`
- `ConsentOriginTypes`
- `Tenants`
- `Clients`
- `ClientConsents`

### Contexto mínimo

Este módulo não deve reimplementar funcionalidades já entregues nos módulos anteriores. O foco é teste, regressão, documentação Swagger/examples e evidências.

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

1. Criar ou atualizar card/issue do Módulo 7.
2. Acionar `QA` para desenhar e executar a matriz de testes.
3. Acionar `Developer-Pleno` apenas para corrigir falhas objetivas encontradas.
4. Se QA reprovar, devolver para `In Progress` com a falha específica.
5. Não pedir validação humana durante correções.

### Handoff objetivo para QA

```text
Execute a validação final da migração.

Cobrir testes de:
- AcquisitionSourceTypes;
- ConsentOriginTypes;
- Tenants;
- Clients;
- ClientConsents;
- seeders;
- repositories;
- app services;
- endpoints;
- Swagger/examples, se aplicável.

Validar AcquisitionSourceTypes:
- cria com sucesso;
- rejeita Name duplicado;
- rejeita Name duplicado;
- lista conforme regra de ativo/inativo;
- executa soft delete conforme padrão.

Validar ConsentOriginTypes:
- cria com sucesso;
- rejeita Name duplicado;
- rejeita Name duplicado;
- lista conforme regra de ativo/inativo;
- executa soft delete conforme padrão.

Validar Tenant:
- não cria sem AcquisitionSourceTypeId;
- rejeita AcquisitionSourceTypeId inexistente/inativo/deletado;
- cria com AcquisitionSourceTypeId válido.

Validar Client:
- não cria sem AcquisitionSourceTypeId;
- rejeita AcquisitionSourceTypeId inexistente/inativo/deletado;
- cria com AcquisitionSourceTypeId válido;
- response retorna origem correta.

Validar ClientConsents:
- não cria sem ConsentOriginTypeId;
- rejeita ConsentOriginTypeId inexistente/inativo/deletado;
- cria com ConsentOriginTypeId válido;
- response retorna origem do consentimento.

Executar:
- dotnet restore
- dotnet build
- dotnet test

Gerar evidência objetiva da execução.
```

### Handoff objetivo para Developer, se QA reprovar

```text
Corrija apenas as falhas objetivas reportadas pelo QA no Módulo 7.

Não reimplemente módulos anteriores.
Não altere contratos aprovados sem necessidade.
Após corrigir, execute:
- dotnet restore
- dotnet build
- dotnet test

Devolva para QA com resumo dos arquivos corrigidos.
```

### Critérios de aceite

- Todos os testes passam.
- Swagger/examples estão coerentes.
- Não há falhas de FK.
- Não há dependência nova de `OriginType` para Client.
- Não há dependência nova de `Origin string` para ClientConsents.
- Evidência de QA foi registrada.
- PR está pronto para aprovação humana.

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
