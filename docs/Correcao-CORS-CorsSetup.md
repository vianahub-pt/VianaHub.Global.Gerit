# Correção do Problema CORS - CorsSetup

## ?? Problema Identificado

A aplicação estava parando na classe `CorsSetup` devido a **configurações CORS incompletas** e **conflito entre políticas de segurança**.

### Problemas Detectados:

1. **Configurações CORS Incompletas no `appsettings.json`**
   - Faltavam: `PolicyName`, `AllowedMethods`, `AllowedHeaders`, `AllowCredentials`, `MaxAge`
   - O código esperava essas propriedades, mas elas não estavam definidas

2. **Conflito de Segurança CORS**
   - Em Development: `AllowedOrigins: ["*"]` + `AllowCredentials: true`
   - **ERRO**: O ASP.NET Core **não permite** `AllowAnyOrigin()` + `AllowCredentials()` por razões de segurança
   - Especificação CORS: https://developer.mozilla.org/en-US/docs/Web/HTTP/CORS

---

## ? Correções Aplicadas

### 1. **CorsSetup.cs** - Lógica Corrigida

```csharp
// ? Detectar ambiente automaticamente
var env = configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT");
var isDevelopment = env?.Equals("Development", StringComparison.OrdinalIgnoreCase) == true;

// ? Verificar se usa AllowAnyOrigin
var allowAnyOrigin = allowedOrigins.Length == 0 || 
                   allowedOrigins.Any(o => o.Equals("*", StringComparison.OrdinalIgnoreCase));

if (allowAnyOrigin)
{
    // ? Bloquear em produção
    if (env?.Equals("Production", StringComparison.OrdinalIgnoreCase) == true)
    {
        throw new InvalidOperationException(
            "?? ERRO DE SEGURANÇA: CORS com origens '*' não é permitido em produção.");
    }

    policy.AllowAnyOrigin();
    // ? NÃO usar AllowCredentials() quando AllowAnyOrigin() está ativo
}
else
{
    policy.WithOrigins(allowedOrigins)
          .SetIsOriginAllowedToAllowWildcardSubdomains();
    
    // ? Só permitir credenciais com origens específicas
    if (allowCredentials.HasValue ? allowCredentials.Value : true)
    {
        policy.AllowCredentials();
    }
}
```

**Mudanças:**
- ? Tornou `allowCredentials` **nullable** para detectar se foi configurado explicitamente
- ? **Separa** a lógica: `AllowAnyOrigin()` **OU** `AllowCredentials()`, nunca ambos
- ? Bloqueia `AllowAnyOrigin()` em **produção** (segurança)

---

### 2. **appsettings.json** - Configuração de Produção

```json
"Cors": {
  "EnableCors": true,
  "PolicyName": "VianaIDCorsPolicy",
  "AllowedOrigins": [
    "https://gerit.com",
    "https://www.gerit.com",
    "https://app.gerit.com"
  ],
  "AllowedMethods": [ "GET", "POST", "PUT", "PATCH", "DELETE", "OPTIONS" ],
  "AllowedHeaders": [ "Content-Type", "Authorization", "X-Requested-With", "X-Tenant-Id" ],
  "AllowCredentials": true,
  "MaxAge": 600
}
```

**Características:**
- ? **Origens específicas** (não usa `*`)
- ? **AllowCredentials**: `true` (seguro com origens específicas)
- ? **Headers customizados**: `X-Tenant-Id` para multi-tenancy
- ? **MaxAge**: 10 minutos (600s) para cache de preflight

---

### 3. **appsettings.Development.json** - Configuração de Desenvolvimento

```json
"Cors": {
  "EnableCors": true,
  "PolicyName": "VianaIDCorsPolicy",
  "AllowedOrigins": [ "*" ],
  "AllowedMethods": [ "*" ],
  "AllowedHeaders": [ "*" ],
  "AllowCredentials": false,
  "MaxAge": 86400
}
```

**Características:**
- ? **AllowedOrigins**: `["*"]` (permite qualquer origem em dev)
- ? **AllowCredentials**: `false` (obrigatório quando usa `*`)
- ? **MaxAge**: 24 horas (86400s) para evitar preflight constante em dev

---

## ?? Matriz de Compatibilidade CORS

| Configuração | AllowAnyOrigin | AllowCredentials | Válido? |
|--------------|----------------|------------------|---------|
| Produção | ? (origens específicas) | ? true | ? SIM |
| Development | ? `["*"]` | ? false | ? SIM |
| ?? Incorreto | ? `["*"]` | ? true | ? **NÃO** (erro CORS) |

---

## ?? Segurança

### Headers Expostos
```csharp
policy.WithExposedHeaders(
    "X-Pagination",
    "X-Total-Count",
    "X-Rate-Limit-Remaining",
    "X-Rate-Limit-Reset",
    "Retry-After"
);
```

Esses headers são necessários para:
- **Paginação**: `X-Pagination`, `X-Total-Count`
- **Rate Limiting**: `X-Rate-Limit-Remaining`, `X-Rate-Limit-Reset`
- **Retry**: `Retry-After` (quando rate limit é atingido)

---

## ?? Como Testar

### 1. **Desenvolvimento** (CORS aberto)
```bash
curl -X OPTIONS http://localhost:5000/health \
  -H "Origin: http://localhost:3000" \
  -H "Access-Control-Request-Method: GET" \
  -v
```

**Resposta esperada:**
```
Access-Control-Allow-Origin: *
Access-Control-Allow-Methods: *
Access-Control-Allow-Headers: *
```

### 2. **Produção** (CORS restrito)
```bash
curl -X OPTIONS https://api.gerit.com/health \
  -H "Origin: https://app.gerit.com" \
  -H "Access-Control-Request-Method: GET" \
  -v
```

**Resposta esperada:**
```
Access-Control-Allow-Origin: https://app.gerit.com
Access-Control-Allow-Credentials: true
Access-Control-Allow-Methods: GET, POST, PUT, PATCH, DELETE, OPTIONS
```

### 3. **Teste de Bloqueio em Produção**
Se tentar usar `AllowedOrigins: ["*"]` em produção, receberá:
```
InvalidOperationException: 
?? ERRO DE SEGURANÇA: CORS com origens '*' não é permitido em produção.
```

---

## ?? Checklist de Validação

- ? Build compila sem erros
- ? Configurações CORS completas em `appsettings.json`
- ? Configurações CORS completas em `appsettings.Development.json`
- ? Lógica de `AllowCredentials` condicional implementada
- ? Bloqueio de `AllowAnyOrigin` em produção
- ? Headers customizados incluídos (`X-Tenant-Id`)
- ? Headers expostos para paginação e rate limiting

---

## ?? Referências

- [MDN - CORS](https://developer.mozilla.org/en-US/docs/Web/HTTP/CORS)
- [ASP.NET Core CORS](https://learn.microsoft.com/en-us/aspnet/core/security/cors)
- [CORS Best Practices](https://owasp.org/www-project-web-security-testing-guide/latest/4-Web_Application_Security_Testing/11-Client-side_Testing/07-Testing_Cross_Origin_Resource_Sharing)

---

## ?? Próximos Passos

1. ? **Testar a aplicação** em ambiente de desenvolvimento
2. ? **Validar CORS** com preflight requests
3. ? **Configurar origens específicas** antes de deploy em produção
4. ?? **Revisar** a lista de origens permitidas conforme necessário
5. ?? **Monitorar** logs de CORS rejeitados (se habilitado)
