# Shared Kanban Flow — Gerit API

Este documento define o fluxo Kanban compartilhado para os agentes de IA do projeto **VianaHub.Global.Gerit** (backend .NET 8).

Toda e qualquer comunicação com o usuário e também as issues, comentários e relatórios do GitHub Projects sempre serão em **português do Brasil**.

---

# Regra de Automação Contínua

O fluxo deve ser **contínuo e fluido**, sem intervenção humana entre as etapas operacionais dos agentes.

A intervenção humana deve acontecer apenas nos seguintes momentos:

1. Validar o resultado final quando o QA aprovar.
2. Revisar o PR.
3. Aprovar o PR.
4. Fazer o merge do PR para a branch de destino definida no fluxo do projeto.

Os agentes não devem pedir confirmação para:

- criar ou refinar issue;
- mover card entre colunas do Kanban;
- fazer assign;
- criar branch;
- implementar;
- executar build e testes;
- commitar alterações;
- fazer push da branch;
- criar PR;
- comentar na issue;
- mover card para `For Tests`;
- invocar QA;
- mover card para `In Test`;
- reprovar e devolver para `In Progress`;
- encaminhar correção para o Developer adequado;
- revalidar após correção;
- mover card para `For Deploy` quando aprovado.

O fluxo só deve parar antes do PR quando existir bloqueio real, como:

- requisito de negócio ausente;
- critério de aceite ambíguo;
- dependência externa não resolvida;
- contrato de API inexistente ou incompatível;
- erro técnico impeditivo que o agente não consiga resolver;
- risco de segurança ou perda de dados que exija decisão humana.

Mesmo nesses casos, o agente deve registrar claramente o bloqueio, o status atual, o responsável e a próxima ação esperada.

---

## Board Padrão

Board padrão para todos os repositórios e aplicações:

`https://github.com/users/vianahub-pt/projects/1`

O repositório deve ser resolvido dinamicamente pelo workspace atual.

Não hardcodar outro repositório quando o agente estiver executando dentro de um workspace diferente.

---

## Princípio de Continuidade do Fluxo

O fluxo deve avançar automaticamente entre PO, Kanban Coordinator, Developer e QA.

A intervenção humana só acontece na validação final, aprovação do PR e merge para a branch de destino definida no projeto.

Nenhum agente deve pedir autorização para executar atividades operacionais normais do fluxo, como mover cards, criar branch, implementar, commitar, criar PR, acionar QA, reprovar, devolver para correção ou revalidar.

---

# Regra Fundamental do Fluxo

## O Kanban Coordinator NUNCA desenvolve

O `kanban-coordinator` é **exclusivamente um orquestrador de fluxo**. Ele **NUNCA** deve criar branch, implementar código, executar validações técnicas, commitar, fazer push, criar PR ou mover card para `In Progress`, `For Tests` ou `In Test`.

Todo o desenvolvimento é responsabilidade **exclusiva** dos subagentes:
- `developer-junior` (baixa complexidade)
- `developer-pleno` (média complexidade)
- `developer-senior` (alta complexidade)

Toda a validação é responsabilidade **exclusiva** do subagente `qa`.

## Automação Total — Nenhuma Intervenção Humana

Todo o fluxo operacional entre os agentes é **100% automático, contínuo e fluido**, sem qualquer intervenção humana.

A **única** intervenção humana possível e inegociável em todo o ciclo de vida de uma issue é:

1. **Revisar** o PR final.
2. **Aprovar** o PR final.
3. **Fazer o merge** do PR final para a branch de destino.

Nenhum agente, em nenhuma circunstância, deve solicitar confirmação, autorização ou validação humana para qualquer atividade operacional. Todas as movimentações de cards, criações de branch, implementações, validações técnicas, commits, pushes, criação de PRs e acionamentos entre agentes devem ocorrer **automática e obrigatoriamente** sem intervenção humana.

O fluxo **só pode parar** para intervenção humana em caso de:
- Bloqueio real (requisito de negócio ausente, critério de aceite ambíguo, dependência externa não resolvida, contrato de API inexistente, erro técnico impeditivo, risco de segurança ou perda de dados).
- Regra anti-loop (mesmo bug reportado 2 vezes na mesma issue).

Mesmo nesses casos, o bloqueio deve ser registrado com clareza antes de qualquer ação.

## Proteção da Estrutura de Agentes — NUNCA Alterar

Nenhuma alteração no repositório — seja novo desenvolvimento, correção de bug/fix, instalação de dependência ou qualquer outra mudança — pode modificar, remover, renomear ou desativar a estrutura atual de agentes, instruções compartilhadas ou configurações do OpenCode.

Isso inclui, mas não se limita a:
- Arquivos em `.opencode/agents/` (todos os agentes)
- Arquivo `.opencode/instructions/kanban-flow.md`
- Arquivo `AGENTS.md` na raiz do projeto
- Arquivo `.opencode/opencode.json`

A **única** exceção é quando o usuário solicitar **expressamente e explicitamente** a alteração desses arquivos.

Qualquer agente que identificar uma tentativa de alteração desses arquivos sem solicitação explícita do usuário deve **recusar a alteração imediatamente** e informar o usuário sobre a proteção vigente.

---

## Agentes do Fluxo

Os agentes disponíveis no fluxo Kanban são:

- `kanban-coordinator`
- `po`
- `developer-junior`
- `developer-pleno`
- `developer-senior`
- `qa`

O `kanban-coordinator` é o agente principal de orquestração.

---

## Fluxo Oficial

O fluxo oficial é:

```text
PO -> Kanban Coordinator -> Developer Junior | Developer Pleno | Developer Senior -> QA
```

O fluxo de status no board é:

```text
Backlog -> To do -> In Progress -> For Tests -> In Test -> For Deploy -> Done
```

---

## Responsabilidades por Etapa

| Etapa | Status | Responsável | Ação |
|------|--------|-------------|------|
| Refinamento | Backlog | `po` | Criar/refinar issue, critérios de aceite, prioridade, severidade e complexidade sugerida |
| Pronto para desenvolvimento | To do | `kanban-coordinator` | Validar prontidão, classificar complexidade e escolher Developer adequado |
| Desenvolvimento | In Progress | `developer-junior`, `developer-pleno` ou `developer-senior` | Implementar, validar tecnicamente, criar PR e comentar issue |
| Pronto para QA | For Tests | Developer escolhido | Entregar para QA com handoff claro |
| Validação | In Test | `qa` | Validar critérios de aceite, build, testes, código e regressões |
| Aprovado para deploy/merge | For Deploy | `qa` / usuário | QA aprova e usuário revisa PR antes do merge |
| Concluído | Done | usuário / fluxo final do projeto | Item concluído após merge/deploy conforme decisão do usuário |

---

## Papel do PO

O `po` é responsável por transformar a demanda em uma issue clara e pronta para desenvolvimento.

O PO deve:

- entender a necessidade de negócio;
- criar ou refinar a issue;
- definir tipo da demanda;
- definir prioridade;
- definir severidade quando for bug;
- sugerir complexidade;
- indicar Developer provável apenas como sugestão;
- escrever critérios de aceite claros;
- documentar cenários de sucesso, insucesso e borda;
- documentar impacto nas camadas (API, Application, Domain, Infra);
- documentar contrato de API quando aplicável;
- garantir Definition of Ready;
- mover para `To do` quando estiver pronta;
- entregar para o `kanban-coordinator`.

O PO não deve acionar diretamente `developer-junior`, `developer-pleno` ou `developer-senior`.

---

## Papel do Kanban Coordinator

O `kanban-coordinator` é responsável por orquestrar o fluxo completo.

O coordinator deve:

- entender a demanda do usuário;
- acionar o PO quando a issue ainda não existir ou precisar de refinamento;
- receber do PO a issue pronta em `To do`;
- validar a complexidade sugerida pelo PO;
- decidir o Developer adequado;
- fazer handoff para o Developer selecionado;
- acompanhar a movimentação até `For Tests`;
- garantir handoff para QA;
- receber reprovações do QA;
- encaminhar correções para o Developer adequado;
- responder sempre com estado atual, próximo responsável e pendências.

---

## Papel dos Developers

Existem três agentes Developer, cada um com escopo diferente.

### Developer Junior

Usar `developer-junior` para tarefas simples, localizadas e de baixo risco.

Exemplos:
- Ajustes de strings de localização
- Correções em validadores FluentValidation existentes
- Pequenos bugs em endpoints existentes
- Ajustes em mapeamentos AutoMapper
- Correções em testes unitários existentes
- Alteração em uma única camada sem impacto arquitetural

Não usar `developer-junior` para:
- Novo endpoint/CRUD completo
- Nova entidade de domínio
- Nova integração com API externa
- Autenticação/autorização
- Alterações em `DependencyInjection.cs`
- Segurança
- Performance
- Refatoração
- Bug crítico ou alto

### Developer Pleno

Usar `developer-pleno` para tarefas intermediárias, funcionais e com padrão já existente.

Exemplos:
- Novo CRUD seguindo padrão existente
- Novos endpoints com `[EndpointMapper]`
- Novas entidades/Value Objects no domínio
- Novos serviços de aplicação
- Integração com APIs já existentes
- Validações FluentValidation
- Mapeamentos AutoMapper
- Testes unitários para novos recursos

Não usar `developer-pleno` para:
- Refatoração estrutural
- Arquitetura (DDD/Clean Architecture/Hexagonal)
- Autenticação/autorização
- Segurança
- Performance crítica
- Multi-tenant/RLS
- Mudanças em múltiplos domínios
- Bug crítico ou alto

### Developer Senior

Usar `developer-senior` para tarefas complexas, críticas, arquiteturais ou de alto risco.

Exemplos:
- Features complexas ou transversais
- Refatorações estruturais
- Bugs críticos ou altos
- Alterações em arquitetura (DDD, Clean Architecture, Hexagonal)
- Alterações em `DependencyInjection.cs`
- Integrações críticas com API
- Performance
- Segurança
- Autenticação/autorização JWT
- Multi-tenant/RLS
- Query complexas com EF Core
- Definição de novos padrões técnicos

---

## Papel do QA

O `qa` é responsável por validar implementações entregues em `For Tests`.

O QA deve:
- ler issue, PR e handoff do Developer;
- mover o card para `In Test`;
- validar critérios de aceite;
- executar validações técnicas (`dotnet build`, `dotnet test`);
- validar regressões;
- gerar relatório em `docs/reviews/`;
- comentar resultado na issue;
- mover para `For Deploy` quando aprovado;
- mover para `In Progress` quando reprovado;
- recomendar o Developer adequado para correção;
- devolver reprovações ao `kanban-coordinator`.

O QA não deve alterar código de produção.

---

## Movimentação dos Cards

### Backlog
Usar quando: issue criada, ainda há dependências, falta refinamento, critérios incompletos.

Responsável principal: `po`

### To do
Usar quando: Definition of Ready atendida, critérios claros, issue pronta para desenvolvimento.

Responsável: `kanban-coordinator`

### In Progress
Usar quando: Developer assumiu, branch criada, implementação em andamento.

Responsável: `developer-junior | developer-pleno | developer-senior`

### For Tests
Usar quando: Developer concluiu, PR criado, QA acionado.

Responsável: `qa`

### In Test
Usar quando: QA iniciou validação.

Responsável: `qa`

### For Deploy
Usar quando: QA aprovou, card pronto para revisão final/merge/deploy.

Responsável: `usuário`

### Done
Usar quando: item concluído, PR mergeado, deploy realizado.

---

## Critério de Roteamento por Complexidade

### Baixa Complexidade → `developer-junior`
Tarefa simples, escopo localizado, baixo risco, sem API nova, sem regra de negócio, sem impacto arquitetural.

### Média Complexidade → `developer-pleno`
Tarefa funcional intermediária, CRUD, formulário, grid, integração com API existente, impacto previsível.

### Alta Complexidade → `developer-senior`
Bug crítico/alto, arquitetura, refatoração, segurança, autenticação, performance, multi-tenant, impacto em múltiplos domínios.

### Regra de Decisão
Em caso de dúvida:
```text
Junior vs Pleno -> escolher Pleno
Pleno vs Senior -> escolher Senior
```

---

## Reprovação pelo QA

Se o QA reprovar:
1. QA comenta a issue com detalhes.
2. QA gera relatório em `docs/reviews/`.
3. QA move o card para `In Progress`.
4. QA recomenda o Developer adequado para correção.
5. QA envia handoff de reprovação para o `kanban-coordinator`.
6. `kanban-coordinator` deve encaminhar a correção ao Developer recomendado.

### Roteamento de Correção após QA

| Tipo de problema | Developer recomendado |
|------------------|----------------------|
| String/validação simples, visual, endpoint isolado | `developer-junior` |
| Regra funcional, serviço, CRUD, endpoint, teste | `developer-pleno` |
| Arquitetura, segurança, auth, multi-tenant, performance, regressão complexa | `developer-senior` |

---

## Regra Anti-loop

Se o mesmo bug for reportado 2 vezes na mesma issue:
1. Não insistir em correção automática.
2. Escalar para o usuário e `kanban-coordinator`.
3. Apresentar histórico das tentativas.
4. Recomendar decisão.

---

## Regra Anti-Ambiguidade de Issues (MULTI-REPO)

Este board gerencia issues de **múltiplos repositórios**. Números de issue são únicos **por repositório**, não globalmente.

**Regras obrigatórias para TODOS os agentes:**

1. **Sempre use `--repo`** em comandos `gh issue`, `gh pr`, `gh project`.
2. **Nunca refira uma issue apenas pelo número** (`#92`). Use sempre `{owner}/{repo}#{numero}` (ex: `vianahub-pt/VianaHub.Global.Gerit#92`).
3. **Resolva o repositório dinamicamente** do workspace atual via `git remote get-url origin`, NUNCA hardcode.
4. **Handoffs devem incluir o link completo** da issue (`https://github.com/{owner}/{repo}/issues/{n}`) e o repositório explícito.

---

## Handoff Padrão entre Agentes

Todo handoff deve conter:
- número da issue (formato: `{owner}/{repo}#{numero}`)
- link da issue (URL completa)
- repositório da issue (ex: `vianahub-pt/VianaHub.Global.Gerit`)
- status atual
- responsável atual
- próximo responsável
- tipo da demanda
- prioridade
- severidade (quando aplicável)
- complexidade
- motivo da classificação
- critérios de aceite
- camadas/arquivos impactados
- próxima ação esperada

---

## Critério de Saída Padrão

Ao responder ao usuário, informar sempre que possível o estado atual do card, próximo responsável, o que já foi feito e o que falta.
