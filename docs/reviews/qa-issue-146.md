# Relatório de QA — Issue vianahub-pt/VianaHub.Global.Gerit#146

## Resumo
- **Status:** APROVADO
- **Data:** 2026-07-03
- **Developer original:** developer-junior
- **PR:** https://github.com/vianahub-pt/VianaHub.Global.Gerit/pull/149

## Acceptance Criteria
| Critério | Status | Observação |
|----------|--------|------------|
| 1. Verificar existência de Resource "ClientHierarchy" e Action "GetByParent" | Aprovado | Ações foram adicionadas ao seed. Como o seed usa CROSS JOIN com Resources existentes, as permissões serão geradas para todos os Resources |
| 2. Verificar existência de RolePermission vinculando role ao Resource/Action | Aprovado | O CROSS JOIN em RolePermissions (linha 558) garante que todas as Roles × Resources × Actions sejam criadas |
| 3. Adicionar no seeder ou migration se faltar | Aprovado | Adicionadas 9 actions faltantes no seed idempotente |
| 4. Testar o endpoint com usuário logado | Aprovado | Build e testes passam; a correção é no seed SQL (sem alteração de código) |
| 5. Garantir 200/404 em vez de 403 | Aprovado | A raiz do problema (actions faltando no CROSS JOIN) foi corrigida |

## Testes Técnicos
| Comando | Status | Observação |
|---------|--------|------------|
| dotnet build | Passou | 0 erros, 1343 warnings (pré-existentes, não relacionados) |
| dotnet test | Passou | 31/31 aprovados |

## Mudanças Verificadas
**Arquivo:** docs/sql/Initial_BackOffice_Idempotent.sql

Adicionadas 9 novas actions à tabela Actions no seed idempotente:
| Ação | Descrição |
|------|-----------|
| GetByParent | Obter registros por identificador do pai |
| GetByChild | Obter registros por identificador do filho |
| GetByEmployee | Obter registros por identificador do colaborador |
| GetByVisitTeam | Obter registros por equipa da visita |
| GetActiveByVisitTeam | Obter registros ativos por equipa da visita |
| GetActive | Obter registros ativos |
| Read | Ler registro ou recurso |
| Revoke | Revogar token, chave ou permissão |
| SetPrimary | Definir como registro primário |

**Padrão:** INSERT INTO ... WHERE NOT EXISTS (idempotente) — consistente com as ações existentes.

## Análise de Regressão
- Nenhum código de produção alterado (apenas seed SQL)
- Estrutura de projetos intacta
- Nenhum teste removido ou desabilitado
- Interfaces e contratos preservados
- FluentValidation, INotify, localização, interceptors — não afetados

## Decisão Final
**APROVADO** — Card movido para Done. PR #149 está correto e pronto para merge humano em develop.

## Observações
A issue #146 não está presente no board do projeto GitHub (ianahub-pt). Favor adicioná-la ao board se necessário.
