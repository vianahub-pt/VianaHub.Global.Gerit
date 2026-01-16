# ?? Correções Aplicadas - VianaHub.Global.Gerit.Api

## ?? Resumo das Mudanças

Este documento descreve todas as correções aplicadas para resolver os erros de configuração e fazer a API funcionar corretamente com Minimal APIs.

---

## ? Problema 1: Configurações Faltantes (System.InvalidOperationException)

### ? Erro Original:
```
System.InvalidOperationException: Erro(s) de configuração detectado(s):
? ConnectionStrings:VianaIDConnection não está configurada
? ConnectionStrings:HangfireConnection não está configurada
? JwtKeyManagement:EncryptionKey não está configurada
? JwtSettings:Issuer não está configurado
? JwtSettings:Audience não está configurado
? JwtSettings:AccessTokenExpirationMinutes deve ser maior que zero
? JwtSettings:RefreshTokenExpirationDays deve ser maior que zero
```

### ? Solução Aplicada:

#### 1. Atualização do `appsettings.json`
Adicionadas todas as configurações obrigatórias:

```json
{
  "ConnectionStrings": {
    "VianaIDConnection": "Server=82.29.172.68,1433;Database=Gerit;...",
    "HangfireConnection": "Server=82.29.172.68,1433;Database=Gerit_Hangfire;..."
  },
  "JwtKeyManagement": {
    "EncryptionKey": "CHANGE-THIS-KEY-IN-PRODUCTION-MIN-32-CHARS-REQUIRED-FOR-SECURITY"
  },
  "JwtSettings": {
    "Issuer": "VianaHub.Global.Gerit.Api",
    "Audience": "VianaHub.Global.Gerit.Client",
    "AccessTokenExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 30
  },
  "RateLimiting": {
    "EnableRateLimiting": true,
    "GeneralRules": {
      "PermitLimit": 100,
      "Window": "00:01:00",
      "QueueLimit": 10
    },
    "AuthenticationEndpoints": {
      "PermitLimit": 10,
      "Window": "00:01:00",
      "QueueLimit": 2
    }
  },
  "Cors": {
    "EnableCors": true,
    "AllowedOrigins": [
      "https://gerit.com",
      "https://www.gerit.com",
      "https://app.gerit.com"
    ]
  }
}
```

#### 2. Atualização do `appsettings.Development.json`
Configurações mais permissivas para desenvolvimento:

```json
{
  "ConnectionStrings": {
    "VianaIDConnection": "Server=82.29.172.68,1433;Database=Gerit_Dev;...",
    "HangfireConnection": "Server=82.29.172.68,1433;Database=Gerit_Dev_Hangfire;..."
  },
  "JwtKeyManagement": {
    "EncryptionKey": "DEV-KEY-ONLY-DO-NOT-USE-IN-PRODUCTION-32-CHARS-MIN-REQUIRED"
  },
  "JwtSettings": {
    "Issuer": "VianaHub.Global.Gerit.Api.Dev",
    "Audience": "VianaHub.Global.Gerit.Client.Dev",
    "AccessTokenExpirationMinutes": 1440,
    "RefreshTokenExpirationDays": 90
  },
  "RateLimiting": {
    "EnableRateLimiting": false
  },
  "Cors": {
    "EnableCors": true,
    "AllowedOrigins": [ "*" ]
  }
}
```

#### 3. Criação do `appsettings.Example.json`
Template para novos ambientes com placeholders.

---

## ? Problema 2: Swagger Erro 500 (Failed to load API definition)

### ? Erro Original:
```
Failed to load API definition.
Fetch error
response status is 500 https://localhost:7111/swagger/v1/swagger.json
```

### ?? Causa Raiz:
O `SwaggerTranslationFilter` depende do `ILocalizationService`, que por sua vez precisa dos arquivos de localização. O arquivo `messages.en-US.json` (fallback) estava ausente, causando exceção ao gerar a documentação do Swagger.

### ? Solução Aplicada:

#### 1. Criação dos Arquivos de Localização

**?? `Localization/messages.pt-BR.json`**
```json
{
  "Swagger.Api.Title": "IAM VianaID API",
  "Swagger.Api.Title.Development": "IAM VianaID API - Desenvolvimento",
  "Swagger.Api.Description": "API de autenticação e autorização do VianaHub",
  "Swagger.Security.Bearer.Description": "Insira o token JWT no formato: Bearer {seu_token}"
}
```

**?? `Localization/messages.en-US.json`** (? CRÍTICO - Fallback)
```json
{
  "Swagger.Api.Title": "IAM VianaID API",
  "Swagger.Api.Title.Development": "IAM VianaID API - Development",
  "Swagger.Api.Description": "VianaHub authentication and authorization API",
  "Swagger.Security.Bearer.Description": "Enter the JWT token in the format: Bearer {your_token}"
}
```

**?? `Localization/messages.es-ES.json`**
```json
{
  "Swagger.Api.Title": "IAM VianaID API",
  "Swagger.Api.Title.Development": "IAM VianaID API - Desarrollo",
  "Swagger.Api.Description": "API de autenticación y autorización de VianaHub",
  "Swagger.Security.Bearer.Description": "Ingrese el token JWT en el formato: Bearer {su_token}"
}
```

---

## ? Problema 3: Configuração Incorreta para Minimal APIs

### ? Problema Original:
O código estava configurado para usar Controllers tradicionais (`AddControllers`, `MapControllers`), mas a aplicação é **Minimal API**.

### ? Solução Aplicada:

#### 1. Remoção do Controller Criado
- ? Removido: `Controllers/HealthController.cs`

#### 2. Ajuste do `Program.cs`

**ANTES:**
```csharp
builder.Services.AddControllers()
    .AddJsonOptions(options => { ... })
    .AddMvcOptions(options => { ... });
    
// ...

app.MapControllers();
```

**DEPOIS:**
```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger(builder.Configuration, builder.Environment);

// ...

// Minimal API Endpoints
app.MapGet("/health", () => Results.Ok(new
{
    Status = "Healthy",
    Timestamp = DateTime.UtcNow,
    Version = "1.0.0",
    Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
}))
.WithName("HealthCheck")
.WithTags("Health")
.Produces(200)
.WithSummary("Health Check")
.WithDescription("Verifica se a API está funcionando corretamente");
```

#### 3. Pipeline Correto de Middlewares

```csharp
app.UseHttpsRedirection();

// Rate Limiting (apenas se habilitado)
var rateLimitingEnabled = builder.Configuration.GetValue<bool>("RateLimiting:EnableRateLimiting", true);
if (rateLimitingEnabled)
{
    app.UseRateLimiter();
}

// CORS (apenas se habilitado)
var corsEnabled = builder.Configuration.GetValue<bool>("Cors:EnableCors", true);
if (corsEnabled)
{
    var policyName = builder.Configuration.GetValue<string>("Cors:PolicyName") ?? "VianaIDCorsPolicy";
    app.UseCors(policyName);
}

app.UseAuthentication();
app.UseAuthorization();

// Endpoints Minimal API
app.MapGet("/health", ...);

app.Run();
```

---

## ? Problema 4: Rate Limiting e CORS - Middlewares Não Registrados

### ? Erro Original:
```
System.InvalidOperationException: Unable to find the required services. 
Please add all the required services by calling 'IServiceCollection.AddRateLimiter' 
in the application startup code.
```

### ?? Causa Raiz:
Os métodos `AddRateLimitingConfiguration` e `AddCorsConfiguration` não registram os serviços quando as funcionalidades estão desabilitadas (`EnableRateLimiting: false` ou `EnableCors: false`), mas os middlewares (`app.UseRateLimiter()` e `app.UseCors()`) eram chamados incondicionalmente.

### ? Solução Aplicada:

#### 1. Uso Condicional dos Middlewares

O uso dos middlewares agora verifica se a funcionalidade está habilitada:

```csharp
// Rate Limiting (apenas se habilitado)
var rateLimitingEnabled = builder.Configuration.GetValue<bool>("RateLimiting:EnableRateLimiting", true);
if (rateLimitingEnabled)
{
    app.UseRateLimiter();
}

// CORS (apenas se habilitado)
var corsEnabled = builder.Configuration.GetValue<bool>("Cors:EnableCors", true);
if (corsEnabled)
{
    var policyName = builder.Configuration.GetValue<string>("Cors:PolicyName") ?? "VianaIDCorsPolicy";
    app.UseCors(policyName);
}
```

#### 2. Correção do Formato de Window no Rate Limiting

**ANTES (? INCORRETO):**
```json
"RateLimiting": {
  "GeneralRules": {
    "Window": 60  // Número inteiro
  }
}
```

**DEPOIS (? CORRETO):**
```json
"RateLimiting": {
  "GeneralRules": {
    "Window": "00:01:00"  // TimeSpan string
  }
}
```

O código `RateLimitingSetup.cs` espera o Window no formato TimeSpan string (`TimeSpan.Parse()`).

---

## ?? Estrutura Final do Projeto

```
src/VianaHub.Global.Gerit.Api/
??? Configuration/
?   ??? ConfigurationValidator.cs      ? Validação de configurações
?   ??? RateLimitingSetup.cs           ? Rate Limiting condicional
?   ??? CorsSetup.cs                   ? CORS condicional
?   ??? Swagger/
?       ??? SwaggerSetup.cs            ? Configuração do Swagger
?       ??? SwaggerTranslationFilter.cs ? Filtro de tradução
??? Localization/
?   ??? messages.pt-BR.json            ? Português (Brasil)
?   ??? messages.en-US.json            ? Inglês (Fallback)
?   ??? messages.es-ES.json            ? Espanhol
??? Middleware/
?   ??? SwaggerLocalizationMiddleware.cs
?   ??? RequestLocalizationMiddleware.cs
??? Services/
?   ??? LocalizationService.cs         ? Serviço de localização
?   ??? CurrentUserApiService.cs       ? Serviço de usuário atual
??? appsettings.json                   ? Configurações de produção
??? appsettings.Development.json       ? Configurações de desenvolvimento
??? appsettings.Example.json           ? Template de configurações
??? Program.cs                         ? Configurado para Minimal API com middlewares condicionais
```

---

## ?? Checklist de Validação

### ? Configurações
- [x] ConnectionStrings configuradas (VianaIDConnection, HangfireConnection)
- [x] JWT configurado (EncryptionKey, Issuer, Audience, Expiration)
- [x] Rate Limiting configurado com Window no formato correto
- [x] CORS configurado

### ? Swagger
- [x] Arquivos de localização criados (pt-BR, en-US, es-ES)
- [x] SwaggerTranslationFilter funcionando
- [x] Swagger UI acessível em Development

### ? Minimal API
- [x] Removidas configurações de Controllers
- [x] Pipeline de middlewares correto e condicional
- [x] Endpoint de health check criado
- [x] AddEndpointsApiExplorer configurado

### ? Middlewares Condicionais
- [x] Rate Limiting usado apenas quando habilitado
- [x] CORS usado apenas quando habilitado
- [x] Nome correto da política CORS utilizado

---

## ?? Como Executar

### 1. Desenvolvimento Local
```bash
cd src/VianaHub.Global.Gerit.Api
dotnet run --environment Development
```

### 2. Acessar Swagger UI
```
https://localhost:7111/swagger
```

### 3. Testar Health Check
```bash
curl https://localhost:7111/health
```

**Resposta Esperada:**
```json
{
  "status": "Healthy",
  "timestamp": "2026-01-14T11:50:00.000Z",
  "version": "1.0.0",
  "environment": "Development"
}
```

---

## ?? Atenções Importantes

### ?? Segurança

1. **JWT EncryptionKey em Produção**
   - ?? A chave no `appsettings.json` é um placeholder
   - ? **DEVE** ser substituída por uma chave forte e aleatória de no mínimo 32 caracteres
   - ? **NUNCA** commitar a chave de produção no Git
   - ? Usar Azure Key Vault, AWS Secrets Manager ou variáveis de ambiente

   ```bash
   # Gerar chave forte
   openssl rand -base64 48
   ```

2. **Senhas de Banco de Dados**
   - ?? As senhas estão hardcoded nos arquivos de configuração
   - ? Em produção, usar Azure App Configuration ou variáveis de ambiente

3. **CORS em Produção**
   - ?? Nunca usar `AllowedOrigins: ["*"]` em produção
   - ? Especificar origins exatas

4. **Rate Limiting em Produção**
   - ? **SEMPRE** habilitar em produção (`EnableRateLimiting: true`)
   - ? Ajustar limites de acordo com a capacidade do servidor

### ?? Configurações Condicionais

A API suporta desabilitar funcionalidades via configuração:

```json
{
  "RateLimiting": {
    "EnableRateLimiting": false  // Desabilita Rate Limiting
  },
  "Cors": {
    "EnableCors": false  // Desabilita CORS
  }
}
```

Quando desabilitadas:
- ? Os serviços não são registrados (`AddRateLimiter`, `AddCors`)
- ? Os middlewares não são usados (`UseRateLimiter`, `UseCors`)
- ? A aplicação inicia normalmente

### ?? Próximos Passos

1. **Adicionar Endpoints de Negócio**
   - Criar endpoints para autenticação
   - Criar endpoints para gerenciamento de tenants
   - Criar endpoints para RBAC

2. **Configurar Health Checks Avançados**
   ```csharp
   builder.Services.AddHealthChecks()
       .AddSqlServer(builder.Configuration.GetConnectionString("VianaIDConnection"))
       .AddDbContextCheck<GeritDbContext>();
   
   app.MapHealthChecks("/health/ready");
   app.MapHealthChecks("/health/live");
   ```

3. **Adicionar Versionamento de API**
   ```csharp
   builder.Services.AddApiVersioning()
       .AddApiExplorer(options => {
           options.GroupNameFormat = "'v'VVV";
           options.SubstituteApiVersionInUrl = true;
       });
   ```

4. **Configurar OpenTelemetry** (já tem `TelemetryInterceptor`)
   - Adicionar métricas
   - Adicionar traces distribuídos

5. **Habilitar Rate Limiting em Desenvolvimento (Opcional)**
   - Testar comportamento de throttling
   - Validar mensagens de erro 429
   - Ajustar limites antes de ir para produção

---

## ?? Referências

- [ASP.NET Core Minimal APIs](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis)
- [Rate Limiting Middleware](https://learn.microsoft.com/aspnet/core/performance/rate-limit)
- [CORS in ASP.NET Core](https://learn.microsoft.com/aspnet/core/security/cors)
- [Swagger/OpenAPI](https://learn.microsoft.com/aspnet/core/tutorials/web-api-help-pages-using-swagger)
- [Configuration in ASP.NET Core](https://learn.microsoft.com/aspnet/core/fundamentals/configuration)
- [Row Level Security (RLS)](https://learn.microsoft.com/sql/relational-databases/security/row-level-security)

---

**Data:** 14/01/2026  
**Versão:** .NET 8  
**Status:** ? Todas as correções aplicadas e validadas  
**Última Atualização:** Correção de middlewares condicionais (Rate Limiting e CORS)
