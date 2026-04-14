Quero planejar a refatoração do domínio de Clients deste repositório.


Use subagents e espere todos concluírem antes de responder:
1. business-context
2. software-architecture
3. dotnet-tech-lead
4. qa-tester

Contexto obrigatório:
- O Gerit trata Clients como CRM central.
- Client deve ser agregado único.
- PF/PJ, fiscal data, contacts, addresses, hierarchy e consents são partes do mesmo Client.
- O banco permanecerá normalizado em múltiplas tabelas.
- A meta é consolidar em um único repositório orientado a agregado.
- Identifique explicitamente quais classes, interfaces e endpoints atuais violam o boundary do agregado Client.

Entregue:
a) boundary do agregado
b) invariantes
c) contrato sugerido de IClientRepository
d) plano incremental de refatoração
e) riscos e testes necessários

Responda em pt-BR.