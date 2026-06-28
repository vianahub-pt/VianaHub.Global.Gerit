# Prompts Kanban-Coordinator — Migração Gerit Create-Tables.sql

Este pacote contém um arquivo `.md` por módulo da migração da aplicação backend `VianaHub.Global.Gerit`.

## Ordem recomendada de execução

1. `00-varredura-local-relatorio-impacto.md`
2. `01-catalogos-globais-acquisition-consent-origin.md`
3. `02-migrar-tenants-acquisition-source-type-id.md`
4. `03-migrar-clients-origin-type-acquisition-source-type-id.md`
5. `04-migrar-client-consents-consent-origin-type-id.md`
6. `05-endpoints-routevalidators-contratos-http.md`
7. `06-efcore-context-mappings-repositories-seeders-sql.md`
8. `07-testes-swagger-build-regressao.md`
9. `08-limpeza-origin-type-legado.md`

## Regras globais reforçadas em todos os prompts

1. A única intervenção humana permitida em todo o fluxo será na aprovação do Pull Request e no merge.
2. O Kanban-Coordinator deve passar aos agentes especializados somente o que cada agente precisa fazer de forma objetiva e clara.
3. É expressamente proibido criar handoffs sobrecarregados com contexto desnecessário.
4. É expressamente proibido solicitar reexecução de validações já feitas por outro agente.
5. O fluxo deve ser contínuo: card/issue, To do, In Progress, For Tests, QA, correção se necessário, PR pronto para aprovação humana.

## Uso recomendado

Abra cada arquivo `.md`, copie o prompt completo e envie ao agente `kanban-coordinator` na ordem definida acima.
