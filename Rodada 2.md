Agora aprofunde o desenho técnico da refatoração de Clients.

Use subagents:
1. software-architecture
2. dotnet-tech-lead
3. senior-dotnet-dev

Peça que definam:
- Aggregate Root e entidades internas
- contratos de Domain e Application
- interface IClientRepository
- estratégia de EF Core para carregamento do agregado
- estratégia de update de coleções internas
- impacto em DTOs, AppServices e endpoints
- plano de rollout por etapas
- Diferencie claramente operações de escrita do agregado e consultas de leitura/projeção.

Exija saída em formato:
1. decisão arquitetural
2. contratos
3. impacto por camada
4. sequência de implementação
5. riscos