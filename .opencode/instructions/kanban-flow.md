# Shared Kanban Flow — Gerit API

Este documento define as regras compartilhadas do fluxo Kanban para os agentes de IA do projeto **VianaHub.Global.Gerit** (backend .NET 8).

Toda comunicação com o usuário, issues, comentários e relatórios do GitHub Projects serão em **português do Brasil**.

---

> **ℹ️ O fluxo detalhado (completo, com todos os passos e comandos) está centralizado no ficheiro do agente orquestrador:**
> **`.opencode/agents/kanban-coordinator.md`** — fonte da verdade para o fluxo operacional.
>
> Este ficheiro contém apenas as **regras transversais** que todos os agentes devem respeitar.

---

# Regra de Automação Contínua

O fluxo é **100% contínuo e fluido** entre agentes, sem intervenção humana nas etapas operacionais.

A **única** intervenção humana permitida em todo o ciclo de vida de uma issue é:
1. Solicitar nova demanda.
2. Validar o resultado final quando o QA aprovar.
3. Revisar o PR.
4. Aprovar o PR.
5. Fazer o merge do PR para a branch de destino.

Nenhum agente deve pedir confirmação para atividades operacionais normais.

Todos os agentes, ao terminarem o seu trabalho, **devem informar o Kanban Coordinator** para que o fluxo prossiga.

---

# Regra Fundamental

## O Kanban Coordinator é o único orquestrador

O `kanban-coordinator` é o **único agente** que interage com o GitHub Projects board. Ele é responsável por:
- Criar Cards no board
- Mover Cards entre as colunas
- Invocar agentes especializados via task tool
- Enviar handoffs compactos com instruções objetivas

Ele **NUNCA** desenvolve código, escreve código, faz análises ou executa validações técnicas.

## Os demais agentes NÃO interagem com o board

- `po` — apenas analisa demandas e escreve Tasks em BDD
- `developer-junior`, `developer-pleno`, `developer-senior` — apenas implementam código
- `qa` — apenas valida implementações

Nenhum desses agentes precisa de informações sobre o GitHub Projects. Toda interação com o board é centralizada no `kanban-coordinator`.

---

# Responsabilidades por Agente

| Agente | Responsabilidade |
|--------|-----------------|
| **Kanban Coordinator** | Criar/mover cards no board, invocar agentes, enviar handoffs compactos |
| **PO** | Analisar demanda, escrever Task em BDD, definir classificação/complexidade |
| **Developer Junior** | Implementar tarefas de baixa complexidade (modelo leve: minimax-m3) |
| **Developer Pleno** | Implementar tarefas de média complexidade (modelo intermédio: qwen3.7-plus) |
| **Developer Senior** | Implementar tarefas de alta complexidade (modelo potente: deepseek-v4-pro) |
| **QA** | Validar implementações, testar critérios de aceite, anexar relatório como comment na issue |

---

# Regra Anti-loop

Se o mesmo bug for reportado 2 vezes na mesma issue:
1. Não insistir em correção automática.
2. Escalar para o usuário com histórico das tentativas.

---

# Reporte de Bugs nos Próprios Agentes

Se for detetado um bug no comportamento de qualquer agente (incluindo erros nos ficheiros de instrução, ambiguidades, mau funcionamento), o agente que detetou o problema deve:

1. Documentar o bug com detalhes (comportamento esperado vs. observado).
2. Informar o usuário de forma explícita, clara e objetiva.
3. Aguardar decisão do usuário sobre como proceder.

> ⛔ **Não alterar** ficheiros em `.opencode/agents/`, `.opencode/instructions/` ou `AGENTS.md` por iniciativa própria — a menos que o usuário autorize explicitamente.

---

# Proteção da Estrutura de Agentes

Nenhuma alteração no repositório pode modificar, remover, renomear ou desativar a estrutura atual de agentes, instruções compartilhadas ou configurações do OpenCode sem solicitação explícita do usuário.

Isso inclui:
- Arquivos em `.opencode/agents/` (todos os agentes)
- Arquivo `.opencode/instructions/kanban-flow.md`
- Arquivo `AGENTS.md` na raiz do projeto
- Arquivo `.opencode/opencode.json`

A única exceção é quando o usuário solicitar **expressamente e explicitamente** a alteração desses arquivos.


# Regra de Documentação do QA

O relatório de validação do QA deve ser **anexado como comment na issue** do GitHub, e NÃO salvo em ficheiro local.

**Porquê:** Manter toda a documentação (descrição, critérios de aceite, relatórios) centralizada na issue, facilitando o acompanhamento por PO, stakeholders e developers.

**Como:**
```bash
gh issue comment NUMERO --repo vianahub-pt/VianaHub.Global.Gerit --body "conteúdo do relatório"
```

> ⚠️ A pasta docs/reviews/ NÃO deve mais ser utilizada para relatórios de QA.

