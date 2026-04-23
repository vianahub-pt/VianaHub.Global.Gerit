# AGENTS.md

## Idioma
- Responder sempre em pt-BR.
- Explicações, planos, revisões e comentários de código em pt-BR.
- Manter nomes de classes, métodos, propriedades, variáveis, interfaces e tabelas conforme o padrão do código-base, preferencialmente em inglês.
- Não misturar a resposta principal entre pt-BR e en-US.

## Contexto do sistema
- Este repositório implementa o Gerit, backend ASP.NET Core 8 multi-tenant.
- Arquitetura esperada: DDD + Clean Architecture + separação por camadas.
- O domínio de Clients deve ser tratado como agregado único.
- PF e PJ são especializações do mesmo Client.
- Contacts, Addresses, Consents, Hierarchy e FiscalData são partes do agregado Client.

## Objetivo desta refatoração
- Consolidar a persistência de Clients em um único repositório orientado a agregado.
- Remover modelagem de “sub-recursos independentes” quando forem partes internas de Client.
- Preservar multi-tenancy, RLS, contratos públicos e integridade transacional.

## Regras de arquitetura
- Não criar um repositório por tabela interna do agregado Client.
- O contrato principal de persistência deve ser IClientRepository.
- O Application Service deve orquestrar casos de uso do agregado, não de tabelas isoladas.
- Regras de negócio ficam no Domain.
- Persistência e tracking ficam na Infra.Data.
- Evitar lógica complexa em endpoints e Program.cs.

## Regras de EF Core
- Modelar relacionamentos explicitamente.
- Usar Include/ThenInclude apenas em consultas de agregado.
- Separar consultas de leitura pesada se necessário.
- Preferir atualizações controladas do agregado ao invés de anexar entidades soltas sem invariantes.
- Preservar TenantId em todo o agregado.

## Critérios de pronto
- Build verde.
- Testes automatizados relevantes.
- Casos PF e PJ cobertos.
- Casos com endereço, contato, consentimento e dados fiscais cobertos.
- Plano de migração/refatoração documentado.
- Riscos e impactos em endpoints explicitados.