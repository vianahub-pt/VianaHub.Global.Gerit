namespace VianaHub.Global.Gerit.Api.Configuration;

/// <summary>
/// Representa as configurações da aplicação.
/// </summary>
public class ApplicationSettings
{
    /// <summary>
    /// Nome da aplicação.
    /// </summary>
    public string Application { get; set; }

    /// <summary>
    /// Nome amigável da aplicação.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Nome do contato responsável.
    /// </summary>
    public string Contact { get; set; }

    /// <summary>
    /// E-mail do contato responsável.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Ambiente da aplicação (ex: Development, Production).
    /// </summary>
    public string Environment { get; set; }
}
