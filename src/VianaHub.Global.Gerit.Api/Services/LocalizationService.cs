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

        // Fallback para pt-PT
        if (culture != "pt-PT")
        {
            var fallbackMessages = GetMessages("pt-PT");
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

            var localizationPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Localization"
            );

            if (!Directory.Exists(localizationPath))
            {
                Log.Error("❌ [Gerit:LocalizationService] Localization folder not found: {Path}", localizationPath);
                _cache[culture] = new Dictionary<string, string>();
                return _cache[culture];
            }

            // Carregar todos os arquivos JSON recursivamente que correspondem ao culture
            var mergedMessages = new Dictionary<string, string>();
            var duplicateKeys = new List<string>();

            // Padrão: {folder}/{folder}.{culture}.json
            // Ex: api/api.en-US.json, application/application.en-US.json, etc.
            var jsonFiles = Directory.GetFiles(localizationPath, $"*.{culture}.json", SearchOption.AllDirectories);

            if (jsonFiles.Length == 0)
            {
                Log.Warning("⚠️ [Gerit:LocalizationService] No JSON files found for culture {Culture} in {Path}", culture, localizationPath);
                
                // Tentar fallback para pt-PT
                if (culture != "pt-PT")
                {
                    Log.Warning("⚠️ [Gerit:LocalizationService] Trying pt-PT fallback");
                    jsonFiles = Directory.GetFiles(localizationPath, "*.pt-PT.json", SearchOption.AllDirectories);
                }

                if (jsonFiles.Length == 0)
                {
                    Log.Error("❌ [Gerit:LocalizationService] No fallback files found for culture {Culture}", culture);
                    _cache[culture] = new Dictionary<string, string>();
                    return _cache[culture];
                }
            }

            Log.Debug("🔍 [Gerit:LocalizationService] Found {Count} JSON files for culture {Culture}", jsonFiles.Length, culture);

            foreach (var filePath in jsonFiles)
            {
                try
                {
                    var json = File.ReadAllText(filePath);
                    // Allow JSON files to contain comments (// ...) and still be parsed
                    var options = new JsonSerializerOptions
                    {
                        ReadCommentHandling = JsonCommentHandling.Skip
                    };
                    var fileMessages = JsonSerializer.Deserialize<Dictionary<string, string>>(json, options);

                    if (fileMessages == null || fileMessages.Count == 0)
                    {
                        Log.Warning("⚠️ [Gerit:LocalizationService] File {File} is empty or invalid", Path.GetFileName(filePath));
                        continue;
                    }

                    foreach (var kvp in fileMessages)
                    {
                        if (mergedMessages.ContainsKey(kvp.Key))
                        {
                            duplicateKeys.Add(kvp.Key);
                            Log.Error("🔴 [Gerit:LocalizationService] DUPLICATE KEY DETECTED: '{Key}' in file {File}", kvp.Key, Path.GetFileName(filePath));
                        }
                        else
                        {
                            mergedMessages[kvp.Key] = kvp.Value;
                        }
                    }

                    Log.Debug("✅ [Gerit:LocalizationService] Loaded {Count} messages from {File}", fileMessages.Count, Path.GetFileName(filePath));
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "❌ [Gerit:LocalizationService] Error loading file {File}", Path.GetFileName(filePath));
                }
            }

            if (duplicateKeys.Count > 0)
            {
                Log.Error("🔴 [Gerit:LocalizationService] Found {Count} duplicate keys for culture {Culture}: {Keys}", 
                    duplicateKeys.Count, culture, string.Join(", ", duplicateKeys.Distinct()));
            }

            _cache[culture] = mergedMessages;
            Log.Information("✅ [Gerit:LocalizationService] Successfully loaded {Count} total messages for culture {Culture} from {FileCount} files",
                mergedMessages.Count, culture, jsonFiles.Length);

            return mergedMessages;
        }
    }
}
