# Documento de Regras de Negócio — JWT Key Management

## 1. Introdução
Este documento descreve de forma detalhada as regras de negócio do módulo **JWT Key Management** no sistema IAM (VianaID).

O módulo **JwtKeys** é responsável pelo gerenciamento completo do ciclo de vida de chaves criptográficas utilizadas para assinatura e validação de tokens JWT (JSON Web Tokens) pela aplicação.

O módulo gerencia a geração, armazenamento, ativação, expiração, rotação automática e revogação de chaves, garantindo segurança robusta, rastreabilidade completa e continuidade de autenticação sem interrupção de serviço.

---

## 2. Objetivos do Módulo de JWT Key Management
- Gerenciar chaves criptográficas para assinatura e validação de tokens JWT por Tenant.
- Suportar rotação automática de chaves conforme políticas de segurança configuráveis.
- Garantir continuidade de serviço durante transições de chaves (período de sobreposição).
- Permitir revogação imediata de chaves comprometidas.
- Manter histórico completo de chaves para validação de tokens antigos ainda válidos.
- Fornecer auditoria e rastreabilidade de uso de chaves.
- Suportar isolamento multi-tenant rigoroso.
- Implementar proteção robusta de chaves privadas em repouso.

---

## 3. Estrutura Geral da JwtKey
Uma **JwtKey** contém:

- `Id`
- `TenantId`
- `KeyId`
- `PublicKey`
- `PrivateKeyEncrypted`
- `Algorithm`
- `KeySize`
- `KeyType`
- `RevokedReason`
- `UsageCount`
- `ActivatedAt`
- `ExpiresAt`
- `LastUsedAt`
- `NextRotationAt`
- `RevokedAt`
- `LastValidatedAt`
- `ValidationCount`
- `RotationPolicyDays`
- `OverlapPeriodDays`
- `MaxTokenLifetimeMinutes`
- `IsActive`
- `IsDeleted`
- `CreatedBy`
- `CreatedAt`
- `ModifiedBy`
- `ModifiedAt`

### 3.2 Regras do campo KeyId
- O `KeyId` é um GUID único que identifica publicamente a chave
- É usado no header JWT como claim `kid` (Key ID)
- Deve ser único em toda a base de dados (constraint UNIQUE)
- Gerado automaticamente pelo sistema usando `Guid.NewGuid()`
- **Nunca** deve ser fornecido pelo usuário
- É imutável após criação

### 3.3 Escopo Multi-tenant
- Toda JwtKey pertence exatamente a um Tenant específico
- O isolamento é garantido através do `TenantId`
- Row-Level Security (RLS) garante isolamento de acesso
- Apenas **uma chave ativa** pode existir por `TenantId`

### 3.4 Proteção de Chave Privada
- A chave privada **NUNCA** é armazenada em texto plano
- O campo `PrivateKeyEncrypted` contém a chave privada criptografada com AES-256
- A chave de criptografia deve ser armazenada de forma segura (Azure Key Vault, AWS KMS, ou similar)
- A chave privada **NUNCA** deve ser retornada em respostas de API
- A chave privada **NUNCA** deve aparecer em logs
- Apenas o módulo de autenticação/emissão de tokens tem acesso à chave privada descriptografada

---

## 4. Regras de Negócio por Operação

### 4.1 Geração Inicial de Chaves
**Contexto:** Executado automaticamente ao provisionar um novo Tenant

**Regras:**
- Ao provisionar um novo Tenant, caso **não exista nenhuma chave registrada** para o `TenantId`, o sistema deve:
  1. Gerar um par de chaves assimétricas RSA (2048 bits ou superior)
  2. Gerar o `KeyId` (GUID único)
  4. Criptografar a chave privada usando AES-256
  5. Persistir a chave pública no formato PEM
  6. Persistir a chave privada criptografada
  7. Marcar a chave como **ativa** (`IsActive = true`)
  8. Definir `Algorithm = "RS256"` (padrão, configurável)
  9. Definir `KeyType = "RSA"`
  10. Definir `KeySize = 2048`
  11. Definir `ActivatedAt = GETDATE()`
  12. Definir `ExpiresAt` baseado em `RotationPolicyDays` (padrão: 90 dias após ativação)
  13. Definir `NextRotationAt` = `ActivatedAt` + `RotationPolicyDays` - `OverlapPeriodDays`
  14. Definir `RotationPolicyDays = 90` (padrão, configurável)
  15. Definir `OverlapPeriodDays = 7` (padrão, configurável)
  16. Definir `MaxTokenLifetimeMinutes = 60` (padrão, configurável)
  18. Definir `CreatedBy` com ID do usuário ou sistema que provisionou

**Validações:**
- O Tenant deve existir e estar ativo
- Não deve existir outra chave ativa para o mesmo `TenantId`

**Políticas de segurança:**
- `RotationPolicyDays` deve estar entre 30 e 365 dias (constraint CHECK)
- `OverlapPeriodDays` deve estar entre 1 e 30 dias (constraint CHECK)
- `MaxTokenLifetimeMinutes` deve estar entre 5 e 1440 minutos (constraint CHECK)
- `OverlapPeriodDays` deve ser menor que `RotationPolicyDays`

---

### 4.2 Emissão de Token JWT
**Contexto:** Executado ao gerar access tokens durante login ou renovação.

**Regras:**
- Apenas a **chave privada ativa** pode ser utilizada para assinar tokens JWT
- O sistema deve buscar a chave ativa para o `TenantId`:
  ```sql
  WHERE TenantId = @TenantId 
    AND IsActive = 1 
    AND IsDeleted = 0 
    AND RevokedAt IS NULL
  ```
- Descriptografar a chave privada em memória (nunca persistir descriptografada)
- O token JWT deve conter o `kid` (Key ID) no header:
  ```json
  {
    "kid": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    "alg": "RS256",
    "typ": "JWT"
  }
  ```
- Assinar o token usando a chave privada descriptografada
- Limpar a chave privada da memória imediatamente após uso
- Incrementar `UsageCount` da chave (assíncrono, não bloquear emissão)
- Atualizar `LastUsedAt = GETDATE()` (assíncrono, não bloquear emissão)

**Validações:**
- Deve existir exatamente uma chave ativa para o Tenant
- Se não existir chave ativa, retornar erro 500 (Internal Server Error) e alertar equipe técnica
- A chave não pode estar expirada (`ExpiresAt > GETDATE()`)
- A chave não pode estar revogada (`RevokedAt IS NULL`)

**Telemetria:**
- Registrar uso da chave em logs estruturados (sem expor a chave)
- Registrar evento de auditoria para uso de chave
- Monitorar falhas na obtenção de chave ativa

---

### 4.3 Validação de Token JWT
**Contexto:** Executado em cada requisição autenticada para validar assinatura do token.

**Regras:**
- Extrair o `kid` (Key ID) do header do JWT
- Buscar a chave pública correspondente:
  ```sql
  WHERE KeyId = @kid 
    AND IsDeleted = 0
    AND (RevokedAt IS NULL OR RevokedAt > @TokenIssuedAt)
  ```
- Validar a assinatura do token usando a chave pública
- Tokens assinados com chaves **não ativas**, mas **não expiradas e não revogadas**, devem continuar sendo aceitos (período de sobreposição)
- Incrementar `ValidationCount` da chave (assíncrono, batch updates a cada 1 minuto)
- Atualizar `LastValidatedAt = GETDATE()` (assíncrono, batch updates a cada 1 minuto)

**Validações:**
- A chave deve existir no banco de dados
- A chave não pode estar deletada (`IsDeleted = false`)
- Se a chave foi revogada, verificar se o token foi emitido **antes** da revogação:
  - Se `TokenIssuedAt < RevokedAt`: **rejeitar** token (chave foi comprometida)
  - Se `TokenIssuedAt >= RevokedAt`: registrar evento de segurança suspeito
- A chave não pode estar expirada no momento da validação
- O algoritmo do token deve corresponder ao algoritmo da chave

**Cache de chaves públicas:**
- Cachear chaves públicas em memória ou Redis para performance
- TTL do cache: 5 minutos
- Invalidar cache ao revogar ou desativar chave
- Key do cache: `jwtkey:public:{KeyId}`

**Tratamento de erros:**
- Se `kid` não for encontrado: retornar 401 Unauthorized
- Se assinatura for inválida: retornar 401 Unauthorized
- Se chave estiver revogada e token emitido antes da revogação: retornar 401 Unauthorized
- Registrar tentativas de validação com chaves inválidas para detecção de ataques

---

### 4.4 Rotação Automática de Chaves (Job)
**Contexto:** Processo automatizado executado por Background Service (Hangfire).

**Regras do Job:**
- Deve existir um **Job** responsável pela rotação de chaves
- O job deve ser executado **diariamente** (recomendado: 03:00 UTC)
- O job deve ser **idempotente** (pode ser executado múltiplas vezes sem efeitos colaterais)
- O job deve processar **todos** os `TenantId` que possuem chaves

**Fluxo de rotação:**

1. **Identificar chaves elegíveis para rotação:**
   ```sql
   SELECT * FROM JwtKeys
   WHERE IsActive = 1 
     AND IsDeleted = 0 
     AND RevokedAt IS NULL
     AND NextRotationAt <= GETDATE()
   ```

2. **Para cada chave elegível:**
   
   a. **Validar estado:**
      - Confirmar que a chave ainda está ativa
      - Confirmar que não foi revogada
      - Confirmar que Tenant está ativos

   b. **Gerar nova chave:**
      - Gerar novo par de chaves RSA (mesmo algoritmo e tamanho)
      - Gerar novo `KeyId` (GUID único)
      - Criptografar chave privada
      - Copiar configurações da chave atual (`RotationPolicyDays`, `OverlapPeriodDays`, `MaxTokenLifetimeMinutes`)
      - Definir `IsActive = true`
      - Definir `ActivatedAt = GETDATE()`
      - Definir `ExpiresAt = GETDATE() + RotationPolicyDays`
      - Definir `NextRotationAt = GETDATE() + RotationPolicyDays - OverlapPeriodDays`
      - Definir `CreatedBy` = ID do sistema/job

   c. **Transação atômica:**
      ```sql
      BEGIN TRANSACTION
      
      -- 1. Inserir nova chave ativa
      INSERT INTO JwtKeys (...) VALUES (...)
      
      -- 2. Atualizar chave anterior
      UPDATE JwtKeys
      SET IsActive = 0,
          ExpiresAt = GETDATE() + @OverlapPeriodDays,
          UpdatedBy = @SystemUserId,
          UpdatedAt = GETDATE()
      WHERE Id = @OldKeyId
      
      -- 3. Registrar auditoria
      INSERT INTO AuditLogs (...) VALUES (...)
      
      COMMIT TRANSACTION
      ```

   d. **Invalidar caches:**
      - Invalidar cache de chave ativa
      - Manter cache de chave pública anterior (ainda válida por período de sobreposição)

   e. **Notificações:**
      - Registrar evento de rotação em logs estruturados
      - Opcionalmente, notificar administradores do Tenant
      - Atualizar métricas de rotação

**Período de sobreposição:**
- Durante o `OverlapPeriodDays`, **duas chaves são válidas**:
  - **Nova chave (ativa)**: usada para **assinar novos tokens**
  - **Chave anterior (inativa mas não expirada)**: usada para **validar tokens antigos**
- Este período garante que tokens emitidos pouco antes da rotação continuem válidos
- Após o período de sobreposição, a chave anterior expira automaticamente

**Tratamento de falhas:**
- Se a geração de nova chave falhar, manter chave atual ativa
- Fazer rollback da transação completa em caso de erro
- Registrar falha em logs com severidade ERROR
- Alertar equipe técnica
- Tentar novamente na próxima execução do job
- **Garantia crítica:** Em nenhum momento o sistema pode ficar sem uma chave ativa

**Métricas e monitoramento:**
- Registrar quantidade de rotações bem-sucedidas
- Registrar quantidade de rotações falhadas
- Alertar se rotação falhar por mais de 3 execuções consecutivas
- Monitorar tempo de execução do job
- Monitorar quantidade de chaves expiradas aguardando limpeza

---

### 4.5 Revogação Manual de Chaves
**Contexto:** Revogação imediata em caso de comprometimento ou incidente de segurança.

**Regras:**
- Uma chave pode ser revogada manualmente apenas se **não for a chave ativa**
- Exceção: Se for a chave ativa e for a **única chave** do Tenant:
  - Gerar nova chave automaticamente **antes** de revogar
  - Garantir que sempre exista pelo menos uma chave ativa

**Fluxo de revogação:**
1. Validar permissões (apenas administradores do Tenant ou super admin)
2. Se for chave ativa única, gerar nova chave primeiro
3. Atualizar chave sendo revogada:
   ```sql
   UPDATE JwtKeys
   SET RevokedAt = GETDATE(),
       RevokedReason = @Reason,
       IsActive = 0,
       ExpiresAt = GETDATE(),  -- Expirar imediatamente
       UpdatedBy = @UserId,
       UpdatedAt = GETDATE()
   WHERE Id = @KeyId
     AND TenantId = @TenantId
   ```
4. Invalidar todos os caches relacionados
5. Registrar evento de auditoria crítico
6. Registrar SecurityEvent com severidade HIGH ou CRITICAL
7. Notificar administradores do Tenant

**Motivos de revogação válidos:**
- `"Security breach"` - Comprometimento de segurança
- `"Key compromised"` - Chave vazada ou exposta
- `"Administrative revocation"` - Revogação administrativa
- `"Emergency rotation"` - Rotação de emergência
- `"Policy violation"` - Violação de política
- `"Scheduled decommission"` - Desativação planejada

**Validações:**
- A chave deve pertencer ao Tenant especificado
- O usuário deve ter permissões adequadas
- O motivo de revogação deve ser informado e não vazio
- Não permitir revogação de chave já revogada (operação idempotente)

**Impacto da revogação:**
- Tokens assinados com a chave revogada se tornam **inválidos imediatamente**
- Usuários com tokens assinados pela chave revogada devem fazer re-autenticação
- A validação de tokens deve rejeitar qualquer token assinado pela chave revogada
- O período de sobreposição é cancelado (ExpiresAt = GETDATE())

**Notificações:**
- Enviar e-mail aos administradores do Tenant
- Registrar no dashboard de segurança
- Opcionalmente, notificar usuários afetados sobre necessidade de re-login

---

### 4.6 Limpeza Automática de Chaves Expiradas (Job)
**Contexto:** Processo automatizado de limpeza periódica.

**Regras:**
- Executar semanalmente via job agendado (ex.: domingo, 04:00 UTC)
- Identificar chaves elegíveis para limpeza:
  ```sql
  WHERE ExpiresAt < DATEADD(day, -90, GETDATE())
    AND IsDeleted = 0
    AND IsActive = 0
  ```

**Estratégia de limpeza:**
- **Fase 1 - Soft delete:**
  - Marcar como deletadas (`IsDeleted = true`)
  - Manter para auditoria por 90 dias

- **Fase 2 - Hard delete (após período de retenção):**
  ```sql
  DELETE FROM JwtKeys
  WHERE IsDeleted = 1
    AND UpdatedAt < DATEADD(day, -365, GETDATE())
  ```

**Política de retenção:**
- Chaves ativas: manter indefinidamente
- Chaves revogadas: 90 dias após revogação
- Chaves expiradas naturalmente: 90 dias após expiração
- Chaves deletadas logicamente: 365 dias para conformidade regulatória

**Logging:**
- Registrar quantidade de chaves limpas
- Alertar se volume for anormal

---

## 5. Regras de Integridade e Dependência

### 5.1 Dependências obrigatórias
- Toda JwtKey deve referenciar um `TenantId` válido e existente
- A unicidade de `KeyId` deve ser garantida por constraint UNIQUE

### 5.2 Cascata de operações
**Desativação de Tenant:**
- Revogar todas as chaves de todos os Tenant
- Definir `RevokedReason = "Tenant deactivated"`

### 5.3 Consistência de dados
- Garantir que sempre exista **exatamente uma chave ativa** por Tenant
- Constraint CHECK para validar `RotationPolicyDays BETWEEN 30 AND 365`
- Constraint CHECK para validar `OverlapPeriodDays BETWEEN 1 AND 30`
- Constraint CHECK para validar `MaxTokenLifetimeMinutes BETWEEN 5 AND 1440`
- Índice único filtrado para garantir apenas uma chave ativa por Tenant

### 5.4 Integridade referencial
- FK `TenantId` ? `Tenants(Id)` com `ON DELETE NO ACTION`
- Não permitir orfandade de chaves

---

## 6. Regras de Segurança

### 6.1 Proteção de chaves privadas
- **NUNCA** armazenar chaves privadas em texto plano
- Utilizar AES-256-GCM para criptografar chaves privadas
- Armazenar chave mestra de criptografia em:
  - **Opção 1 (recomendada):** Azure Key Vault / AWS KMS / Google Cloud KMS
  - **Opção 2:** Configuração criptografada com Data Protection API
  - **Opção 3:** Variáveis de ambiente com acesso restrito
- Implementar rotação da chave mestra de criptografia
- Descriptografar chaves privadas apenas em memória, nunca persistir
- Limpar chaves descriptografadas da memória imediatamente após uso
- Considerar uso de Hardware Security Module (HSM) para ambientes de alta segurança

### 6.2 Proteção de dados sensíveis
- **NUNCA** expor chaves privadas em:
  - Logs de aplicação
  - Respostas de API
  - Interfaces de usuário
  - Mensagens de erro
  - Dumps de memória
- **NUNCA** incluir chaves privadas em backups não criptografados
- Mascarar chaves em logs e traces de debug
- Utilizar serviços de gerenciamento de segredos (Azure Key Vault, AWS Secrets Manager, etc.)

### 6.3 Controle de acesso
- Apenas serviços de autenticação/autorização podem acessar chaves privadas
- Implementar RBAC rigoroso para operações de gerenciamento de chaves
- Registrar todos os acessos a chaves em audit logs
- Implementar MFA para operações sensíveis (revogação, etc.)

### 6.4 Auditoria de segurança
- Registrar todos os eventos críticos:
  - Geração de chave
  - Ativação de chave
  - Uso de chave para assinar token
  - Validação de token com chave
  - Revogação de chave
  - Falhas de validação
  - Tentativas de acesso não autorizado
- Incluir em cada evento:
  - Timestamp preciso
  - KeyId
  - UserId (se aplicável)
  - IP de origem
  - Resultado da operação
  - Motivo de falha (se aplicável)

### 6.5 Detecção de anomalias
**Monitorar:**
- Falhas repetidas de validação com mesma chave
- Uso de chaves revogadas
- Uso de chaves expiradas
- Tentativas de acesso a chaves de outros Tenants
- Volume anormal de uso de chave
- Padrões temporais incomuns

**Ações em caso de anomalia:**
- Registrar evento em `SecurityEvents` com severidade apropriada
- Alertar equipe de segurança
- Considerar revogação automática se padrão indicar comprometimento
- Bloquear IPs suspeitos temporariamente

### 6.6 Conformidade
**Requisitos regulatórios:**
- LGPD: Proteção adequada de chaves, auditoria, direito ao esquecimento
- GDPR: Mesmos requisitos, com ênfase em consentimento
- PCI-DSS: Proteção de chaves criptográficas, rotação periódica
- SOC 2: Controles de acesso, auditoria, disponibilidade
- ISO 27001: Gestão de chaves, políticas de segurança

---

## 7. Integração com Outros Módulos

### 7.1 Tenants
- JwtKeys isoladas por Tenant (RLS aplicada na tabela JwtKeys)
- Desativação de Tenant revoga todas as chaves
- Políticas de rotação podem ser configuradas por Tenant

### 7.3 UserSessions
- Tokens JWT emitidos contêm `kid` que referencia JwtKey
- Validação de sessão valida assinatura usando chave pública correspondente
- Revogação de chave pode invalidar sessões ativas

### 7.4 AuditLogs
- Todas as operações críticas geram audit logs
- Logs incluem contexto completo para investigação forense

### 7.5 SecurityEvents
- Eventos de segurança relacionados a chaves são registrados
- Integração com sistema de detecção de ameaças
- Alertas automáticos para eventos críticos

### 7.6 Jobs
- Job de rotação automática de chaves
- Job de limpeza de chaves expiradas
- Jobs devem ser monitorados e ter retry automático

---

## 8. Considerações de Performance

### 8.1 Indexação
**Índices obrigatórios (já definidos no schema):**
```sql
-- Lookup de chave ativa por Tenant
CREATE INDEX IX_JwtKeys_Tenant_Active 
ON dbo.JwtKeys(TenantId, IsActive, IsDeleted) 
WHERE IsDeleted = 0 AND RevokedAt IS NULL;

-- Lookup rápido de chave pública por KeyId
CREATE INDEX IX_JwtKeys_KeyId_Lookup 
ON dbo.JwtKeys(KeyId) 
INCLUDE (Algorithm, PublicKey) 
WHERE IsDeleted = 0 AND RevokedAt IS NULL;

-- Identificação de chaves para rotação
CREATE INDEX IX_JwtKeys_NextRotation 
ON dbo.JwtKeys(NextRotationAt) 
WHERE IsActive = 1 AND IsDeleted = 0 AND RevokedAt IS NULL;

-- Identificação de chaves expiradas
CREATE INDEX IX_JwtKeys_Expiration 
ON dbo.JwtKeys(ExpiresAt) 
WHERE IsActive = 1 AND IsDeleted = 0 AND RevokedAt IS NULL;
```

### 8.2 Caching
**Estratégia de cache multi-camada:**

**Camada 1 - Memória local (in-process):**
- Cachear chave ativa por Tenant
- TTL: 5 minutos
- Key: `jwtkey:active:{TenantId}`
- Invalidar ao ativar nova chave

**Camada 2 - Redis (distribuído):**
- Cachear chaves públicas por KeyId
- TTL: 1 hora
- Key: `jwtkey:public:{KeyId}`
- Invalidar ao revogar ou desativar chave

### 8.3 Otimizações
- Usar connection pooling para banco de dados
- Implementar prepared statements
- Usar batch operations quando possível
- Implementar circuit breaker para Key Vault
- Usar async/await para operações de I/O
- Implementar retry com backoff exponencial
- Batch updates de telemetria (UsageCount, ValidationCount) a cada 1 minuto

---

## 9. Considerações Técnicas

### 9.1 Algoritmos suportados
**Atualmente suportados:**
- **RS256 (RSA SHA-256):** padrão, amplamente suportado
- **RS384 (RSA SHA-384):** maior segurança
- **RS512 (RSA SHA-512):** máxima segurança

**Futuro (roadmap):**
- **ES256 (ECDSA SHA-256):** performance superior, chaves menores
- **ES384 (ECDSA SHA-384)**
- **ES512 (ECDSA SHA-512)**

### 9.2 Tamanhos de chave
**RSA:**
- Mínimo: 2048 bits (padrão)
- Recomendado: 3072 bits (alta segurança)
- Máximo: 4096 bits (máxima segurança, impacto em performance)

**ECDSA (futuro):**
- P-256 (equivalente a RSA 3072)
- P-384 (equivalente a RSA 7680)
- P-521 (equivalente a RSA 15360)

### 9.3 Formato de chaves
- **Chave pública:** PEM (Privacy-Enhanced Mail)
- **Chave privada:** PEM criptografado com AES-256-GCM
- Codificação: Base64
- Charset: UTF-8

### 9.4 Criptografia de chaves privadas
**Algoritmo:** AES-256-GCM
- **Key size:** 256 bits
- **IV size:** 96 bits (recomendação NIST)
- **Tag size:** 128 bits
- **Salt:** único por chave, armazenado junto (se aplicável)
- **Key derivation:** PBKDF2 com 100.000 iterações (se aplicável)

---

## 10. Não Objetivos

Este módulo **NÃO** cobre:
- Gerenciamento de certificados X.509 (TLS/SSL)
- Integração direta com HSM externo (pode ser adicionado via abstração)
- Integração direta com serviços externos de KMS (pode ser adicionado via abstração)
- Suporte a chaves simétricas (HMAC)
- Gerenciamento de chaves de criptografia de dados (AES para dados em repouso, etc.)
- Key escrow ou recuperação de chaves perdidas
- Backup e restore de chaves (responsabilidade da infraestrutura de banco de dados)

---

## 11. Conclusão

O módulo **JWT Key Management** é componente crítico para a segurança do sistema IAM VianaID.

As regras aqui definidas garantem:
- **Segurança robusta:** Proteção de chaves privadas, criptografia em repouso, isolamento multi-tenant
- **Continuidade de serviço:** Rotação sem interrupção, período de sobreposição, garantia de chave ativa
- **Rastreabilidade completa:** Auditoria de todas as operações, telemetria de uso, conformidade regulatória
- **Governança consistente:** Políticas configuráveis, notificações automáticas, revogação imediata
- **Performance otimizada:** Cache multi-camada, índices adequados, validação eficiente
- **Escalabilidade:** Arquitetura preparada para alto volume, suporte a multi-tenant
- **Resiliência:** Tolerância a falhas, retry automático, operações idempotentes
- **Conformidade:** Atendimento a LGPD, GDPR, PCI-DSS, SOC 2, ISO 27001

Com esta estrutura detalhada e abrangente, o sistema garante gestão profissional de chaves criptográficas, atendendo aos mais altos padrões de segurança da indústria e possibilitando autenticação confiável e escalável.

---

**Documento mantido por:** Equipe de Segurança e Arquitetura - VianaHub.VianaID  
**Última revisão:** 25/12/2025  
**Versão:** 2.0  
**Status:** Aprovado para implementação
