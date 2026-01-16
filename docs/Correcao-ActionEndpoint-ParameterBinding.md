# Correção do Erro de Inferência de Parâmetros - ActionEndpoint

## ?? Erro Original

```
System.InvalidOperationException: Body was inferred but the method does not allow inferred body parameters.
Below is the list of parameters that we found: 

Parameter           | Source                        
---------------------------------------------------------------------------------
request             | Body (Inferred)
appService          | Services (Inferred)
notify              | Services (Inferred)

Did you mean to register the "Body (Inferred)" parameter(s) as a Service or apply the [FromServices] or [FromBody] attribute?
```

**Local:** `src\VianaHub.Global.Gerit.Api\Endpoints\ActionEndpoint.cs`

**Causa:** O ASP.NET Core **não conseguiu inferir** de onde vêm os parâmetros no endpoint `/v1/actions/paged`.

---

## ?? Análise do Problema

### Endpoint Problemático:

```csharp
// ? ANTES - Sem atributos de binding
groupV1.MapGet("/paged", async (PagedFilterRequest request, IActionAppService appService, INotify notify, CancellationToken ct) =>
{
    var response = await appService.GetPagedAsync(request, ct);
    return notify.CustomResponse(response, 200);
})
```

### Por que o erro ocorreu?

1. **Ambiguidade de Fonte:**
   - `PagedFilterRequest request` ? O ASP.NET Core inferiu como `[FromBody]`
   - Mas é um **GET**, que não deveria ter body
   - Na verdade, deveria vir da **Query String** (`?PageNumber=1&PageSize=10&Search=...`)

2. **Múltiplos Parâmetros Complexos:**
   - Quando há múltiplos parâmetros e um deles é um **tipo complexo** (classe), o ASP.NET Core fica confuso
   - Ele tenta inferir automaticamente mas falha quando há ambiguidade

3. **Regras de Inferência do ASP.NET Core:**
   | Tipo de Parâmetro | Inferência Padrão |
   |-------------------|-------------------|
   | Tipos primitivos (int, string, bool) | `[FromRoute]` ou `[FromQuery]` |
   | Tipos complexos (classes) em **GET** | `[FromQuery]` ou `[AsParameters]` |
   | Tipos complexos em **POST/PUT** | `[FromBody]` |
   | `IFormFile` | `[FromForm]` |
   | Serviços registrados no DI | `[FromServices]` |
   | `CancellationToken` | Automático |

---

## ? Solução Aplicada

### 1. Endpoint `/paged` (GET com Query String)

```csharp
// ? DEPOIS - Com [AsParameters]
groupV1.MapGet("/paged", async ([AsParameters] PagedFilterRequest request, IActionAppService appService, INotify notify, CancellationToken ct) =>
{
    var response = await appService.GetPagedAsync(request, ct);
    return notify.CustomResponse(response, 200);
})
```

**`[AsParameters]`:**
- Indica que as propriedades da classe devem ser **extraídas da Query String**
- Funciona como um "explode" do objeto em múltiplos parâmetros
- Exemplo: `/v1/actions/paged?PageNumber=1&PageSize=20&Search=Admin`

**Estrutura do `PagedFilterRequest`:**
```csharp
public class PagedFilterRequest : Paging
{
    public string Search { get; set; }  // Query param: ?Search=...
}

public class Paging : Order
{
    public int? PageNumber { get; set; }  // Query param: ?PageNumber=1
    public int? PageSize { get; set; }    // Query param: ?PageSize=20
}
```

---

### 2. Endpoint `/bulk-upload` (POST com Multipart/Form-Data)

```csharp
// ? ANTES (estava correto, mas adicionei [FromForm] para clareza)
groupV1.MapPost("/bulk-upload", async ([FromForm] IFormFile file, IActionAppService appService, INotify notify, CancellationToken ct) =>
{
    var success = await appService.BulkUploadAsync(file, ct);
    return notify.CustomResponse(success ? 200 : 400);
})
.DisableAntiforgery()
.Accepts<IFormFile>("multipart/form-data")
```

**`[FromForm]`:**
- Indica que o parâmetro vem de um **formulário multipart**
- Necessário para upload de arquivos
- `.DisableAntiforgery()` é usado porque não estamos enviando token CSRF

---

## ?? Atributos de Binding em Minimal APIs

| Atributo | Uso | Exemplo |
|----------|-----|---------|
| `[FromRoute]` | Parâmetros da URL | `GET /users/{id}` ? `[FromRoute] int id` |
| `[FromQuery]` | Query string | `GET /users?name=John` ? `[FromQuery] string name` |
| `[FromBody]` | Body JSON | `POST /users` ? `[FromBody] CreateUserRequest request` |
| `[FromForm]` | Form data | `POST /upload` ? `[FromForm] IFormFile file` |
| `[FromHeader]` | Headers HTTP | `[FromHeader(Name = "X-Tenant-Id")] int tenantId` |
| `[FromServices]` | Dependency Injection | `[FromServices] IUserService service` |
| `[AsParameters]` | Múltiplos parâmetros de um objeto | `[AsParameters] PagedFilterRequest request` |

---

## ?? Como Testar

### 1. Testar Endpoint `/paged` (GET com Query String)

```bash
# Teste básico
curl -X GET "https://localhost:7000/v1/actions/paged?PageNumber=1&PageSize=10"

# Com filtro de busca
curl -X GET "https://localhost:7000/v1/actions/paged?PageNumber=1&PageSize=20&Search=Admin"

# Teste de limite máximo (MaxPageSize = 1000)
curl -X GET "https://localhost:7000/v1/actions/paged?PageSize=5000"
# Deve retornar apenas 1000 itens
```

### 2. Testar Endpoint `/bulk-upload` (POST com arquivo)

```bash
# Upload de arquivo CSV
curl -X POST "https://localhost:7000/v1/actions/bulk-upload" \
  -H "Content-Type: multipart/form-data" \
  -F "file=@actions.csv"
```

### 3. Testar Swagger UI

- Acesse: `https://localhost:7000/swagger`
- Endpoint `/v1/actions/paged` deve mostrar:
  - ? Parâmetros na **Query String** (não no Body)
  - ? `PageNumber`, `PageSize`, `Search` como campos separados

---

## ?? Outros Endpoints Verificados

Todos os outros endpoints estão **corretos** e **não precisaram de correção**:

| Endpoint | Método | Binding | Status |
|----------|--------|---------|--------|
| `/v1/actions/` | GET | Apenas serviços DI | ? OK |
| `/v1/actions/{id}` | GET | `id` inferido como `[FromRoute]` | ? OK |
| `/v1/actions/` | POST | `[FromBody]` explícito | ? OK |
| `/v1/actions/{id}` | PUT | `id` + `[FromBody]` explícito | ? OK |
| `/v1/actions/{id}/activate` | PATCH | `id` inferido como `[FromRoute]` | ? OK |
| `/v1/actions/{id}/deactivate` | PATCH | `id` inferido como `[FromRoute]` | ? OK |
| `/v1/actions/{id}` | DELETE | `id` inferido como `[FromRoute]` | ? OK |
| `/v1/actions/bulk-upload` | POST | `[FromForm]` adicionado | ? OK |

---

## ?? Boas Práticas Aplicadas

### ? 1. Sempre Use Atributos de Binding Explícitos

```csharp
// ? EVITE (ambíguo)
app.MapPost("/users", async (CreateUserRequest request, IUserService service) => { ... });

// ? PREFIRA (explícito)
app.MapPost("/users", async ([FromBody] CreateUserRequest request, IUserService service) => { ... });
```

### ? 2. Use `[AsParameters]` para Query Strings Complexas

```csharp
// ? EVITE (difícil de manter)
app.MapGet("/users", async (int? page, int? size, string search, string order) => { ... });

// ? PREFIRA (mais limpo)
app.MapGet("/users", async ([AsParameters] UserFilterRequest filter) => { ... });
```

### ? 3. Valide Parâmetros na Própria Classe

```csharp
public class PagedFilterRequest
{
    private const int MaxPageSize = 1000;
    private int? _pageSize;
    
    public int? PageSize 
    { 
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}
```

---

## ?? Resumo

| Item | Antes | Depois |
|------|-------|--------|
| **Erro** | `InvalidOperationException` | ? Resolvido |
| **Endpoint `/paged`** | Binding ambíguo | `[AsParameters]` adicionado |
| **Endpoint `/bulk-upload`** | Implícito | `[FromForm]` adicionado |
| **Build** | ? Falhando | ? Sucesso |
| **Testes** | Não executáveis | ? Prontos |

---

## ?? Referências

- [Minimal APIs Parameter Binding](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/parameter-binding)
- [AsParameters Attribute](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/parameter-binding#asparameters)
- [Explicit Parameter Binding](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/parameter-binding#explicit-parameter-binding)
