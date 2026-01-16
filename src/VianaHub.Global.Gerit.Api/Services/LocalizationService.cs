using Serilog;
using System.Globalization;
using System.Text.Json;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Services;

/// <summary>
/// Implementação do serviço de localização para a camada de domínio da aplicação Gerit
/// </summary>
public class LocalizationService : ILocalizationService
{
    private static readonly Dictionary<string, Dictionary<string, string>> _cache = new();
    private static readonly object _lock = new();
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LocalizationService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetMessage(string key)
    {
        var culture = GetCurrentCulture();

        var messages = GetMessages(culture);

        if (messages.TryGetValue(key, out var value))
            return value;

        // Fallback para pt-BR
        if (culture != "pt-BR")
        {
            var fallbackMessages = GetMessages("pt-BR");
            if (fallbackMessages.TryGetValue(key, out var fallbackValue))
                return fallbackValue;
        }
        return key;
    }

    public string GetMessage(string key, params object[] args)
    {
        var message = GetMessage(key);
        try
        {
            return string.Format(message, args);
        }
        catch
        {
            return message;
        }
    }

    private string GetCurrentCulture()
    {
        // First, try to get culture from HttpContext.Items (set by RequestLocalizationMiddleware)
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.Items.TryGetValue("RequestCulture", out var cultureFromContext) == true
            && cultureFromContext is string cultureName)
        {
            return cultureName;
        }

        // Fallback to CurrentUICulture
        var fallbackCulture = CultureInfo.CurrentUICulture.Name;
        return fallbackCulture;
    }

    private Dictionary<string, string> GetMessages(string culture)
    {
        if (_cache.TryGetValue(culture, out var cached))
            return cached;

        lock (_lock)
        {
            // Double check após lock
            if (_cache.TryGetValue(culture, out cached))
                return cached;

            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Localization",
                $"messages.{culture}.json"
            );

            // Se não encontrar o arquivo, tenta fallback para pt-BR
            if (!File.Exists(filePath))
            {
                Log.Warning("⚠️ [Gerit:LocalizationService] File not found: {FilePath}, trying pt-BR fallback", filePath);
                filePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Localization",
                    "messages.pt-BR.json"
                );
            }

            if (!File.Exists(filePath))
            {
                Log.Error("❌ [Gerit:LocalizationService] Fallback file not found: {FilePath}", filePath);
                // Cria dicionário vazio se não encontrar arquivo algum
                _cache[culture] = new Dictionary<string, string>();
                return _cache[culture];
            }

            var json = File.ReadAllText(filePath);
            var messages = JsonSerializer.Deserialize<Dictionary<string, string>>(json)
                ?? new Dictionary<string, string>();

            _cache[culture] = messages;
            Log.Debug("✅ [Gerit:LocalizationService] Loaded {Count} messages for {Culture}", messages.Count, culture);
            return messages;
        }
    }
}
