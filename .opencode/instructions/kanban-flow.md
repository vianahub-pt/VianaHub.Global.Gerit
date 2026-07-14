# Shared Kanban Flow — Gerit API

Este documento define o fluxo Kanban compartilhado para os agentes de IA do projeto **VianaHub.Global.Gerit** (backend .NET 8).

Toda comunicação com o usuário, issues, comentários e relatórios do GitHub Projects serão em **português do Brasil**.

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

# Fluxo Oficial

```
Usuário solicita demanda
       ↓
Kanban Coordinator recebe a solicitação
       ↓
PO analisa, escreve Task em BDD, define classificação/complexidade
       ↓
Kanban Coordinator recebe a Task
       ↓
Cria issue → Adiciona ao board → Backlog
       ↓
Move card para To do
       ↓
Valida classificação e invoca Developer Junior/Pleno/Senior
       ↓
Developer implementa: pull develop → branch → código → build → test → commit → push → PR
       ↓
Kanban Coordinator move card para For Tests e invoca QA
       ↓
QA testa (build + test + critérios de aceite)
       ↓
  ├── ✅ Aprovado
  │      ↓
  │   Coordinator move para For Deploy
  │      ↓
  │   Usuário revisa, aprova e faz merge do PR
  │      ↓
  │   Coordinator move para Done
  │
  └── ❌ Reprovado
         ↓
      Coordinator move para In Progress
         ↓
      Coordinator envia handoff de correção para o mesmo Developer
```

---

# Responsabilidades por Agente

| Agente | Responsabilidade |
|--------|-----------------|
| **Kanban Coordinator** | Criar/mover cards no board, invocar agentes, enviar handoffs compactos |
| **PO** | Analisar demanda, escrever Task em BDD, definir classificação/complexidade |
| **Developer Junior** | Implementar tarefas de baixa complexidade |
| **Developer Pleno** | Implementar tarefas de média complexidade |
| **Developer Senior** | Implementar tarefas de alta complexidade |
| **QA** | Validar implementações, testar critérios de aceite, reportar resultado |

---

# Handoff Padrão

Todos os handoffs são **compactos**, contendo apenas:
- Issue (formato `owner/repo#numero`)
- Task (descrição objetiva)
- Critérios de Aceite (BDD)
- Instruções Técnicas (branch, camadas, commit, PR)
- Verificação Obrigatória (build, test)

O Kanban Coordinator usa dois templates de handoff:
1. **Handoff de Desenvolvimento** → para Developer Junior/Pleno/Senior
2. **Handoff de Validação** → para QA

---

# Classificação de Complexidade

| Complexidade | Agente | Critério |
|-------------|--------|----------|
| **Baixa** | `developer-junior` | Tarefa simples, localizada, sem impacto arquitetural |
| **Média** | `developer-pleno` | CRUD, endpoints, serviços, integrações existentes |
| **Alta** | `developer-senior` | Refatoração, arquitetura, segurança, multi-tenant, bugs críticos |

Regra de decisão: `Junior vs Pleno → Pleno`, `Pleno vs Senior → Senior`.

---

# Reprovação pelo QA

Se o QA reprovar:
1. Kanban Coordinator move o card para `In Progress`.
2. Kanban Coordinator envia novo Handoff de correção para o mesmo Developer.
3. Developer corrige e sinaliza pronto.
4. Kanban Coordinator move para `For Tests` e invoca QA novamente.

---

# Regra Anti-loop

Se o mesmo bug for reportado 2 vezes na mesma issue:
1. Não insistir em correção automática.
2. Escalar para o usuário com histórico das tentativas.

---

# Proteção da Estrutura de Agentes

Nenhuma alteração no repositório pode modificar, remover, renomear ou desativar a estrutura atual de agentes, instruções compartilhadas ou configurações do OpenCode sem solicitação explícita do usuário.

Isso inclui:
- Arquivos em `.opencode/agents/` (todos os agentes)
- Arquivo `.opencode/instructions/kanban-flow.md`
- Arquivo `AGENTS.md` na raiz do projeto
- Arquivo `.opencode/opencode.json`

A única exceção é quando o usuário solicitar **expressamente e explicitamente** a alteração desses arquivos.
