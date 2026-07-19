# Handoff Templates e Regras de Responsabilidade

Este documento define **obrigatoriamente** como cada agente deve ser invocado e o que cada um pode ou não fazer.

---

## Regra de Ouro

**TODO handoff enviado via task tool DEVE seguir o template definido neste documento.**
**NUNCA** enviar conteúdo bruto de ficheiros, código-fonte completo, ou instruções fora do formato.

---

## Template de Retorno do PO (para Criação de Issues)

Quando o PO retornar um plano de tasks, **cada task DEVE conter obrigatoriamente**:

```markdown
## Metadata

| Campo | Valor |
|-------|-------|
| **Agente:** | Developer Junior / Developer Pleno / Developer Senior |
| **Complexidade:** | Baixa / Média / Alta |
| **Fase:** | [número e nome da fase] |
| **Foco:** | Domain + Infra.Data / Infra.Data / Domain |
```

Esta Metadata é **obrigatória** e será incluída no corpo da Issue criada pelo Kanban Coordinator.

### Regra de Roteamento por Complexidade

| Complexidade | Agente | Critério |
|-------------|--------|----------|
| **Baixa** | `developer-junior` | Tarefa simples, escopo localizado, baixo risco, sem nova API, sem regra de negócio, sem impacto arquitetural |
| **Média** | `developer-pleno` | Tarefa funcional intermediária, CRUD, endpoints, serviços, integração com API existente, impacto previsível |
| **Alta** | `developer-senior` | Refatoração estrutural, arquitetura DDD, segurança, autenticação, multi-tenant, performance, bug crítico/alto, alterações em DependencyInjection.cs |

**Regra de decisão:** Em caso de dúvida:
- `Junior vs Pleno → escolher Pleno`
- `Pleno vs Senior → escolher Senior`

---

## Handoff para PO (Product Owner)

Ao invocar o PO, usar **exclusivamente** este formato:

```markdown
## Handoff — Análise de Demanda

### Contexto
[descrição concisa do que precisa ser analisado, 3-5 frases no máximo]

### Artefactos de Referência
- Caminho do ficheiro relevante: `docs/sql/Create-Tables.sql`
- [apenas referências, NUNCA o conteúdo completo]

### Pedido
[instrução objetiva do que o PO deve produzir]

### Restrições
- [restrições de negócio ou técnicas relevantes]
```

### Regras ABSOLUTAS do PO

1. ❌ **NUNCA** lê ficheiros do repositório por conta própria (a não ser que o handoff contenha o caminho)
2. ❌ **NUNCA** escreve, edita ou altera qualquer ficheiro de código
3. ❌ **NUNCA** acede à base de dados ou executa comandos git
4. ❌ **NUNCA** cria issues, PRs ou move cards no GitHub Projects
5. ✅ **APENAS** analisa a demanda e retorna Tasks em formato BDD
6. ✅ **APENAS** define classificação/complexidade e **agente sugerido**
7. ✅ **SEMPRE** retorna para o Kanban Coordinator quando terminar

### Formato de Retorno do PO (Obrigatório)

Cada task retornada pelo PO DEVE conter:
1. **Metadata** (Agente, Complexidade, Fase, Foco)
2. **Descrição** em formato BDD
3. **Critérios de Aceite** (Given/When/Then)
4. **Pontos de Atenção**

---

## Handoff para Developer

Seguir o template definido no `kanban-flow.md`:

```markdown
## Handoff — Desenvolvimento

**Issue:** `vianahub-pt/VianaHub.Global.Gerit#NUMERO`
**Task:** [descrição objetiva]

### Critérios de Aceite (BDD)
- [ ] Critério 1
- [ ] Critério 2

### Instruções Técnicas
- **Branch:** `tipo/issue-NUMERO-descricao`
- **Camadas afetadas:** [Api, Application, Domain, Infra.Data, Infra.IoC]
- **Commit:** `tipo(escopo): descrição — closes #NUMERO`
- **PR base:** `develop`

### Verificação Obrigatória
- [ ] `dotnet build` sem erros
- [ ] `dotnet test` passando 100%
```

---

## Handoff para QA

```markdown
## Handoff — Validação

**Issue:** `vianahub-pt/VianaHub.Global.Gerit#NUMERO`
**PR:** `https://github.com/.../pull/PR_NUMERO`

### Critérios de Aceite a Validar
- [ ] Critério 1
- [ ] Critério 2

### Verificação Obrigatória
- [ ] `dotnet build` sem erros
- [ ] `dotnet test` passando 100%
```

---

## Regras Gerais para o Kanban Coordinator

1. **NUNCA** incluir conteúdo completo de ficheiros no handoff — usar referências de caminho
2. **NUNCA** dar ao PO acesso a código-fonte operacional — ele só precisa de contexto de negócio
3. **SEMPRE** manter handoffs compactos (máximo 1-2 parágrafos de contexto + lista)
4. **SEMPRE** garantir que o agente invocado tem instruções claras sobre o que NÃO fazer
5. **SEMPRE** incluir Metadata (Agente + Complexidade) no corpo de cada Issue criada
6. Se um agente invocado reportar confusão ou desvio, **parar e reavaliar o handoff** antes de continuar
7. **SEMPRE** usar `--body-file` com ficheiro UTF-8 (sem BOM) para criar/atualizar issues — **NUNCA** passar body como string inline no PowerShell, pois corrói caracteres acentuados
8. Para escrever o body em ficheiro, usar `[System.IO.File]::WriteAllText($path, $content, [System.Text.UTF8Encoding]::new($false))` — isto garante UTF-8 sem BOM

---

## Consequências da Violação

- Handoffs fora do template causam confusão nos agentes subordinados
- PO com acesso a código-fonte pode desviar-se e começar a escrever código
- Conteúdo excessivo no prompt faz o LLM perder foco e ir por caminhos errados
- Issues sem Metadata (Agente/Complexidade) deixam o utilizador sem saber quem vai implementar
- Causa retrabalho, loops, e intervenção manual desnecessária

---

## Encoding ao Criar Issues

**Problema:** PowerShell corrompe caracteres acentuados (ã, ç, é, etc.) ao passar strings para comandos externos como `gh`.

**Solução:** Sempre usar ficheiros temporários com UTF-8 sem BOM:

```powershell
# CORRETO
$tempDir = "$env:TEMP\gh-issues"
$body = "conteúdo com acentos..."
[System.IO.File]::WriteAllText("$tempDir\169.md", $body, [System.Text.UTF8Encoding]::new($false))
gh issue edit 169 --repo vianahub-pt/VianaHub.Global.Gerit --body-file "$tempDir\169.md"

# ERRADO - PowerShell corrompe UTF-8
gh issue edit 169 --body "conteúdo com acentos..."
```

**Alternativa válida:** Usar texto simplificado sem acentos (ex: "Entao" em vez de "Então") quando o conteúdo é curto e compreensível.
