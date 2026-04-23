Revise criticamente a refatoração do agregado Client.

Use subagents:
1. dotnet-tech-lead
2. qa-tester
3. software-architecture

Valide:
- consistência com DDD/Clean Architecture
- uso correto de EF Core
- possíveis problemas de eager loading excessivo
- riscos transacionais
- regressões em PF/PJ, contatos, endereços, consentimentos e hierarquia
- necessidade de testes adicionais

Entregue:
1. problemas encontrados
2. severidade
3. correção recomendada
4. backlog técnico pós-refatoração