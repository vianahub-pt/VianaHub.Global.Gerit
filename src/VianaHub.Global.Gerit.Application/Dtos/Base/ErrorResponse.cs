namespace VianaHub.Global.Gerit.Application.Dtos.Base;

/// <summary>
/// Modelo padrão para respostas de erro da API
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Título do erro
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Dicionário de erros agrupados por campo/propriedade
    /// </summary>
    public Dictionary<string, string[]> Errors { get; set; } = new();

    /// <summary>
    /// Construtor padrão
    /// </summary>
    public ErrorResponse()
    {
    }

    /// <summary>
    /// Construtor com título
    /// </summary>
    public ErrorResponse(string title)
    {
        Title = title;
    }

    /// <summary>
    /// Construtor com título e erro único
    /// </summary>
    public ErrorResponse(string title, string field, string errorMessage)
    {
        Title = title;
        Errors[field] = new[] { errorMessage };
    }

    /// <summary>
    /// Construtor com título e múltiplos erros
    /// </summary>
    public ErrorResponse(string title, Dictionary<string, string[]> errors)
    {
        Title = title;
        Errors = errors;
    }

    /// <summary>
    /// Adiciona um erro a um campo específico
    /// </summary>
    public void AddError(string field, string errorMessage)
    {
        if (Errors.ContainsKey(field))
        {
            var existingErrors = Errors[field].ToList();
            existingErrors.Add(errorMessage);
            Errors[field] = existingErrors.ToArray();
        }
        else
        {
            Errors[field] = new[] { errorMessage };
        }
    }

    /// <summary>
    /// Adiciona múltiplos erros a um campo específico
    /// </summary>
    public void AddErrors(string field, params string[] errorMessages)
    {
        if (Errors.ContainsKey(field))
        {
            var existingErrors = Errors[field].ToList();
            existingErrors.AddRange(errorMessages);
            Errors[field] = existingErrors.ToArray();
        }
        else
        {
            Errors[field] = errorMessages;
        }
    }
}
