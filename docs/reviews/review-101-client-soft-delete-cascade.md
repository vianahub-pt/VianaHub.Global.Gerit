# Revisão QA - Issue #101

**Data:** 07/06/2026
**Issue:** vianahub-pt/VianaHub.Global.Gerit#101
**PR:** vianahub-pt/VianaHub.Global.Gerit#102
**Responsável:** QA

## Resumo
Correção de soft delete em cascata para exclusão lógica de cliente.

## Validações

### Código
- [x] ClientEntity.Delete() cascateia para Addresses, Contacts, Consents, Hierarchies
- [x] ClientCompanyEntity.Delete() cascateia para FiscalData
- [x] ClientCompanyEntity expõe propriedade pública FiscalData (IReadOnlyCollection)
- [x] ClientRepository.GetByIdAsync() inclui todas as coleções necessárias (Addresses, Consents, ChildHierarchies, ParentHierarchies, Company.FiscalData)
- [x] Validador limpo de regras redundantes

### Build
- [x] Projetos de produção (`src/`): **0 erros**

### Regras de Negócio Atendidas
- [x] ClientType 1/2/3 (Pessoa Singular): afeta Clients + ClientIndividuals + ClientAddresses + ClientContacts + ClientFiscalData + ClientHierarchy + ClientConsents
- [x] ClientType 4/5 (Pessoa Jurídica): afeta Clients + ClientCompanies + ClientFiscalData + ClientAddresses + ClientContacts + ClientHierarchy + ClientConsents

### Testes
- [ ] Testes unitários pré-existentes com falhas não relacionadas a esta alteração

## Resultado
**APROVADO** ✅

## Próximo passo
Card movido para **For Deploy** — o usuário deve revisar e aprovar o PR #102 para fazer o merge.

## Evidências
- PR criado: https://github.com/vianahub-pt/VianaHub.Global.Gerit/pull/102
- Issue: https://github.com/vianahub-pt/VianaHub.Global.Gerit/issues/101
