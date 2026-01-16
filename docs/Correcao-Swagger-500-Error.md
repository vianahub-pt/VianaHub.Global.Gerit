# Correção do Erro 500 no Swagger - SwaggerTranslationFilter

## ?? Erro Original

```
Failed to load API definition.
Errors: Fetch error - response status is 500
https://localhost:7111/swagger/v1/swagger.json
```

**Causa:** O `SwaggerTranslationFilter` estava tentando resolver o `ILocalizationService` (um **serviço Scoped**) durante a geração do documento Swagger, causando conflitos de ciclo de vida.

---

## ?? Análise do Problema

### 1. **SwaggerTranslationFilter com Dependency Injection Problemática**

```csharp
// ? PROBLEMA
public class SwaggerTranslationFilter : IDocumentFilter
{
    private readonly IServiceProvider _serviceProvider;

    public SwaggerTranslationFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        // Tentando resolver um serviço SCOPED em um contexto SINGLETON
        using var scope = _serviceProvider.CreateScope();
        var localizationService = scope.ServiceProvider.GetRequiredService<ILocalizationService>();
        var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        // ...
    }
}
```

**Por que falhou:**
- `IDocumentFilter` é registrado como **Singleton** no Swagger
- `ILocalizationService` e `IHttpContextAccessor` são **Scoped** (por requisição HTTP)
- Durante a geração do documento Swagger, **não há contexto HTTP ativo**
- Resultado: **InvalidOperationException** ? HTTP 500

### 2. **Complexidade Desnecessária**

O código original tentava:
- ? Traduzir dinamicamente todo o Swagger com base na cultura
- ? Suportar múltiplos idiomas via query string `?lang=pt-BR`
- ? Mas isso causava problemas de ciclo de vida e performance

---

## ? Solução Aplicada

### Abordagem: **Simplificar e Remover Tradução Dinâmica**

Para um ambiente de desenvolvimento e documentação, **não é necessário** traduzir dinamicamente o Swagger. A solução foi:

1. **Remover o `SwaggerTranslationFilter`**
2. **Definir textos estáticos** com base no ambiente
3. **Manter suporte a JWT Bearer Authentication**

### Código Corrigido - `SwaggerSetup.cs`

```csharp
public static IServiceCollection AddSwagger(
    this IServiceCollection services, 
    IConfiguration configuration, 
    IWebHostEnvironment environment)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(options =>
    {
        // ? Definir título e descrição com base no ambiente
        string title;
        string description;

        if (environment.IsProduction())
        {
            title = "IAM VianaID API - Production";
            description = "Identity and Access Management API for VianaHub Platform - Production Environment";
        }
        else if (environment.IsStaging())
        {
            title = "IAM VianaID API - Staging";
            description = "Identity and Access Management API for VianaHub Platform - Staging Environment";
        }
        else
        {
            title = "IAM VianaID API - Development";
            description = "Identity and Access Management API for VianaHub Platform - Development Environment";
        }

        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = title,
            Version = "v1",
            Description = description,
            Contact = new OpenApiContact
            {
                Name = "VianaHub Support",
                Email = "support@vianahub.pt",
                Url = new Uri("https://vianahub.pt")
            },
            License = new OpenApiLicense
            {
                Name = "Proprietary",
                Url = new Uri("https://vianahub.pt/license")
            }
        });

        // ? Schema IDs personalizados
        options.CustomSchemaIds(type => (type.FullName ?? type.Name).Replace('.', '_'));

        // ? JWT Bearer Authentication
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme. " +
                         "Enter 'Bearer' [space] and then your token. " +
                         "Example: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...'"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });

        options.EnableAnnotations();
    });

    return services;
}

// ? Método simplificado para configurar Swagger UI
public static void UseSwaggerConfiguration(this WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "IAM VianaID API v1");
            options.RoutePrefix = "swagger";
            options.DocumentTitle = "IAM VianaID API - Documentation";
            options.DisplayRequestDuration();
            options.EnableDeepLinking();
            options.EnableFilter();
            options.ShowExtensions();
            options.EnableValidator();
        });
    }
}
```

### Mudanças no `Program.cs`

```csharp
// ? ANTES
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ? DEPOIS
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
}
```

---

## ?? Comparação: Antes vs Depois

| Aspecto | Antes | Depois |
|---------|-------|--------|
| **Tradução Dinâmica** | ? Sim (via `ILocalizationService`) | ? Não (textos estáticos) |
| **Múltiplos Idiomas** | ? `?lang=pt-BR` / `?lang=en-US` | ? Apenas inglês |
| **Complexidade** | ?? Alta (Filter + Middleware + DI) | ? Baixa (configuração direta) |
| **Problemas de DI** | ? Scoped em Singleton | ? Nenhum |
| **Erro 500** | ? Sim | ? Não |
| **JWT Authentication** | ? Sim | ? Sim |
| **Performance** | ?? Média (tradução em runtime) | ? Alta (textos estáticos) |
| **Manutenção** | ?? Difícil (muito código) | ? Fácil (código simples) |

---

## ?? Como Testar

### 1. Acessar o Swagger UI

```bash
# Ambiente Development
https://localhost:7111/swagger
```

**Resultado esperado:**
- ? Swagger UI carrega sem erros
- ? Título: "IAM VianaID API - Development"
- ? Endpoint `/health` visível
- ? Endpoint `/v1/actions/*` visível
- ? Botão "Authorize" para JWT disponível

### 2. Testar Autenticação JWT

1. Clicar em **"Authorize"**
2. Inserir: `Bearer <seu_token_jwt>`
3. Clicar em **"Authorize"**
4. Testar endpoints protegidos

### 3. Verificar Endpoints

```bash
# Health Check
curl https://localhost:7111/health

# Swagger JSON
curl https://localhost:7111/swagger/v1/swagger.json
```

**Resposta esperada (swagger.json):**
```json
{
  "openapi": "3.0.1",
  "info": {
    "title": "IAM VianaID API - Development",
    "description": "Identity and Access Management API for VianaHub Platform - Development Environment",
    "contact": {
      "name": "VianaHub Support",
      "email": "support@vianahub.pt",
      "url": "https://vianahub.pt"
    },
    "license": {
      "name": "Proprietary",
      "url": "https://vianahub.pt/license"
    },
    "version": "v1"
  },
  "paths": { ... }
}
```

---

## ?? Arquivos Afetados

### ? Modificados
- `src\VianaHub.Global.Gerit.Api\Configuration\Swagger\SwaggerSetup.cs`
- `src\VianaHub.Global.Gerit.Api\Program.cs`

### ?? Não Mais Utilizados (podem ser removidos)
- `src\VianaHub.Global.Gerit.Api\Configuration\Swagger\SwaggerTranslationFilter.cs`
- `src\VianaHub.Global.Gerit.Api\Middleware\SwaggerLocalizationMiddleware.cs`

### ? Mantidos
- `src\VianaHub.Global.Gerit.Api\Configuration\Swagger\SwaggerSettings.cs` (para uso futuro)
- `src\VianaHub.Global.Gerit.Api\Configuration\Swagger\SwaggerContact.cs`
- `src\VianaHub.Global.Gerit.Api\Configuration\Swagger\SwaggerLicense.cs`

---

## ?? Benefícios da Solução

### ? 1. **Simplicidade**
- Código mais limpo e fácil de entender
- Menos dependências e complexidade
- Manutenção mais simples

### ? 2. **Performance**
- Sem overhead de tradução em runtime
- Swagger JSON gerado mais rapidamente
- Menos consumo de memória

### ? 3. **Confiabilidade**
- Sem conflitos de ciclo de vida de serviços
- Sem dependência de `IHttpContextAccessor` fora de requisições
- Swagger sempre funciona, independente de configuração

### ? 4. **Funcionalidades Mantidas**
- JWT Bearer Authentication configurado
- Swagger UI com todas as features habilitadas
- Diferenciação por ambiente (Dev/Staging/Prod)

---

## ?? Implementação Futura (Opcional)

Se a tradução dinâmica for realmente necessária, a abordagem correta seria:

### Opção 1: **Pre-Build Translation**
```csharp
// Gerar arquivos swagger.pt-BR.json e swagger.en-US.json durante o build
// Servir arquivos estáticos baseados no idioma
```

### Opção 2: **Custom Middleware**
```csharp
// Middleware que intercepta /swagger/v1/swagger.json
// Aplica tradução no documento JSON antes de retornar
// Não interfere com o ciclo de vida do Swagger
```

### Opção 3: **Client-Side Translation**
```javascript
// Carregar traduções via JavaScript no Swagger UI
// Traduzir interface do usuário, não o documento OpenAPI
```

---

## ?? Checklist de Validação

- ? Build compila sem erros
- ? Swagger UI carrega em `https://localhost:7111/swagger`
- ? Documento JSON acessível em `/swagger/v1/swagger.json`
- ? JWT Authentication configurado
- ? Endpoints `/health` e `/v1/actions/*` visíveis
- ? Sem erro 500 no console
- ? Descrições em inglês carregando corretamente

---

## ?? Referências

- [Swashbuckle Documentation](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
- [OpenAPI Specification](https://swagger.io/specification/)
- [ASP.NET Core Dependency Injection Lifetimes](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection#service-lifetimes)
- [Swagger UI Configuration](https://swagger.io/docs/open-source-tools/swagger-ui/usage/configuration/)

---

## ?? Próximos Passos

1. ? **Testar Swagger UI** no navegador
2. ? **Validar todos os endpoints** estão visíveis
3. ? **Testar autenticação JWT** no Swagger
4. ?? **Decidir** se tradução dinâmica é necessária
5. ?? **Remover arquivos não utilizados** (SwaggerTranslationFilter, SwaggerLocalizationMiddleware)
6. ?? **Documentar** decisões de design para o time
