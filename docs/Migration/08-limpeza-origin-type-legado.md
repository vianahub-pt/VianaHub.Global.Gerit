# Módulo 8 — Limpeza técnica de `OriginType` legado

## Prompt para enviar ao agente Kanban-Coordinator

Você é o agente `kanban-coordinator` do repositório `VianaHub.Global.Gerit`.

Orquestre o Módulo 8 da migração da aplicação backend para limpeza técnica do modelo legado de origem.

### Objetivo do módulo

Remover ou isolar usos legados de:

- `OriginType`
- `OriginTypeEntity`
- `OriginTypeMapping`
- `originType`
- `Origin`
- `origin`

sem quebrar módulos que ainda dependam legitimamente desses elementos.

### Contexto mínimo

Após a migração:

- `Clients` deve usar `AcquisitionSourceTypeId`.
- `ClientConsents` deve usar `ConsentOriginTypeId`.
- `OriginType` não deve ser usado como origem persistida de cliente.
- `Origin string` não deve ser usado como origem persistida de consentimento.

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

1. Criar ou atualizar card/issue do Módulo 8.
2. Acionar `Developer-Senior`, pois envolve limpeza segura e análise de dependência.
3. Acionar `QA` após a limpeza.
4. Não remover código sem confirmar se ainda é usado por outro módulo.
5. Não pedir aprovação humana durante a limpeza; aprovação humana fica apenas no PR.

### Handoff objetivo para Developer-Senior

```text
Faça a limpeza técnica do uso legado de origem.

Busca global por:
- OriginType
- OriginTypeEntity
- OriginTypeMapping
- OriginType enum
- originType
- Origin
- origin

Classifique cada uso:
- usado por Clients;
- usado por ClientConsents;
- usado por outro domínio;
- morto/não referenciado.

Para Clients:
- remover dependência de OriginType;
- garantir uso exclusivo de AcquisitionSourceTypeId.

Para ClientConsents:
- remover persistência de Origin string;
- garantir uso exclusivo de ConsentOriginTypeId.

Para OriginTypeEntity:
- se não for mais usada por nenhum módulo, remover entity, mapping, DbSet, repository, service, endpoint, DTOs e testes;
- se ainda for usada por outro módulo, manter e documentar que não deve ser usada por Clients ou ClientConsents.

Criar documentação:
docs/migration/origin-type-deprecation.md

A documentação deve conter:
- o que foi removido;
- o que foi mantido;
- por que foi mantido, se aplicável;
- regra futura: Clients usam AcquisitionSourceTypeId e ClientConsents usam ConsentOriginTypeId.

Executar:
- dotnet restore
- dotnet build
- dotnet test
```

### Handoff objetivo para QA

```text
Valide o Módulo 8.

Verifique:
- Client não depende mais de OriginType;
- ClientConsents não depende mais de Origin string;
- se OriginTypeEntity foi mantida, há justificativa documentada;
- se OriginTypeEntity foi removida, não há referência quebrada;
- documentação origin-type-deprecation.md foi criada;
- dotnet build passa;
- dotnet test passa.

Não reexecute testes completos dos módulos anteriores, apenas valide regressão relacionada à limpeza.
```

### Critérios de aceite

- Código legado foi removido ou documentado.
- `Clients` usa apenas `AcquisitionSourceTypeId`.
- `ClientConsents` usa apenas `ConsentOriginTypeId`.
- Não há referência quebrada.
- Documentação técnica foi criada.
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
