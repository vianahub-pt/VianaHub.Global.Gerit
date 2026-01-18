# Correção do Erro de Conversão de Tipo no Banco de Dados

## ?? Problema Identificado

O erro **"Explicit conversion from data type uniqueidentifier to int is not allowed"** ocorre devido a uma **incompatibilidade de tipos** entre:

1. **Banco de Dados**: Colunas `TenantId` são do tipo `INT`
2. **SESSION_CONTEXT**: Estava recebendo `GUID/UNIQUEIDENTIFIER` do interceptor
3. **Função RLS**: Tentava fazer `CAST(SESSION_CONTEXT(N'TenantId') AS INT)` causando erro

### Causa Raiz

A função Row-Level Security (RLS) `fn_TenantAccessPredicate` estava tentando converter um `UNIQUEIDENTIFIER` para `INT`, o que é uma conversão explícita não permitida pelo SQL Server.

## ? Correções Aplicadas

### 1. Código C# (6 Repositórios)

Removidas conversões problemáticas nos filtros de busca:

- ? `RoleDataRepository.cs`
- ? `ActionDataRepository.cs`
- ? `ResourceDataRepository.cs`
- ? `TenantDataRepository.cs`
- ? `PlanDataRepository.cs`
- ? `SubscriptionDataRepository.cs`

### 2. Interceptor de Sessão

Corrigido `TenantSessionConnectionInterceptor.cs` para:
- Enviar `TenantId` como **INT** ao invés de **GUID**
- Usar `0` (zero) para SuperAdmin ao invés de `Guid.Empty`

### 3. Função RLS (Banco de Dados)

A função `fn_TenantAccessPredicate` precisa ser recriada para:
- Aceitar `TenantId` como INT
- Verificar `IsSuperAdmin` para bypass
- Compatibilidade com o tipo correto do SESSION_CONTEXT

## ?? Ações Necessárias

### ?? IMPORTANTE: Execute o Script SQL

**Execute este script no banco de dados GeritDb:**

`src\VianaHub.Global.Gerit.Infra.Data\Migrations\fix-rls-function-type-mismatch.sql`

**O que o script faz:**

1. Remove a política de segurança existente
2. Remove a função RLS antiga
3. Recria a função com os tipos corretos
4. Recria a política de segurança
5. Executa testes de validação

### Como Executar

1. Abra o SQL Server Management Studio (SSMS) ou Azure Data Studio
2. Conecte-se ao banco de dados `GeritDb`
3. Abra o arquivo: `src\VianaHub.Global.Gerit.Infra.Data\Migrations\fix-rls-function-type-mismatch.sql`
4. Execute o script completo (pressione F5 ou clique em "Execute")
5. Verifique a saída - deve ver mensagens de sucesso

**Saída esperada:**
```
Iniciando correção da função RLS fn_TenantAccessPredicate...
Removendo política de segurança existente...
Política removida
Removendo função existente...
Função removida
Criando nova função fn_TenantAccessPredicate...
Função criada com sucesso
Recriando política de segurança...
Política de segurança criada com sucesso
Correção concluída! A função RLS agora suporta:
  1. SuperAdmin mode (IsSuperAdmin = 1 bypass RLS)
  2. TenantId como INT no SESSION_CONTEXT
```

## ?? Verificação

Após aplicar todas as correções:

### 1. Reinicie a API

```powershell
cd C:\git\my\VianaHub.Global.Gerit
dotnet run --project src\VianaHub.Global.Gerit.Api\VianaHub.Global.Gerit.Api.csproj
```

### 2. Teste no Swagger

Acesse: `https://localhost:7111/swagger`

Teste estes endpoints:

| Endpoint | Método | Deve Retornar |
|----------|--------|---------------|
| `/api/roles` | GET | Lista de papéis (200 OK) |
| `/api/actions` | GET | Lista de ações (200 OK) |
| `/api/resources` | GET | Lista de recursos (200 OK) |
| `/api/tenants` | GET | Lista de inquilinos (200 OK) |
| `/api/plans` | GET | Lista de planos (200 OK) |
| `/api/subscriptions` | GET | Lista de assinaturas (200 OK) |

### 3. Verifique os Logs

Procure por logs do interceptor RLS:

```
?? [RLS] Development SuperAdmin context configured successfully. IsSuperAdmin=1, TenantId=0
```

## ?? Checklist

- [x] Código dos repositórios corrigido
- [x] Interceptor de sessão corrigido
- [x] Build bem-sucedido
- [ ] Script SQL executado
- [ ] Função RLS recriada
- [ ] API reiniciada
- [ ] Endpoints testados com sucesso

## ?? Detalhes Técnicos

### Alteração na Função RLS

**Antes (Problema):**
```sql
WHERE @TenantId = CAST(SESSION_CONTEXT(N'TenantId') AS INT)
```

**Depois (Solução):**
```sql
WHERE 
    CAST(SESSION_CONTEXT(N'IsSuperAdmin') AS INT) = 1
    OR
    (
        SESSION_CONTEXT(N'TenantId') IS NOT NULL
        AND @TenantId = CAST(CAST(SESSION_CONTEXT(N'TenantId') AS VARBINARY(4)) AS INT)
    )
```

### Alteração no Interceptor

**Antes (Problema):**
```csharp
tenantValue = tenantGuid; // GUID/UNIQUEIDENTIFIER
```

**Depois (Solução):**
```csharp
tenantValue = tenantInt; // INT
```

## ?? Resultado Esperado

Após todas as correções:

? API inicia sem erros  
? Todos os endpoints funcionam corretamente  
? Row-Level Security operacional  
? SuperAdmin mode funcionando em Development  
? Nenhum erro de conversão de tipos  

## ?? Notas Importantes

1. **TenantId agora é INT**: A arquitetura assume que `TenantId` é um inteiro, não um GUID
2. **SuperAdmin = 0**: O valor `0` é usado para indicar SuperAdmin (acesso total)
3. **Development Mode**: Automaticamente configura SuperAdmin quando não há autenticação
4. **RLS Ativo**: A política de segurança permanece ativa em todos os ambientes

## ?? Troubleshooting

### Se o erro persistir:

1. Verifique se o script SQL foi executado completamente
2. Confirme que a função foi recriada: `SELECT * FROM sys.objects WHERE name = 'fn_TenantAccessPredicate'`
3. Confirme que a política existe: `SELECT * FROM sys.security_policies WHERE name = 'TenantSecurityPolicy'`
4. Verifique os logs da aplicação para confirmar o SESSION_CONTEXT
5. Execute a query de teste incluída no script SQL

### Query de Diagnóstico:

```sql
-- Verifica o estado atual do RLS
SELECT 
    sp.name AS PolicyName,
    o.name AS TableName,
    sp.is_enabled AS IsEnabled
FROM sys.security_policies sp
INNER JOIN sys.security_predicates pred ON sp.object_id = pred.object_id
INNER JOIN sys.objects o ON pred.target_object_id = o.object_id
WHERE sp.name = 'TenantSecurityPolicy'
ORDER BY o.name
