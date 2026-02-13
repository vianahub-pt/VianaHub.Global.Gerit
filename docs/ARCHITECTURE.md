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

## Convenções de Nomenclatura de Endpoints

### Arquivos de Endpoint
- **Nome do arquivo**: Deve ser singular terminando com `Endpoint`
  - ✅ Correto: `ActionEndpoint.cs`, `UserEndpoint.cs`, `AuthEndpoint.cs`
  - ❌ Incorreto: `ActionsEndpoint.cs`, `ActionEndpoints.cs`

### Classes de Endpoint
- **Nome da classe**: Deve corresponder ao nome do arquivo (singular + `Endpoint`)
  - ✅ Correto: `public static class ActionEndpoint`
  - ❌ Incorreto: `public static class ActionEndpoints`

### Métodos de Mapeamento
- **Nome do método**: Deve ser plural terminando com `Endpoints`
  - ✅ Correto: `MapActionEndpoints`, `MapUserEndpoints`, `MapAuthEndpoints`
  - ❌ Incorreto: `MapActionEndpoint`, `MapAction`

### Exemplo Completo
```csharp
// Arquivo: ActionEndpoint.cs
namespace VianaHub.Global.Gerit.Api.Endpoints.Identity;

[EndpointMapper]
public static class ActionEndpoint
{
    public static void MapActionEndpoints(this IEndpointRouteBuilder app)
    {
        // Mapeamento dos endpoints
    }
}
```

### Justificativa
Esta convenção segue o padrão do ASP.NET Core:
- A **classe** representa um mapeador único (singular)
- O **método** mapeia múltiplos endpoints (plural)
- Facilita a descoberta automática via reflexão
- Mantém consistência em toda a aplicação

## Tratamento de Exceções

- A aplicação NÃO deve usar `throw new Exception` (ou qualquer `throw new ...`) de forma explícita para controlar fluxo ou comunicar mensagens para o usuário. Sempre que for necessário enviar uma mensagem ao consumidor, utilize a interface `INotify` para agregar a(s) mensagem(ns) e o código HTTP apropriado.

- Evite `try/catch` generalistas nos serviços de Application ou Domain apenas para mapear exceções técnicas em respostas HTTP. O fluxo esperado é:
  - As classes da camada de Application executam validações e, quando necessário, adicionam notificações via `INotify` (sem lançar exceções explícitas para o usuário).
  - Exceções técnicas inesperadas (ex.: `DbUpdateException`, `SqlException`, `JsonException`) serão capturadas pelo `GlobalExceptionMiddleware`, que:
    - Gera um `ErrorId` único e registra o erro de forma estruturada nos logs (Serilog).
    - Constrói uma resposta amigável ao usuário usando mensagens localizadas e adiciona também uma notificação via `INotify` para garantir consistência no formato de resposta.

- Exceções de domínio (representando regras de negócio) devem ser comunicadas como notificações via `INotify` e não devem conter detalhes técnicos.

## Regras de Recursos e Status HTTP (Obrigatórias)

1. Validação de existência por ID (410 Gone):
   - Sempre que um endpoint receber um `id` na rota (em requisições `GET`, `POST`, `PATCH` ou `DELETE`), a existência e o estado do recurso (por exemplo: ativo/inativo) devem ser validados pela camada `VianaHub.Global.Gerit.Application/Services`.
   - Se o recurso com o `id` informado não existir ou estiver desativado, a aplicação deve responder com o status `410 Gone`.
   - A classe de serviço deve adicionar a notificação apropriada via `INotify` com a mensagem localizada e o código `410`.

2. Validação de unicidade na criação (409 Conflict):
   - Sempre que for criado um novo recurso, a camada de `VianaHub.Global.Gerit.Application/Services` deve verificar se já existe um recurso equivalente na base de dados (regra de unicidade definida pelo negócio).
   - Se o recurso já existir, a aplicação deve responder com `409 Conflict` e a validação deve ser sinalizada via `INotify` pela classe de serviço responsável.

Essas regras garantem que a lógica de decisão sobre códigos HTTP semânticos (410, 409, etc.) esteja centralizada na camada de Application, mantendo a API mais fina e consistente.

## Notificações e Respostas

- Use `INotify` para agregar mensagens e códigos de status (ex.: 400, 404, 409, 410) ao longo do fluxo de execução.
- Os endpoints devem retornar `notify.CustomResponse(...)` para padronizar o formato das respostas de erro/sucesso.

## Internacionalização e Mensagens (Obrigatório)

- A aplicação Gerit é Multi-Idioma.
- Todas as mensagens retornadas ao usuário (erros, validações, notificações, respostas de sucesso, etc.) devem obrigatoriamente utilizar chaves de tradução.
- É estritamente proibido o uso de mensagens hardcoded em qualquer camada da aplicação.
- Todas as mensagens devem:
  - Possuir uma chave de tradução padronizada.
  - Estar devidamente traduzidas nos arquivos de localização localizados em VianaHub.Global.Gerit.Api/Localization.
- Mensagens não traduzidas ou hardcoded serão consideradas violação arquitetural e não devem ser aprovadas em PRs.
- O uso de INotify deve sempre referenciar chaves de tradução, nunca textos literais.

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

