Implemente a refatoração do agregado Client neste repositório.

Use subagents:
1. senior-dotnet-dev para codificar
2. dotnet-tech-lead para revisar direção técnica
3. qa-tester para derivar e revisar testes

Regras:
- usar Client como Aggregate Root
- consolidar persistência em um único IClientRepository
- não criar repositórios por parte interna do agregado
- manter TenantId consistente em toda a árvore
- preservar contratos públicos quando possível
- documentar qualquer breaking change
- Não introduza breaking changes em endpoints públicos sem listar explicitamente contrato anterior, contrato novo e estratégia de compatibilidade.

Ao final:
- mostre os arquivos alterados
- explique as decisões
- rode build e testes
- liste pendências