using System.Text.RegularExpressions;

var domainDir = Path.Combine(Directory.GetCurrentDirectory(), "src", "VianaHub.Global.Gerit.Domain");
var messagesPath = Path.Combine(Directory.GetCurrentDirectory(), "src", "VianaHub.Global.Gerit.Api", "Localization", "messages.pt-BR.json");

if (!Directory.Exists(domainDir))
{
    Console.WriteLine("Domain directory not found: " + domainDir);
    return;
}

if (!File.Exists(messagesPath))
{
    Console.WriteLine("Messages file not found: " + messagesPath);
    return;
}

var codeFiles = Directory.GetFiles(domainDir, "*.cs", SearchOption.AllDirectories);
var keyPattern = new Regex("GetMessage\\(\\s*\\\"([A-Za-z0-9_.]+)\\\"", RegexOptions.Compiled);
var keysFound = new HashSet<string>();

foreach (var file in codeFiles)
{
    var text = File.ReadAllText(file);
    foreach (Match m in keyPattern.Matches(text))
    {
        keysFound.Add(m.Groups[1].Value);
    }
}

var jsonRaw = File.ReadAllText(messagesPath);
// Extract keys from JSON-like file using regex to avoid strict JSON parsing
var jsonKeyPattern = new Regex("\"([^\"]+)\"\s*:", RegexOptions.Compiled);
var translations = new HashSet<string>();
foreach (Match m in jsonKeyPattern.Matches(jsonRaw))
{
    translations.Add(m.Groups[1].Value);
}

var missing = keysFound.Where(k => !translations.Contains(k)).OrderBy(k => k).ToList();

Console.WriteLine($"Found {keysFound.Count} keys in domain. Translations: {translations.Count}. Missing: {missing.Count}");
foreach (var k in missing)
    Console.WriteLine(k);
