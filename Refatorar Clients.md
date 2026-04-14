Quero refatorar o domínio de Clients deste repositório para um modelo orientado a agregado.

Antes de propor mudanças, inspecione a implementação atual de Clients no Domain, Application, Infra.Data e Endpoints, e compare o desenho atual com o desenho alvo orientado a agregado.

Use subagents especializados e espere todos concluírem:
- business-context
- software-architecture
- dotnet-tech-lead
- senior-dotnet-dev
- qa-tester

Contexto do domínio:
- O Gerit é uma plataforma multi-tenant ASP.NET Core 8.
- O módulo de Clients representa um CRM central.
- Client deve ser a raiz do agregado.
- ClientIndividuals, ClientCompanies, ClientAddresses, ClientContacts, ClientIndividualFiscalData, ClientCompanyFiscalData, ClientHierarchy e ClientConsents são partes internas do mesmo Client.
- As tabelas continuarão separadas no banco.
- A aplicação hoje precisa melhorar uso de Repository, Aggregate e EF Core.

Objetivo:
- Consolidar a modelagem em torno do agregado Client.
- Criar um único IClientRepository.
- Refatorar Domain, Application, Infra.Data e endpoints impactados.
- Preservar TenantId, RLS, build e testes.

Fluxo de trabalho:
1. Primeiro planeje e proponha o boundary do agregado.
2. Depois desenhe os contratos de Domain/Application/Infra.
3. Só então implemente.
4. Em seguida rode build e testes.
5. Por fim faça revisão crítica da própria implementação.

Instruções de implementação:
1. for /f "delims=" %i in ('type "Rodada 1.md"') do @set PROMPT=%PROMPT%%i codex "%PROMPT%"
2. for /f "delims=" %i in ('type "Rodada 2.md"') do @set PROMPT=%PROMPT%%i codex "%PROMPT%"
3. for /f "delims=" %i in ('type "Rodada 3.md"') do @set PROMPT=%PROMPT%%i codex "%PROMPT%"
4. for /f "delims=" %i in ('type "Rodada 4.md"') do @set PROMPT=%PROMPT%%i codex "%PROMPT%"


Formato de saída:
a) diagnóstico atual
b) desenho alvo
c) plano incremental
d) arquivos alterados
e) testes criados/ajustados
f) riscos pendentes

Responder sempre em pt-BR.

