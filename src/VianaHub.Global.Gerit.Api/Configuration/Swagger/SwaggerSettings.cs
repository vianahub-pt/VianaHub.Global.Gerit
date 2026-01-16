#nullable enable

namespace VianaHub.Global.Gerit.Api.Configuration.Swagger;

/// <summary>
/// Swagger configuration settings from appsettings.json
/// </summary>
public class SwaggerSettings
{
    /// <summary>
    /// API Title (can include environment info)
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// API Description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Environment badge to display (e.g., "🔧 Development", "🚀 Production")
    /// </summary>
    public string? EnvironmentBadge { get; set; }

    /// <summary>
    /// Whether to show environment information in Swagger UI
    /// </summary>
    public bool ShowEnvironment { get; set; } = true;

    /// <summary>
    /// Contact information
    /// </summary>
    public SwaggerContact? Contact { get; set; }

    /// <summary>
    /// License information
    /// </summary>
    public SwaggerLicense? License { get; set; }
}
