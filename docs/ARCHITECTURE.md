# Arquitetura do Projeto Gerit

- Sempre responda os prompts enviado no chat em Português do Brasil.
- Não crie nenhum tipo de documentação do que foi feito, nenhum arquivo .md deve ser gerado/criado, a não ser que for explicitamente solicitado no prompt.
- Este documento descreve diretrizes arquiteturais e convenções que devem ser seguidas por todos os desenvolvedores do projeto Gerit.

> Observação: este arquivo deve ser mantido em UTF-8 (sem BOM preferencialmente) para preservar acentuação e caracteres especiais.

## Princípios Gerais


- Separação de responsabilidades: cada camada (API, Application, Domain, Infra) tem responsabilidades bem definidas.
- Código expressivo e testável: priorizar composição, injeção de dependência e interfaces para facilitar testes.
- Tratamento centralizado de erros: o pipeline HTTP contém um middleware responsável por capturar exceções não tratadas e retornar respostas amigáveis.
- Notificações amigáveis: use a interface `INotify` para acumular mensagens que serão exibidas ao consumidor; não exponha detalhes técnicos ao cliente em produção.
- Diretriz de idioma: todo o código (nomes de classes, métodos, variáveis, DTOs, mensagens internas de logs técnicas) deve ser escrito em inglês, porém os comentários no código devem ser escritos em português para facilitar a comunicação da equipe local.

## Estrutura de Layers

- API: roteamento, validação de entrada (binding), autenticação/autorização, mapping para objetos de aplicação e retorno de respostas HTTP.
- Application: orquestração de casos de uso, mapeamento de DTOs, chamadas para serviços de domínio e retorno de modelos para a API.
- Domain: regras de negócio, entidades ricas, validações e serviços de domínio. Deve conter pouca ou nenhuma dependência de infra-estrutura.
- Infra: implementações de repositórios, acesso a banco, mapeamentos ORM, integrações externas.

## Tratamento de Exceções

- Não coloque `try/catch` generalistas nos serviços de aplicação ou domain apenas para transformar exceções técnicas em respostas HTTP. Exceções técnicas (por exemplo `DbUpdateException`) devem ser lançadas e tratadas pelo middleware global, que:
  - Loga detalhes técnicos (incluindo stack trace e inner exception) de forma estruturada.
  - Constrói uma resposta amigável ao usuário usando `INotify` e `ErrorResponse`.
  - Em ambientes de desenvolvimento, pode incluir informações de debug adicionais; em produção, deve omitir detalhes sensíveis.

- Exceções de domínio (representando regras de negócio) podem ser expostas como notificações via `INotify` e não devem conter detalhes técnicos.

## Notificações e Respostas

- Use `INotify` para agregar mensagens e códigos de status (ex.: 400, 404, 409) ao longo do fluxo de execução.
- Os endpoints devem retornar `notify.CustomResponse(...)` para padronizar o formato das respostas de erro/sucesso.

## Logging

- Utilize logging estruturado (Serilog) em todos os níveis apropriados:
  - `Information` para eventos normais (criação/alteração de recursos).
  - `Warning` para situações anômalas que não impedem a execução.
  - `Error` para exceções que requerem investigação.
- O middleware global deve gerar um `ErrorId` único para correlacionar logs e retornar esse id ao usuário quando ocorrer um erro inesperado.

## Banco de Dados e Constrangimentos

- Validações de integridade (FK, UNIQUE) podem resultar em exceções do provedor (ex.: `DbUpdateException`) quando a persistência falha. O domínio pode tentar validar previamente, mas não é obrigado a capturar todas as condições de corrida — o middleware é o responsável por mapear essas exceções para mensagens amigáveis.

## Boas Práticas de Implementação

- Favor utilizar tipos imutáveis onde fizer sentido, e métodos `ValidateForCreateAsync` / `ValidateForUpdateAsync` nas entidades para centralizar validações.
- Mantenha o mapeamento DTO ↔ Entidade no Application (ou AutoMapper profiles) e não misture lógica de negócio com mapping.
- As mensagens retornadas ao consumidor devem ser curtas, em português e sem revelar detalhes internos (ex.: stack traces, SQL completo).

## Encerramento

Manter este documento atualizado é obrigatório. Antes de abrir PRs que mudem comportamentos transversais (logging, tratamento de exceções, contratos de API), verifique e, se necessário, atualize este documento.

