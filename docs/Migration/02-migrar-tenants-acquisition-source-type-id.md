# Módulo 2 — Migrar `Tenants` para `AcquisitionSourceTypeId`

## Prompt para enviar ao agente Kanban-Coordinator

Você é o agente `kanban-coordinator` do repositório `VianaHub.Global.Gerit`.

Orquestre o Módulo 2 da migração da aplicação backend para adequar o fluxo de `Tenants` ao novo `Create-Tables.sql`.

### Objetivo do módulo

Atualizar domínio, contratos HTTP, validações, mappings, repositories e testes de `Tenants`, incluindo os campos:

- `TenantType`
- `AcquisitionSourceTypeId`

### Contexto mínimo

A tabela `dbo.Tenants` agora deve trabalhar com origem de aquisição por FK para `dbo.AcquisitionSourceTypes`.

Modelo esperado:

- `Id`
- `TenantType`
- `AcquisitionSourceTypeId`
- `Name`
- `Email`
- `Website`
- `UrlImage`
- `Note`
- `IsActive`
- `IsDeleted`
- `CreatedBy`
- `CreatedAt`
- `ModifiedBy`
- `ModifiedAt`

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

1. Criar ou atualizar card/issue do Módulo 2.
2. Acionar `Developer-Pleno` ou `Developer-Senior`.
3. Acionar `QA` após implementação.
4. Usar `PO` somente se houver dúvida objetiva sobre contrato funcional.
5. Evitar reabrir análise do Módulo 0 e implementação do Módulo 1, salvo se houver bloqueio real.

### Handoff objetivo para Developer

```text
Atualize o fluxo de Tenants para refletir o novo modelo da tabela dbo.Tenants.

Domain:
- localizar TenantEntity;
- adicionar/ajustar TenantType;
- adicionar AcquisitionSourceTypeId;
- adicionar navigation property para AcquisitionSourceTypeEntity se o padrão do projeto usar navegação;
- atualizar construtores, método Update e validações.

Infra.Data:
- atualizar TenantMapping;
- mapear TenantType como obrigatório;
- mapear AcquisitionSourceTypeId como obrigatório;
- configurar FK para AcquisitionSourceTypes;
- ajustar includes/queries quando necessário.

Application:
- atualizar requests de Tenant;
- atualizar responses de Tenant;
- atualizar MappingProfile;
- atualizar TenantAppService;
- validar se AcquisitionSourceTypeId existe, está ativo e não está deletado.

Api:
- atualizar TenantEndpoint;
- atualizar TenantRouteValidator;
- atualizar validators de request;
- atualizar exemplos Swagger/Postman se existirem.

Testes:
- não cria tenant sem AcquisitionSourceTypeId;
- não cria tenant com AcquisitionSourceTypeId inexistente/inativo/deletado;
- cria tenant com AcquisitionSourceTypeId válido;
- response retorna os novos campos.

Executar:
- dotnet restore
- dotnet build
- dotnet test
```

### Handoff objetivo para QA

```text
Valide o Módulo 2.

Verifique:
- Tenant exige AcquisitionSourceTypeId válido;
- Tenant não aceita origem inexistente, inativa ou deletada;
- TenantType está presente no request/response;
- response retorna AcquisitionSourceTypeId;
- se implementado, retorna também Code/Name da origem;
- mappings EF estão compatíveis;
- seeders/testes não quebram FK;
- dotnet build passa;
- dotnet test passa.

Não reexecute validações dos catálogos do Módulo 1, exceto se houver falha direta neste módulo.
```

### Critérios de aceite

- `TenantEntity` está compatível com a tabela nova.
- Requests e responses de Tenant foram atualizados.
- `AcquisitionSourceTypeId` é validado.
- Não há criação/edição de Tenant com FK inválida.
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
